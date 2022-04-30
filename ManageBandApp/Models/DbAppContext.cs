using ManageBandApp.Extentions;
using Microsoft.EntityFrameworkCore;

namespace ManageBandApp.Models
{
    /// <summary>
    /// Контекст данных
    /// </summary>
    public class DbAppContext : DbContext
    {

        /// <summary>
        /// Справочник номенклатуры
        /// </summary>
        public DbSet<Nomenclature> Nomenclatures { get; set; }

        /// <summary>
        /// Справочник складов
        /// </summary>
        public DbSet<Stock> Stocks { get; set; }

        /// <summary>
        /// Перемещения номенклатуры
        /// </summary>
        public DbSet<GoodsMoving> GoodsMovings { get; set; }

        private bool IsModelCreated = false;

        public DbAppContext()
        {
            Database.EnsureCreated();

            if (IsModelCreated)
                this.SeedInitMovings();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Consts.ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.InitSeed();
            IsModelCreated = true;
        }
    }
}
