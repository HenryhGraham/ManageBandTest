using ManageBandApp.Extentions;
using ManageBandApp.Models;
using ManageBandApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ManageBandApp.ControllerProcessors
{
    public class RemindersControllerProcessor
    {
        public RemindersControllerProcessor(DbAppContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            _context = context;
        }

        private readonly DbAppContext _context;

        /// <summary>
        /// Получить остатки по всем номенклатурам на складе
        /// </summary>
        /// <param name="stockId">Идентификатор склада</param>
        /// <param name="date">Дата по которую нужно считать остаток</param>
        /// <returns></returns>
        public void SetReminders(RemindersReportViewModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (model.StockId <= 0)
                throw new ArgumentException($"Идентификатор склада должен быть больше нуля");


            Stock stock = _context.Stocks.Find(model.StockId);

            if (stock == null)
                throw new Exception($"Не найден склад с идентификатором {model.StockId}");
            model.NomenclatureReminders = GetNomenclaturesReminders(stock, model.RemidersDate).ToList();
        }

        private IEnumerable<NomenclatureReminderViewModel> GetNomenclaturesReminders(Stock stock, DateTime date)
        {
            foreach (Nomenclature nomenclature in _context.Nomenclatures)
                yield return GetNomenclatureReminder(stock, date, nomenclature);
        }

        private NomenclatureReminderViewModel GetNomenclatureReminder(Stock stock, DateTime date, Nomenclature nomenclature)
        {
            long reminder = stock.GetReminder(nomenclature, _context, date);
            return new NomenclatureReminderViewModel
            {
                Reminder = reminder,
                NomenclatureName = nomenclature.Name
            };
        }
    }
}
