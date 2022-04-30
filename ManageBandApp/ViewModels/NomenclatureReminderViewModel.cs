namespace ManageBandApp.ViewModels
{
    /// <summary>
    /// Модель для отображения остатков по конкретным номенклатурам
    /// </summary>
    public class NomenclatureReminderViewModel
    {
        /// <summary>
        /// Название номенклатуры
        /// </summary>
        public string NomenclatureName { get; set; }

        /// <summary>
        /// Остаток
        /// </summary>
        public long Reminder { get; set; }
    }
}
