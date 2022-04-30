using System.ComponentModel.DataAnnotations;

namespace ManageBandApp.Models
{
    /// <summary>
    /// Номенклатура
    /// </summary>
    public class Nomenclature
    {
        /// <summary>
        /// Идентификатор номенклатуры
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Название номенклатуры
        /// </summary>
        [Required]
        public string Name { get; set; }
    }
}
