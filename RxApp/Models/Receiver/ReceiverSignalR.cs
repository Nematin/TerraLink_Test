using Microsoft.AspNetCore.SignalR.Client;

using Newtonsoft.Json;

using RxApp.DTO;

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;



namespace RxApp.Models.Receiver
{
    public class ReceiverSignalR
    {
        private HubConnection _connection;

        private bool _isConnected;
        public bool IsConnected
        {
            get => _isConnected;
            set => _isConnected = value;
        }

        private string _message;
        public string Message
        {
            get => _message;
            set=> _message = value;
        }

        public ReceiverSignalR(string ipAddress, int port, string url)
        {
            _connection = new HubConnectionBuilder()
                .WithUrl($@"http://{ipAddress}:{port}/{url}")
                .WithAutomaticReconnect()
                .Build();


            _connection.Reconnected += Reconnected_Event;
            _connection.Reconnecting += Reconnecting_Event;

            _connection.On<string>("ConnectToServer", ReceiveConnectionToServer);

            _connection.On<string>("ReceiveMessage", Receive);
        }

        private async Task Reconnecting_Event(Exception? arg)
        {
            IsConnected = false;
            OnPropertyChanged(nameof(IsConnected));
        }

        private async Task Reconnected_Event(string? arg)
        {
            IsConnected = _connection.State == HubConnectionState.Connected;
            OnPropertyChanged(nameof(IsConnected));
        }

        private void ReceiveConnectionToServer(string dto)
        {
            var res = JsonConvert.DeserializeObject<ConnectionDTO>(dto);
            IsConnected = res!.IsConnected;
            OnPropertyChanged(nameof(IsConnected));
        }

        public void Receive(string message)
        {
            Message = message;
            OnPropertyChanged(nameof(Message));
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
                await _connection.InvokeAsync("RxDisconnect", "Приёмник отключился");
                await _connection.StopAsync();
            }
            catch (Exception)
            {
                throw new Exception("Ошибка при отключении от сервера");
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
