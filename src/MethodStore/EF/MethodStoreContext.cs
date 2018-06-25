using Microsoft.EntityFrameworkCore;

namespace MethodStore.EF
{
    public class MethodStoreContext : DbContext
    {
        private readonly string _fileNameDatabase = "Methods.db";

        public DbSet<Models.Method> Methods { get; set; }
        public DbSet<Models.Group> Groups { get; set; }
        public DbSet<Models.Types> Types { get; set; }

        public MethodStoreContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=" + _fileNameDatabase);
        }
    }
}