using Microsoft.EntityFrameworkCore;

namespace EPGManager.Models
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options) {}
        public DbSet<Channel> Channels { get; set; }
        public DbSet<Programme> Programmes { get; set; }
    }
}