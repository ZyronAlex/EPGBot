using System.Collections.Generic;

namespace EPGBot.Models
{
    public class ScheduleCommands
    {
        public const string Now = "Ver o que está pasando agora em todos os canais";
        public static readonly List<string> NowSynonyms = new List<string> { "todos os canais" };

        public const string NowChannel = "Ver o que está pasando agora em um canal específico";
        public static readonly List<string> NowChannelSynonyms = new List<string> { "em um canal" };
    }
}
