using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Fergun.Interactive;
using KillersLibrary.Services;
using Microsoft.Extensions.DependencyInjection;
using MINECRAFT_RU_v2.Handlers;
using MINECRAFT_RU_v2.Services;
using SHENii_Bot_Template.Handlers;

namespace SHENii_Bot_Template
{
    class Program
    {
        private static DiscordSocketClient _client;
        private static IServiceProvider _serviceProvider;
        public static InteractiveService Interactive { get; set; }

        static void Main(string[] args)
            => new Program().MainAsync().GetAwaiter().GetResult();

        private static DiscordSocketConfig GetDiscordSocketConfig()
        {
            return new DiscordSocketConfig()
            {
                AlwaysDownloadUsers = true,
                GatewayIntents = GatewayIntents.All,
                MessageCacheSize = 100
            };
        }

        public async Task MainAsync()
        {
            var discordSocketConfig = GetDiscordSocketConfig();
            if (discordSocketConfig == null) return;

            _client = new DiscordSocketClient(discordSocketConfig);
            _serviceProvider = ConfigureServices();

            _serviceProvider.GetRequiredService<CommandService>().Log += LogAsync;
            _serviceProvider.GetRequiredService<DiscordEventHandler>().InitDiscordEvents();
            await _serviceProvider.GetRequiredService<MongoManager>().InitializeAsync("replace_with_bot_mongodb_connection");
            await _client.LoginAsync(TokenType.Bot, "replace_with_bot_token");
            await _client.StartAsync();
            await _serviceProvider.GetRequiredService<CommandHandler>().InitializeAsync();
            Interactive = _serviceProvider.GetRequiredService<InteractiveService>();
            _client.Log += LogAsync;

            await Task.Delay(Timeout.Infinite);

        }

        private Task LogAsync(LogMessage log)
        {
            Console.WriteLine(log.ToString());

            return Task.CompletedTask;
        }

        private ServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                .AddSingleton(_client)
                .AddSingleton<CommandService>()
                .AddSingleton<EmbedPagesService>()
                .AddSingleton<InteractiveService>()
                .AddSingleton<DiscordEventHandler>()
                .AddSingleton<CommandHandler>()
                .AddSingleton<MongoManager>()
                .AddSingleton<HttpClient>()
                .BuildServiceProvider();
        }
    }
}
