using Microsoft.EntityFrameworkCore;

namespace EPGManager.Models
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options) {}
        public DbSet<Channel> Channel { get; set; }
        public DbSet<Programme> Programme { get; set; }
    }
}