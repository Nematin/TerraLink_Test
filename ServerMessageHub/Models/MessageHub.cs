using Microsoft.AspNetCore.SignalR;

using Newtonsoft.Json;

using ServerMessageHub.DTO;

namespace ServerMessageHub.Models
{
    public class MessageHub : Hub
    {
        public async Task Send(string message)
        {
            var messageDTO = new MessageDTO { Message = message, Date = DateTime.Now };

            await this.Clients.All.SendAsync("Receive", JsonConvert.SerializeObject(messageDTO));

            await Console.Out.WriteLineAsync($"Received: {message}");
        }
        public override async Task OnConnectedAsync()
        {
            var connectionDTO = new ConnectionDTO { IsConnected = true };

            await this.Clients.Caller.SendAsync("ConnectToServer", JsonConvert.SerializeObject(connectionDTO));
            await base.OnConnectedAsync();
        }
    }
}
