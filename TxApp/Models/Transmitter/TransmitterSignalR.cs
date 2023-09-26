using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

using Microsoft.AspNetCore.SignalR.Client;

using Newtonsoft.Json;

using TxApp.DTO;

namespace TxApp.Models.Transmitter
{
    public class TransmitterSignalR : BaseTransmitter, INotifyPropertyChanged
    {
        private HubConnection _connection;

        private bool _isConnected;
        public bool IsConnected
        {
            get => _isConnected;
            set => _isConnected = value;
        }

        public TransmitterSignalR(string ipAddress, int port, string url)
        {
            _connection = new HubConnectionBuilder()
                .WithUrl($@"http://{ipAddress}:{port}/{url}")
                .WithAutomaticReconnect()
                .Build();

            _connection.Reconnected += Reconnected_Event;
            _connection.On<string>("ConnectToServer", ReceiveData);
            _connection.Reconnecting += Reconnecting_Event;
        }

        private async Task Reconnecting_Event(Exception? exception)
        {
            IsConnected = false;
            OnPropertyChanged(nameof(IsConnected));
        }

        private void ReceiveData(string dto)
        {
            var res = JsonConvert.DeserializeObject<ConnectionDTO>(dto);
            IsConnected = res!.IsConnected;

            OnPropertyChanged(nameof(IsConnected));
        }

        private async Task Reconnected_Event(string? arg)
        {
            await _connection.InvokeAsync("TxReconnect", "Передатчик переподключился");
        }

        public async Task Start()
        {
            try
            {
                await _connection.StartAsync();

            }
            catch (Exception)
            {
                throw new Exception("Подключение к серверу пошло не по плану");
            }
        }

        public async Task Disconnect()
        {
            try
            {
                await _connection.InvokeAsync("TxDisconnect", "Передатчик отключился");
                await _connection.StopAsync();
            }
            catch (Exception)
            {
                throw new Exception("Ошибка при отключении от сервера");
            }
        }

        public override async Task Send<T>(T message)
        {
            try
            {
                if (_connection.State == HubConnectionState.Connected)
                    await _connection.InvokeAsync("Send", message);
            }
            catch (Exception ex)
            {
                throw new Exception($"Отправка на сервер пошла не по плану. {ex.Message}");
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
