using Microsoft.AspNetCore.SignalR;

using ServerMessageHub.DTO;

namespace ServerMessageHub.Models
{
    public class MessageHub : Hub
    {
        public async Task Send(string message)
        {
            var messageDTO = new MessageDTO { Message = message, Date = DateTime.Now };

            await this.Clients.All.SendAsync("Receive", messageDTO);

            await Console.Out.WriteLineAsync($"Received: {message}");
        }
    }
}
