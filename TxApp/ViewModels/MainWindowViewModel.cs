using Avalonia.Threading;

using System;
using System.Collections.ObjectModel;
using System.IO;


using TxApp.Models;
using TxApp.Models.Transmitter;

namespace TxApp.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        AppSettings _settings;
        DispatcherTimer _typingTimer;
        TransmitterSignalR? _transmitter;

        private string? _message;
        public string? Message
        {
            get => _message;
            set
            {
                if (string.IsNullOrEmpty(value)) { return; }

                _typingTimer.Stop();
                SetProperty(ref _message, value);
                _typingTimer.Start();
            }
        }

        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private bool _isInputTBEnable;
        public bool IsInputTBEnable
        {
            get => _isInputTBEnable;
            set => SetProperty(ref _isInputTBEnable, value);
        }

        public MainWindowViewModel()
        {
            Title = "Передатчик. Соединение не установлено";

            try
            {
                _settings = AppSettings.GetSettings();
            }
            catch (FileNotFoundException)
            {
                AppSettings.CreateDefaultSettings();
                _settings = AppSettings.GetSettings();
            }

            var time = TimeSpan.FromSeconds(_settings.DelayTime);
            _typingTimer = new DispatcherTimer() { Interval = time };
            _typingTimer.Tick += TypingTimerEvent;

            _transmitter = new(_settings.IpAddress, _settings.Port, _settings.URL);
            _transmitter?.Start();
            _transmitter!.PropertyChanged += TransmitterPropertyChangedEvent;
        }

        private void TransmitterPropertyChangedEvent(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_transmitter.IsConnected))
            {
                if (_transmitter!.IsConnected)
                {
                    Title = "Передатчик. Соединение установлено";
                    IsInputTBEnable = true;
                }
            }
        }

        private async void TypingTimerEvent(object? sender, EventArgs e)
        {
            await _transmitter!.Send(Message);
            _typingTimer.Stop();
        }
    }
}