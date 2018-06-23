using Microsoft.EntityFrameworkCore;

namespace MethodStore.EF
{
    public class MethodStoreContext : DbContext
    {
        private readonly string _fileNameDatabase = "Methods.db";
        //private readonly string _fileNameDatabase = @"F:\C#\GitHub\MethodStoreV2\src\MethodStore\bin\localdb\Methods.db";

        public DbSet<Models.Method> Methods { get; set; }

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