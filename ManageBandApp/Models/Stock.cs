using System.ComponentModel.DataAnnotations;

namespace ManageBandApp.Models
{
    /// <summary>
    /// Склад
    /// </summary>
    public class Stock
    {
        /// <summary>
        /// Идентификатор склада
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Название склада
        /// </summary>
        [Required]
        public string Name { get; set; }
    }
}
