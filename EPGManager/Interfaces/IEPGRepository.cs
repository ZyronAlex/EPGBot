using EPGManager.Models;
using System.Collections.Generic;

namespace EPGManager.Interfaces
{
    public interface IEPGRepository
    {
        Channel GetChannel(string chanelName);
        List<Channel> ListChannels(int currentPage, int PageSize, bool underage);
        List<Programme> ListProgramme(bool underage);
        List<Programme> ListProgramme(string chanelId, int currentPage, int PageSize, bool underage);
    }
}
