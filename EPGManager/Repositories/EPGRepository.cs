using EPGManager.Interfaces;
using EPGManager.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPGManager.Repositories
{
    public class EPGRepository : IEPGRepository
    {
        protected readonly Context _context;
        protected readonly ILogger _logger;

        public EPGRepository(Context context, ILogger<EPGRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task AddChannels(List<Channel> channels)
        {
            try
            {
                _context.Channels.AddRange(channels);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
            }
        }

        public async Task<Channel> GetChannel(string chanelName)
        {
            try
            {
                return await _context.Channels.SingleAsync(x => x.Name.Contains(chanelName));
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return null;
            }
        }

        public async Task<List<Channel>> ListChannels(int currentPage, int PageSize, bool underage)
        {
            try
            {
                return await _context.Channels.Skip(currentPage * PageSize).Take(PageSize).ToListAsync();
            }
            catch(Exception e)
            {
                _logger.LogError(e.Message);
                return new List<Channel>();
            }
        }

        public async Task AddProgrammes(List<Programme> programmes)
        {
            try
            {
                _context.Programmes.AddRange(programmes);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
            }
        }

        public async Task<List<Programme>> ListProgramme(bool underage)
        {
            try
            {
                return await _context.Programmes.ToListAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return new List<Programme>();
            }
        }

        public async Task<List<Programme>> ListProgramme(string chanelId, int currentPage, int PageSize, bool underage)
        {
            try
            {
                return await _context.Programmes.Where(x => x.ChannelId.Equals(chanelId)).Skip(currentPage * PageSize).Take(PageSize).ToListAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return new List<Programme>();
            }
        }
    }
}
