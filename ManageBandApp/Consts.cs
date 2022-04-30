namespace ManageBandApp
{
    /// <summary>
    /// Константы для работы
    /// </summary>
    public class Consts
    {
        /// <summary>
        /// Строка подключения к бд
        /// </summary>
        public const string ConnectionString = "Server=(localdb)\\mssqllocaldb;Database=ManageBandDbTest;Trusted_Connection=True;";

        /// <summary>
        /// Кол-во строк в таблице для отображения перемещений (оставил дефолтным)
        /// </summary>
        public const int DefaultPageSize = 8;
    }
}
