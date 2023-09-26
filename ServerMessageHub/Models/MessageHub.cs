using Microsoft.AspNetCore.SignalR;

using Newtonsoft.Json;

using ServerMessageHub.DTO;

namespace ServerMessageHub.Models
{
    public class MessageHub : Hub
    {
        /// <summary>
        /// Отправка сообщения клиенту
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task Send(string message)
        {
            if (string.IsNullOrWhiteSpace(message) || string.IsNullOrEmpty(message)) return;
            
            var messageDTO = new MessageDTO { Message = message, Date = DateTime.Now };
            await Clients.All.SendAsync("ReceiveMessage", JsonConvert.SerializeObject(messageDTO));
            await Console.Out.WriteLineAsync($"Received: {message}");
        }
        /// <summary>
        /// Метод для обработки отключения передатчика
        /// </summary>
        /// <param name="message">Сообщение</param>
        /// <returns></returns>
        public async Task TxDisconnect(string message)
        {
            await Console.Out.WriteLineAsync($"Received: {message}");
        }
        /// <summary>
        /// Метод для обработки отключения приёмника
        /// </summary>
        /// <param name="message">Сообщение</param>
        /// <returns></returns>
        public async Task RxDisconnect(string message)
        {
            await Console.Out.WriteLineAsync($"Received: {message}");
        }
        /// <summary>
        /// Метод что происходит при реконекте передатчика
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task TxReconnect(string message)
        {
            await Console.Out.WriteLineAsync($"Received: {message}");
        }
        /// <summary>
        /// Метод что происходит при реконекте приемника
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task RxReconnect(string message)
        {
            await Console.Out.WriteLineAsync($"Received: {message}");
        }
        
        /// <summary>
        /// Обработка при подключении клиента
        /// </summary>
        /// <returns></returns>
        public override async Task OnConnectedAsync()
        {
            var connectionDTO = new ConnectionDTO { IsConnected = true };

            await Clients.Caller.SendAsync("ConnectToServer", JsonConvert.SerializeObject(connectionDTO));
            await base.OnConnectedAsync();
        }
        /// <summary>
        /// Обработка при отключении клиента
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await Console.Out.WriteLineAsync($"Клиент {Context.ConnectionId} отключился");
            await base.OnDisconnectedAsync(exception);
        }    
    }
}
