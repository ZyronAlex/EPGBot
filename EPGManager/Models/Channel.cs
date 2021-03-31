using System.Collections.Generic;

namespace EPGManager.Models
{
    public class Channel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public List<Programme> Programmes { get; set; }
    }
}
