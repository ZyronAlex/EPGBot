using EPGBot.Models;
using EPGManager.Interfaces;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
using Microsoft.Bot.Schema;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EPGBot.Dialogs
{
    public class ScheudleDialog : BaseDialog
    {
        private readonly IEPGRepository _repository;
        private readonly IStatePropertyAccessor<UserProfile> _userProfileAccessor;
        private UserProfile userProfile;

        public ScheudleDialog(UserState userState,
            ScheduleChannelDialog scheduleChannelDialog,
            IEPGRepository repository)
            : base(nameof(ScheudleDialog))
        {

            _userProfileAccessor = userState.CreateProperty<UserProfile>(nameof(UserProfile));
            _repository = repository;

            var scheduleWaterfallSteps = new WaterfallStep[]
           {
                IntroStepAsync,
                ActStepAsync,
                FinalStepAsync
           };
            AddDialog(new WaterfallDialog(nameof(scheduleWaterfallSteps), scheduleWaterfallSteps));

            AddDialog(new TextPrompt(nameof(TextPrompt)));
            AddDialog(new ChoicePrompt(nameof(ChoicePrompt)));
            AddDialog(scheduleChannelDialog);

            // The initial child Dialog to run.
            InitialDialogId = nameof(scheduleWaterfallSteps);
        }

        private async Task<DialogTurnResult> IntroStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            userProfile = await _userProfileAccessor.GetAsync(stepContext.Context, () => new UserProfile(), cancellationToken);

            return await stepContext.PromptAsync(nameof(ChoicePrompt),
                   new PromptOptions
                   {
                       Prompt = MessageFactory.Text("O que você Deseja ?"),
                       Choices = GetScheduleChoices(),
                   }, cancellationToken);
        }

        private async Task<DialogTurnResult> ActStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var command = ((FoundChoice)stepContext.Result).Value;

            switch (command)
            {
                case ScheduleCommands.Now:
                    var result = await GetProgramme();
                    return await stepContext.NextAsync(result, cancellationToken);

                case ScheduleCommands.NowChannel:
                    return await stepContext.BeginDialogAsync(nameof(ScheduleChannelDialog), new ScheduleChannelOptions(), cancellationToken);

                default:
                    // Catch all for unhandled intents
                    var didntUnderstandMessageText = $"Me desculpa, Eu nao entendi. Por favor pergunte novamente (Tente usar umas das sugestões)";
                    var didntUnderstandMessage = MessageFactory.Text(didntUnderstandMessageText, didntUnderstandMessageText, InputHints.IgnoringInput);
                    await stepContext.Context.SendActivityAsync(didntUnderstandMessage, cancellationToken);
                    break;
            }

            return await stepContext.NextAsync(null, cancellationToken);
        }

        private async Task<DialogTurnResult> FinalStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            if (stepContext.Result is ProgrammeResult result)
                return await stepContext.EndDialogAsync(result, cancellationToken);

            return await stepContext.EndDialogAsync(null, cancellationToken);
        }

        private async Task<ProgrammeResult> GetProgramme()
        {
            var programme = _repository.ListProgramme(underage: userProfile.IsAdult);

            return new ProgrammeResult(programme);
        }

        private IList<Choice> GetScheduleChoices()
        {
            var cardOptions = new List<Choice>()
            {
                new Choice() { Value = ScheduleCommands.Now, Synonyms = ScheduleCommands.NowSynonyms },
                new Choice() { Value = ScheduleCommands.NowChannel, Synonyms =ScheduleCommands.NowChannelSynonyms },
            };

            return cardOptions;
        }
    }
}
