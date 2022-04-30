using ManageBandApp.Models;
using System.Collections.Generic;

namespace ManageBandApp.ViewModels
{
    /// <summary>
    /// Модель представления перемещений номенклатуры
    /// </summary>
    public class GoodsMovingViewModel
    {
        /// <summary>
        /// Передвижения номенклатуры
        /// </summary>
        public List<GoodsMoving> Movings { get; set; }

        /// <summary>
        /// Есть ли следующая страница
        /// </summary>
        public bool HavePreviousPage { get; set; }

        /// <summary>
        /// Есть ли предыдущая страница
        /// </summary>
        public bool HaveNextPage { get; set; }

        /// <summary>
        /// Актуальная страница
        /// </summary>
        public int CurrentPage { get; set; }
    }
}
