#nullable disable
using Newtonsoft.Json;

using System.IO;

namespace TxApp.Models
{
    /// <summary>
    /// Настройки приложения
    /// </summary>
    public class AppSettings
    {
        /// <summary>
        /// IP адрес сервера
        /// </summary>
        public string IpAddress { get; set; }
        /// <summary>
        /// Порт сервера
        /// </summary>
        public int Port { get; set; }
        /// <summary>
        /// Путь к API
        /// </summary>
        public string URL { get; set; }

        /// <summary>
        /// Время до отправки сообщения в сек
        /// </summary>
        public double DelayTime { get; set; }

        /// <summary>
        /// Прочитать файл настроек
        /// </summary>
        /// <returns></returns>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="System.Exception"></exception>
        public static AppSettings GetSettings()
        {
            try
            {
                AppSettings settings = JsonConvert.DeserializeObject<AppSettings>(File.ReadAllText("Settings.json"));

                return settings;
            }
            catch (FileNotFoundException)
            {
                throw new FileNotFoundException("Не удалось найти файл.", "Settings.json");
            }
            catch (System.Exception)
            {
                throw new System.Exception("Неизвестная ошибка");
            }
        }

        /// <summary>
        /// Создать файл настроек по умолчанию
        /// </summary>
        public static void CreateDefaultSettings()
        {
            AppSettings settings = new()
            {
                IpAddress = "localhost",
                Port = 8000,
                URL = "api",
                DelayTime = 1.0
            };
            File.WriteAllText("Settings.json", JsonConvert.SerializeObject(settings));

        }
    }
}
