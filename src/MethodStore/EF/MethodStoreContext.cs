using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace MethodStore.EF
{
    public class MethodStoreContext : DbContext
    {
        private readonly string _fileNameDatabase = "Methods.db";

        #region Constructors
        public MethodStoreContext()
        {
            Database.EnsureCreated();
        } 
        #endregion

        #region Properties db table
        public DbSet<Models.Method> Methods { get; set; }
        public DbSet<Models.Group> Groups { get; set; }
        public DbSet<Models.Types> Types { get; set; }
        public DbSet<Models.RemovingText> RemovingTexts { get; set; }
        #endregion

        public async Task<T> FindByNameAsync<T>(string name) where T : class
        {
            Type typeofT = typeof(T);
            if (typeofT == typeof(Models.Group))
                return await Groups.SingleOrDefaultAsync(f => f.Name == name) as T;
            else if (typeofT == typeof(Models.Types))
                return await Types.SingleOrDefaultAsync(f => f.Name == name) as T;
            else
                return null;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=" + _fileNameDatabase);
        }

        internal void InitializingDB()
        {
            if (new Context<Models.RemovingText>().IsEmpty())
            {
                InitializindDBRemovingTexts();

                SaveChanges();
            }
        }

        private void InitializindDBRemovingTexts()
        {
            RemovingTexts.Add(new Models.RemovingText() { Text = "Процедура", Type = TypesRemovingText.Start });
            RemovingTexts.Add(new Models.RemovingText() { Text = "Функция", Type = TypesRemovingText.Start });
        }
    }
}