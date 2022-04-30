namespace ManageBandApp.ViewModels
{
    /// <summary>
    /// Модель передвижения по конкретному товару
    /// </summary>
    public class CreateMovingNomenclatureModel
    {
        /// <summary>
        /// Идентификатор номенклатуры
        /// </summary>
        public int? NomenclatureId { get; set; }

        /// <summary>
        /// Количество
        /// </summary>
        public uint Count { get; set; }

        /// <summary>
        /// Удалено?
        /// </summary>
        public string Deleted { get; set; }
    }
}
