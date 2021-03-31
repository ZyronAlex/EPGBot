using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace EPGBot.Models
{
    public static class EPGCommands
    {
        public const string Channel = "Canais disponível";
        public static readonly List<string> ChannelSynonyms = new List<string> { "Canais", "Canal" };

        public const string Scheduler = "Programação e horário do(s) Canai(s)";
        public static readonly List<string> SchedulerSynonyms = new List<string> { "Programação", "horário" };
    }
}
