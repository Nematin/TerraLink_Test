using Avalonia;

using Newtonsoft.Json;

using RxApp.DTO;
using RxApp.Models;
using RxApp.Models.Receiver;

using System.Collections.ObjectModel;
using System.IO;
using System.Reactive.Linq;

namespace RxApp.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        AppSettings _settings;
        ReceiverSignalR? _receiver;

        private string _title;
        /// <summary>
        /// Название приложения
        /// </summary>
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        /// <summary>
        /// Список сообщений
        /// </summary>
        public ObservableCollection<string> Messages { get; set; } = new();

        public MainWindowViewModel()
        {
            Title = "Приёмник. Соединение не установлено";

            try
            {
                _settings = AppSettings.GetSettings();
            }
            catch (FileNotFoundException)
            {
                AppSettings.CreateDefaultSettings();
                _settings = AppSettings.GetSettings();
            }

            _receiver = new(_settings.IpAddress, _settings.Port, _settings.URL);
            _receiver?.Start();
            _receiver!.PropertyChanged += ReceiverPropertyChangedEvent;

        }

        /// <summary>
        /// Вызывается при закрытии программы
        /// </summary>
        public void CloseApp()
        {
            _receiver?.Disconnect();
        }

        private void ReceiverPropertyChangedEvent(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_receiver.IsConnected))
            {
                if (_receiver!.IsConnected)
                {
                    Title = "Приёмник. Соединение установлено";
                }
                else
                {
                    Title = "Приёмник. Соединения нет";
                }
            }
            else if (e.PropertyName == nameof(_receiver.Message))
            {
                var mesDTO = JsonConvert.DeserializeObject<MessageDTO>(_receiver!.Message);
                Messages.Add($"{mesDTO?.Date} | {mesDTO?.Message}");
            }
        }
    }
}