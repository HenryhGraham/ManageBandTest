using ManageBandApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ManageBandApp.ViewModels
{
    /// <summary>
    /// Модель представления для создания перемещения номенклатуры
    /// </summary>
    public class CreateMovingViewModel
    {
        public CreateMovingViewModel()
        {
            NomenclaturesToCreate = new List<CreateMovingNomenclatureModel>();
        }

        private bool __init_Nomenclatures;
        private Nomenclature[] _Nomenclatures;
        /// <summary>
        /// Список номенклатуры
        /// </summary>
        public Nomenclature[] Nomenclatures
        {
            get
            {
                if (!__init_Nomenclatures)
                {
                    using (DbAppContext context = new DbAppContext())
                        _Nomenclatures = context.Nomenclatures.ToArray();

                    __init_Nomenclatures = true;
                }
                return _Nomenclatures;
            }
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
        /// Идентификатор склада в который происходит перемещение
        /// </summary>
        public int? StockInId { get; set; }

        /// <summary>
        /// Идентификатор склада из которого происходит перемещение
        /// </summary>
        public int? StockOutId { get; set; }

        /// <summary>
        /// Дата перемещения
        /// </summary>
        public DateTime MovingDate { get; set; }

        /// <summary>
        /// Список ошибок
        /// </summary>
        public List<string> Errors = new List<string>();

        /// <summary>
        /// Показать ли сообщение об успешном создании перемещения
        /// </summary>
        public bool ShowSuccessMsg = false;

        /// <summary>
        /// Перемещения номенклатур
        /// </summary>
        public List<CreateMovingNomenclatureModel> NomenclaturesToCreate { get; set; }

    }


}
