using EPGManager.Models;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using System.Collections.Generic;
using System.Linq;

namespace EPGBot.Models
{
    public class ProgrammeResult : EPGResult
    {
        public ProgrammeResult(List<Programme> programmes)
        {
            programmesCard = new List<HeroCard>();
            programmes.ForEach(programme =>
            programmesCard.Add(new HeroCard
            {
                Title = programme.Title,
                Subtitle = programme.SubTitle,
                Text = programme.Description +
                        $" \n Duração {programme.Start} : { programme.Stop}" +
                        $" \n Categoria {programme.Category}.",
                Images = new List<CardImage> { new CardImage(programme.Icon) }

            }));
        }

        private List<HeroCard> programmesCard { get; set; }

        public override IMessageActivity Message()
        {
            if (!programmesCard.Any())
            {
                var Empty = "Não há atrações para serem exibidas";
                return MessageFactory.Text(Empty, Empty, InputHints.ExpectingInput);
            }

            // Cards are sent as Attachments in the Bot Framework.
            // So we need to create a list of attachments for the reply activity.
            var attachments = new List<Attachment>();
            // Reply to the activity we received with an activity.
            var reply = MessageFactory.Attachment(attachments);

            if (programmesCard.Count() > 1)
                reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;

            programmesCard.ForEach(x => reply.Attachments.Add(x.ToAttachment()));

            return reply;
        }
    }
}
