using ServerMessageHub.Models;

namespace ServerMessageHub
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddSignalR();

            var app = builder.Build();
            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.MapHub<MessageHub>("/messagehub");

            app.Run();
        }
    }
}