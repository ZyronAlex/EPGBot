// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with Bot Builder V4 SDK Template for Visual Studio CoreBot v4.12.2

using EPGBot.Models;
using EPGManager.Interfaces;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Microsoft.Recognizers.Text.DataTypes.TimexExpression;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EPGBot.Dialogs
{
    public class ScheduleChannelDialog : BaseDialog
    {
        private const int pageSize = 10;

        private readonly IEPGRepository _repository;
        private readonly IStatePropertyAccessor<UserProfile> _userProfileAccessor;
        private UserProfile userProfile;

        public ScheduleChannelDialog(UserState userState, FindChannelDialog findChannelDialog, IEPGRepository repository)
            : base(nameof(ScheduleChannelDialog))
        {
            HelpMsgText = "Escreva \"ver mais\" para ver mais atrções ou \"voltar\" para ver outra opções";
            CancelMsgText = "Sem mais atrações pra mostrar";

            _userProfileAccessor = userState.CreateProperty<UserProfile>(nameof(UserProfile));
            _repository = repository;

            var schedulerChannelWaterfallSteps = new WaterfallStep[]
            {
                IntroStepAsync,
                InitialStepAsync,
                ActStepAsync,
                FinalStepAsync
            };

            AddDialog(new WaterfallDialog(nameof(schedulerChannelWaterfallSteps), schedulerChannelWaterfallSteps));

            AddDialog(new TextPrompt(nameof(TextPrompt)));
            AddDialog(new ConfirmPrompt(nameof(ConfirmPrompt)));
            AddDialog(findChannelDialog);

            // The initial child Dialog to run.
            InitialDialogId = nameof(schedulerChannelWaterfallSteps);
        }

        private async Task<DialogTurnResult> IntroStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var scheduleChannelOptions = (ScheduleChannelOptions)stepContext.Options;
            if (string.IsNullOrEmpty(scheduleChannelOptions.ChannelId))
                return await stepContext.BeginDialogAsync(nameof(FindChannelDialog), null, cancellationToken);

            return await stepContext.NextAsync(scheduleChannelOptions, cancellationToken);
        }

        private async Task<DialogTurnResult> InitialStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var scheduleChannelOptions = (ScheduleChannelOptions)stepContext.Options;
            if (string.IsNullOrEmpty(scheduleChannelOptions.ChannelId))
            {
                scheduleChannelOptions.ChannelId = (string)stepContext.Result;
                userProfile = await _userProfileAccessor.GetAsync(stepContext.Context, () => new UserProfile(), cancellationToken);
                await ShowProgrammes(scheduleChannelOptions, stepContext, cancellationToken);
            }

            var promptMessage = MessageFactory.Text(HelpMsgText, HelpMsgText, InputHints.ExpectingInput);
            return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = promptMessage }, cancellationToken);
        }

        private async Task<DialogTurnResult> ActStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var result = (string)stepContext.Result;
            var scheduleChannelOptions = (ScheduleChannelOptions)stepContext.Options;
            switch (result.ToLower())
            {
                case "ver mais":
                    scheduleChannelOptions.CurrentPage++;
                    if (await ShowProgrammes(scheduleChannelOptions, stepContext, cancellationToken))
                        return await stepContext.ReplaceDialogAsync(InitialDialogId, scheduleChannelOptions, cancellationToken);
                    return await stepContext.NextAsync(null, cancellationToken);
                case "voltar":
                    return await stepContext.NextAsync(null, cancellationToken);
                default:
                    var didntUnderstandMessageText = $"Me desculpa, Eu nao entendi. Por favor pergunte novamente";
                    await stepContext.Context.SendActivityAsync(didntUnderstandMessageText, didntUnderstandMessageText, cancellationToken: cancellationToken);
                    return await stepContext.ReplaceDialogAsync(InitialDialogId, scheduleChannelOptions, cancellationToken);
            }
        }

        private async Task<DialogTurnResult> FinalStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            await stepContext.Context.SendActivityAsync(CancelMsgText, CancelMsgText, cancellationToken: cancellationToken);

            return await stepContext.EndDialogAsync(null, cancellationToken);
        }

        private async Task<bool> ShowProgrammes(ScheduleChannelOptions scheduleChannelOptions, WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var programmes = _repository.ListProgramme(chanelId: scheduleChannelOptions.ChannelId, 
                currentPage: scheduleChannelOptions.CurrentPage, PageSize: pageSize, underage: userProfile.IsAdult);

            if (programmes.Any())
            {
                var channelMessage = "Aqui estão alguns Atrações:";
                await stepContext.Context.SendActivityAsync(channelMessage, channelMessage, cancellationToken: cancellationToken);

                var channelResult = new ProgrammeResult(programmes);

                // Send the card(s) to the user as an attachment to the activity
                await stepContext.Context.SendActivityAsync(channelResult.Message(), cancellationToken);
            }

            return programmes.Any();
        }
    }
}
