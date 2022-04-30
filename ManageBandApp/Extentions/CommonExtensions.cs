using ManageBandApp.Generator;
using ManageBandApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace ManageBandApp.Extentions
{
    public static class CommonExtensions
    {
        /// <summary>
        /// Инициализаруем стартовый набор номенклатур и складов
        /// </summary>
        /// <param name="modelBuilder"></param>
        public static void InitSeed(this ModelBuilder modelBuilder)
        {
            InitDataGenerator generator = new InitDataGenerator();
            //Генерируем и записываем в бд стартовые данные
            Stock[] stocks = generator
                .GenerateStocks()
                .ToArray();
            Nomenclature[] nomenclatures = generator
                .GenerateNomenclatures()
                .ToArray();

            modelBuilder.Entity<Stock>().HasData(stocks);
            modelBuilder.Entity<Nomenclature>().HasData(nomenclatures);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public static void SeedInitMovings(this DbAppContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (context.GoodsMovings.FirstOrDefault() != null)
                return;

            InitDataGenerator generator = new InitDataGenerator();
            GoodsMoving[] movings = generator
               .GenerateGoodsMovings(context)
               .ToArray();
            context.GoodsMovings.AddRange(movings);
            context.SaveChanges();
        }

        /// <summary>
        /// Получить остаток по номенклатуре за всё время
        /// </summary>
        /// <param name="stock">Склад</param>
        /// <param name="nomenclature">Номенклатура</param>
        /// <param name="context"> Контекст </param>
        /// <returns></returns>
        public static long GetReminder(this Stock stock, Nomenclature nomenclature, DbAppContext context) =>
            GetReminder(stock, nomenclature, context, DateTime.MaxValue);

        /// <summary>
        /// Получить остаток по номенклатуре на момент указанной даты
        /// </summary>
        ///<param name = "stock" > Склад </ param >
        /// <param name="nomenclature">Номенклатура</param>
        /// <param name="context"> Контекст </param>
        /// <param name="date"> Дата по которую нужно посчитать остаток </param>
        /// <returns></returns>
        public static long GetReminder(this Stock stock, Nomenclature nomenclature, DbAppContext context, DateTime date)
        {
            if (stock == null)
                throw new ArgumentNullException(nameof(stock));

            if (nomenclature == null)
                throw new ArgumentNullException(nameof(nomenclature));

            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (date == null)
                throw new ArgumentNullException(nameof(date));

            long income = context
                .GoodsMovings
                .Include(m => m.StockIn)
                .Include(m => m.StockOut)
                .Include(m => m.Nomenclature)
                .Where(m => m.StockIn == stock && m.MovingTime.Date <= date.Date && m.Nomenclature == nomenclature)
                .Sum(m => m.Count);

            long outcome = context
                .GoodsMovings
                .Include(m => m.StockIn)
                .Include(m => m.StockOut)
                .Include(m => m.Nomenclature)
                .Where(m => m.StockOut == stock && m.MovingTime.Date <= date.Date && m.Nomenclature == nomenclature)
                .Sum(m => m.Count);

            return income - outcome;
        }
    }
}
