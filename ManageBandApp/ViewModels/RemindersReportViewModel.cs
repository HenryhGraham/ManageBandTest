using ManageBandApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ManageBandApp.ViewModels
{
    /// <summary>
    /// Модель представления для просмотра остатков
    /// </summary>
    public class RemindersReportViewModel
    {
        public RemindersReportViewModel()
        {
            NomenclatureReminders = new List<NomenclatureReminderViewModel>();
        }

        private bool __init_Stocks;
        private Stock[] _Stocks;
        /// <summary>
        /// Список складов
        /// </summary>
        public Stock[] Stocks
        {
            get
            {
                if (!__init_Stocks)
                {
                    using (DbAppContext context = new DbAppContext())
                        _Stocks = context.Stocks.ToArray();

                    __init_Stocks = true;
                }
                return _Stocks;
            }
        }

        /// <summary>
        /// Остатки по номенклатурам
        /// </summary>
        public List<NomenclatureReminderViewModel> NomenclatureReminders { get; set; }

        /// <summary>
        /// Идентификатор склада
        /// </summary>
        public int StockId { get; set; }

        /// <summary>
        /// Дата на которую нужно считать остатки
        /// </summary>
        public DateTime RemidersDate { get; set; }
    }
}
