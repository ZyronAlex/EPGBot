using EPGManager.Models;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using System.Collections.Generic;
using System.Linq;

namespace EPGBot.Models
{
    public class ChannelResult : EPGResult
    {
        public ChannelResult(Channel channel) : this(new List<Channel>() { channel }) { }

        public ChannelResult(List<Channel> channels)
        {
            channelsCard = new List<HeroCard>();
            channels.ForEach(channel =>
            channelsCard.Add(new HeroCard
            {
                Title = channel.Name,
                Images = new List<CardImage> { new CardImage(channel.Icon) }
            }));
        }

        private List<HeroCard> channelsCard { get; set; }

        public override IMessageActivity Message()
        {
            if (!channelsCard.Any())
            {
                var Empty = "Não há canais para serem exibidos";
                return MessageFactory.Text(Empty, Empty, InputHints.ExpectingInput);
            }

            // Cards are sent as Attachments in the Bot Framework.
            // So we need to create a list of attachments for the reply activity.
            var attachments = new List<Attachment>();
            // Reply to the activity we received with an activity.
            var reply = MessageFactory.Attachment(attachments);

            if (channelsCard.Count() > 1)
                reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;

            channelsCard.ForEach(x => reply.Attachments.Add(x.ToAttachment()));

            return reply;
        }
    }
}
