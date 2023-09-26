using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RxApp.Models
{
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
                URL = "api"
            };
            File.WriteAllText("Settings.json", JsonConvert.SerializeObject(settings));

        }
    }
}
