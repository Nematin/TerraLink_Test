using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.SignalR.Client;

namespace TxApp.Models.Transmitter
{
    public class TransmitterSignalR : BaseTransmitter
    {
        private HubConnection _connection;

        public TransmitterSignalR(string ipAddress, int port, string url)
        {
            _connection = new HubConnectionBuilder()
                .WithUrl($@"http://{ipAddress}:{port}/{url}")
                .WithAutomaticReconnect()
                .Build();

            _connection.Reconnected += Reconnected_Event;
        }

        private async Task Reconnected_Event(string? arg)
        {
            await _connection.InvokeAsync("Send", "Передатчик переподключился");
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
                await _connection.InvokeAsync("Send", "Передатчик отключился");
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
                await _connection.InvokeAsync("Send", message);
            }
            catch (Exception ex)
            {
                throw new Exception($"Отправка на сервер пошла не по плану. {ex.Message}");
            }
        }
    }
}
