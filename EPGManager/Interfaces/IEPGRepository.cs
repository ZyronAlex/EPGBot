using EPGManager.Models;
using System.Collections.Generic;

namespace EPGManager.Interfaces
{
    public interface IEPGRepository
    {
        List<Channel> ListChannels(int currentPage, int PageSize, bool underage);
        List<Programme> ListProgramme(bool underage);
    }
}
