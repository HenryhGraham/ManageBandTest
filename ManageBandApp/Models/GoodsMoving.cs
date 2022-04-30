using System;
using System.ComponentModel.DataAnnotations;

namespace ManageBandApp.Models
{
    /// <summary>
    /// Перемещаемый товар (ТМЦ)
    /// </summary>
    public class GoodsMoving
    {
        /// <summary>
        /// Идентификатор записи о перемещении
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Идентификатор перемещения
        /// </summary>
        [Required]
        public Guid MovingId { get; set; }

        /// <summary>
        /// Склад в который идет перемещение
        /// </summary>
        public Stock StockIn { get; set; }

        /// <summary>
        /// Склад из которого идет перемещение
        /// </summary>
        public Stock StockOut { get; set; }

        /// <summary>
        /// Номенклатура
        /// </summary>
        [Required]
        public Nomenclature Nomenclature { get; set; }

        /// <summary>
        /// Количество номенклатуры
        /// </summary>
        [Required]
        public uint Count { get; set; }

        /// <summary>
        /// Время перемещения
        /// </summary>
        [Required]
        public DateTime MovingTime { get; set; }
    }
}
