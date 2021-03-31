using EPGManager.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPGManager.Interfaces
{
    public interface IEPGRepository
    {
        Task AddChannels(List<Channel> channels);
        Task AddProgrammes(List<Programme> programmes);
        Task<Channel> GetChannel(string chanelName);
        Task<List<Channel>> ListChannels(int currentPage, int PageSize, bool underage);
        Task<List<Programme>> ListProgramme(bool underage);
        Task<List<Programme>> ListProgramme(string chanelId, int currentPage, int PageSize, bool underage);
    }
}
