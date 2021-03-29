using EPGBot.Models;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EPGBot.Dialogs
{
    public class MainDialog : BaseDialog
    {
        protected readonly ILogger _logger;
        private readonly IStatePropertyAccessor<UserProfile> _userProfileAccessor;

        // Dependency injection uses this constructor to instantiate MainDialog
        public MainDialog(UserState userState, 
            UserProfileDialog userProfileDialog, 
            ChannelDialog channelDialog, 
            ScheudleDialog scheudleDialog,
            ILogger<MainDialog> logger)
            : base(nameof(MainDialog))
        {
            HelpMsgText = "";
            CancelMsgText = "";

            _logger = logger;
            _userProfileAccessor = userState.CreateProperty<UserProfile>(nameof(UserProfile));

            var mainWaterfallSteps = new WaterfallStep[]
            {
                IntroStepAsync,
                ActStepAsync,
                FinalStepAsync
            };
            AddDialog(new WaterfallDialog(nameof(mainWaterfallSteps), mainWaterfallSteps));

            AddDialog(new TextPrompt(nameof(TextPrompt)));
            AddDialog(new ChoicePrompt(nameof(ChoicePrompt)));

            AddDialog(userProfileDialog);
            AddDialog(channelDialog);
            AddDialog(scheudleDialog); 

             // The initial child Dialog to run.
             InitialDialogId = nameof(mainWaterfallSteps);
        }

        private async Task<DialogTurnResult> IntroStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var userProfile = await _userProfileAccessor.GetAsync(stepContext.Context, () => new UserProfile(), cancellationToken);
            if (string.IsNullOrEmpty(userProfile.Name))
                return await stepContext.BeginDialogAsync(nameof(UserProfileDialog), null, cancellationToken);

            return await stepContext.PromptAsync(nameof(ChoicePrompt),
                   new PromptOptions
                   {
                       Prompt = MessageFactory.Text("Como eu posso te ajudar?"),
                       Choices = GetMainChoices(),
                   }, cancellationToken);
        }

        private async Task<DialogTurnResult> ActStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var command = ((FoundChoice)stepContext.Result).Value;

            switch (command)
            {
                case EPGCommands.Channel:
                    return await stepContext.BeginDialogAsync(nameof(ChannelDialog), 0, cancellationToken);

                case EPGCommands.Scheduler:
                    return await stepContext.BeginDialogAsync(nameof(ScheudleDialog), 0, cancellationToken);

                case EPGCommands.Show:
                    // We haven't implemented the GetWeatherDialog so we just display a TODO message.
                    var messageText = "TODO: get weather flow here";
                    var message = MessageFactory.Text(messageText, messageText, InputHints.IgnoringInput);
                    await stepContext.Context.SendActivityAsync(message, cancellationToken);
                    break;

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
            // If the child dialog ("BookingDialog") was cancelled, the user failed to confirm or if the intent wasn't BookFlight
            // the Result here will be null.
            if (stepContext.Result is EPGResult result)
            {
                // Now we have all the booking details call the booking service.

                // If the call to the booking service was successful tell the user.

                //var timeProperty = new TimexProperty(result.Schedule);
                //var travelDateMsg = timeProperty.ToNaturalLanguage(DateTime.Now);
                //var messageText = $"I have you booked to {result.Channel} from {result.Programer} on {travelDateMsg}";
                //var message = MessageFactory.Text(messageText, messageText, InputHints.IgnoringInput);

                var message = result.Message();
                await stepContext.Context.SendActivityAsync(message, cancellationToken);
            }

            // Restart the main dialog with a different message the second time around
            var promptMessage = "Posso te ajudar com mais alguma coias?";
            return await stepContext.ReplaceDialogAsync(InitialDialogId, promptMessage, cancellationToken);
        }
    }
}
