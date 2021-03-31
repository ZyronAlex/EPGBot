using EPGBot.Models;
using EPGManager.Interfaces;
using EPGManager.Models;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EPGBot.Dialogs
{
    public class FindChannelDialog : BaseDialog
    {
        private readonly IEPGRepository _repository;
        private readonly IStatePropertyAccessor<UserProfile> _userProfileAccessor;
        private UserProfile userProfile;

        public FindChannelDialog(UserState userState, ListChannelDialog listChannelDialog, IEPGRepository repository)
            : base(nameof(FindChannelDialog))
        {
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
            AddDialog(listChannelDialog);

            // The initial child Dialog to run.
            InitialDialogId = nameof(channelWaterfallSteps);
        }

        private async Task<DialogTurnResult> InitialStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var ms = "Qual o nome do canal que você gostaria, se quiser me diga \"ver lista\"  ";
            var promptMessage = MessageFactory.Text(HelpMsgText, HelpMsgText, InputHints.ExpectingInput);
            return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = promptMessage }, cancellationToken);
        }

        private async Task<DialogTurnResult> ActStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var channelName = (string)stepContext.Result;

            switch (channelName.ToLower())
            {
                case "ver lista":
                    return await stepContext.BeginDialogAsync(nameof(ListChannelDialog), 0, cancellationToken);
                default:
                    var channel = _repository.GetChannel(channelName);
                    if (channel == null)
                    {
                        var didntUnderstandMessageText = $"Me desculpa, Eu não entendi. Vamos tentar novamente ou digite \"sair\", para terminamos";
                        await stepContext.Context.SendActivityAsync(didntUnderstandMessageText, didntUnderstandMessageText, cancellationToken: cancellationToken);
                        return await stepContext.ReplaceDialogAsync(InitialDialogId, null, cancellationToken);
                    }
                    else
                    {
                        stepContext.Values["channel"] = channel;
                        await stepContext.Context.SendActivityAsync(new ChannelResult(channel).Message(), cancellationToken);

                        var ms = "Esse é o canal que vc estava procurando";
                        var promptMessage = MessageFactory.Text(ms, ms, InputHints.ExpectingInput);
                        return await stepContext.PromptAsync(nameof(ConfirmPrompt), new PromptOptions { Prompt = promptMessage }, cancellationToken);
                    }
            }

        }

        private async Task<DialogTurnResult> FinalStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            if (stepContext.Result is bool confimed)
            {
                if (confimed)
                {
                    var channel = (Channel)stepContext.Options;
                    return await stepContext.EndDialogAsync(channel.Id, cancellationToken);
                }
                else
                {
                    var didntUnderstandMessageText = $"Certo Vamos tentar novamente ou digite \"sair\", para terminamos";
                    await stepContext.Context.SendActivityAsync(didntUnderstandMessageText, didntUnderstandMessageText, cancellationToken: cancellationToken);
                    return await stepContext.ReplaceDialogAsync(InitialDialogId, null, cancellationToken);
                }
            }
            else
                return await stepContext.ReplaceDialogAsync(InitialDialogId, null, cancellationToken);
        }

    }
}
