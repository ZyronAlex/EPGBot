using EPGBot.Models;
using EPGManager.Interfaces;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EPGBot.Dialogs
{
    public class ListChannelDialog : BaseDialog
    {
        private const int pageSize = 10;

        private readonly IEPGRepository _repository;
        private readonly IStatePropertyAccessor<UserProfile> _userProfileAccessor;
        private UserProfile userProfile;

        public ListChannelDialog(UserState userState, IEPGRepository repository)
            : base(nameof(ListChannelDialog))
        {
            HelpMsgText = "Escreva \"ver mais\" para ver mais canais ou \"voltar\" para ver outra opções";
            CancelMsgText = "Sem mais Canais pra mostrar";

            _userProfileAccessor = userState.CreateProperty<UserProfile>(nameof(UserProfile));
            _repository = repository;

            var channelWaterfallSteps = new WaterfallStep[]
            {
                InitialStepAsync,
                ActStepAsync,
                FinalStepAsync
            };

            AddDialog(new WaterfallDialog(nameof(channelWaterfallSteps), channelWaterfallSteps));

            AddDialog(new TextPrompt(nameof(TextPrompt)));
            AddDialog(new ConfirmPrompt(nameof(ConfirmPrompt)));

            // The initial child Dialog to run.
            InitialDialogId = nameof(channelWaterfallSteps);
        }

        private async Task<DialogTurnResult> InitialStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {            
            var currentPage = (int)stepContext.Options;
            if (currentPage == 0)
            {
                userProfile = await _userProfileAccessor.GetAsync(stepContext.Context, () => new UserProfile(), cancellationToken);
                await ShowChannels(currentPage, stepContext, cancellationToken);
            }

            var promptMessage = MessageFactory.Text(HelpMsgText, HelpMsgText, InputHints.ExpectingInput);
            return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = promptMessage }, cancellationToken);
        }

        private async Task<DialogTurnResult> ActStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var result = (string)stepContext.Result;
            var currentPage = (int)stepContext.Options;

            switch (result.ToLower())
            {
                case "ver mais":
                    currentPage++;
                    if (await ShowChannels(currentPage, stepContext, cancellationToken))
                        return await stepContext.ReplaceDialogAsync(InitialDialogId, currentPage, cancellationToken);
                    return await stepContext.NextAsync(null, cancellationToken);
                case "voltar":
                    return await stepContext.NextAsync(null, cancellationToken);
                default:
                    var didntUnderstandMessageText = $"Me desculpa, Eu nao entendi. Por favor pergunte novamente";
                    await stepContext.Context.SendActivityAsync(didntUnderstandMessageText, didntUnderstandMessageText, cancellationToken: cancellationToken);
                    return await stepContext.ReplaceDialogAsync(InitialDialogId, currentPage, cancellationToken);
            }
        }

        private async Task<DialogTurnResult> FinalStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            await stepContext.Context.SendActivityAsync(CancelMsgText, CancelMsgText, cancellationToken: cancellationToken);

            return await stepContext.EndDialogAsync(null, cancellationToken);
        }

        private async Task<bool> ShowChannels(int currentPage, WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var channels = _repository.ListChannels(currentPage: currentPage, PageSize: pageSize, underage: userProfile.IsAdult);

            if (channels.Any())
            {
                var channelMessage = "Aqui estão alguns canais:";
                await stepContext.Context.SendActivityAsync(channelMessage, channelMessage, cancellationToken: cancellationToken);

                var channelResult = new ChannelResult(channels);

                // Send the card(s) to the user as an attachment to the activity
                await stepContext.Context.SendActivityAsync(channelResult.Message(), cancellationToken);
            }

            return channels.Any();
        }
    }
}
