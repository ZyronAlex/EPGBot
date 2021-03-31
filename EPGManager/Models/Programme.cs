using System;
using System.Collections.Generic;
using System.Text;

namespace EPGManager.Models
{
    public class Programme
    {
        public int Id { get; set; }
        public DateTime Start { get; set; }
        public DateTime Stop { get; set; }
        public string ChannelId { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string Icon { get; set; }
    }
}
