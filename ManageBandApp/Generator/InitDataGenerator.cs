using ManageBandApp.Models;
using System;
using System.Collections.Generic;

namespace ManageBandApp.Generator
{
    /// <summary>
    /// Генератор данных для первоначального запуска
    /// </summary>
    public class InitDataGenerator
    {
        /// <summary>
        /// Генерирует первоначальные данные по складам
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Stock> GenerateStocks()
        {
            for (int i = 1; i <= 5; i++)
                yield return new Stock
                {
                    Id = i,
                    Name = $"Склад{i}"
                };
        }

        /// <summary>
        /// Генерирует первоначальные данные по номенклатуре
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Nomenclature> GenerateNomenclatures()
        {
            for (int i = 1; i <= 10; i++)
                yield return new Nomenclature
                {
                    Id = i,
                    Name = $"Номенклатура{i}"
                };
        }

        /// <summary>
        /// Гененрирует передвижения товаров (только поступления извне и расход вовне)
        /// </summary>
        /// <param name="context">Контекст БД</param>context
        /// <returns></returns>
        public IEnumerable<GoodsMoving> GenerateGoodsMovings(DbAppContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            DateTime movingsDateTime = DateTime.Now.AddDays(-5);
            //По три поступления и три расхода на каждый склад
            for (int i = 1; i <= 3; i++)
            {
                foreach (Stock stock in context.Stocks)
                {
                    Guid movingGuid = Guid.NewGuid();
                    foreach (Nomenclature nomenctlature in context.Nomenclatures)
                    {
                        yield return new GoodsMoving
                        {
                            MovingId = movingGuid,
                            MovingTime = movingsDateTime,
                            Nomenclature = nomenctlature,
                            StockIn = stock,
                            Count = 10
                        };

                        yield return new GoodsMoving
                        {
                            MovingId = movingGuid,
                            MovingTime = movingsDateTime,
                            Nomenclature = nomenctlature,
                            StockOut = stock,
                            Count = 5
                        };

                    }

                }
            }
        }
    }
}
