using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.WebSocket;
using KillersLibrary.Services;
using MINECRAFT_RU_v2.Handlers;

namespace SHENii_Bot_Template.Handlers
{
    public class DiscordEventHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandHandler _commandHandler;

        public DiscordEventHandler(DiscordSocketClient client, CommandHandler commandHandler)
        {
            _client = client;
            _commandHandler = commandHandler;
        }

        public void InitDiscordEvents()
        {
            _client.Ready += OnReady;
            _client.MessageReceived += MessageReceived;
        }

        private async Task OnReady()
        {
            Console.WriteLine(DateTime.Now.ToString("HH:mm:ss") + " MuteSystem  Ready");
            Console.WriteLine(DateTime.Now.ToString("HH:mm:ss") + " MongoDB     Ready");
            Console.WriteLine(DateTime.Now.ToString("HH:mm:ss") + " Logs        Ready");
        }

        private async Task MessageReceived(SocketMessage message)
        {
            await _commandHandler.HandleCommandAsync(message);
        }
    }
}
