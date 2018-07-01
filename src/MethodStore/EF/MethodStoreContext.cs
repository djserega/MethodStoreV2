using Microsoft.EntityFrameworkCore;

namespace MethodStore.EF
{
    public class MethodStoreContext : DbContext
    {
        private readonly string _fileNameDatabase = "Methods.db";

        public DbSet<Models.Method> Methods { get; set; }
        public DbSet<Models.Group> Groups { get; set; }
        public DbSet<Models.Types> Types { get; set; }
        public DbSet<Models.RemovingText> RemovingTexts { get; set; }

        public MethodStoreContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=" + _fileNameDatabase);
        }

        internal void InitializingDB()
        {
            if (new Context<Models.RemovingText>().IsEmpty())
            {
                RemovingTexts.Add(new Models.RemovingText() { Text = "Процедура", Type = TypesRemovingText.Start });
                RemovingTexts.Add(new Models.RemovingText() { Text = "Функция", Type = TypesRemovingText.Start });
                SaveChanges();
            }
        }
    }
}