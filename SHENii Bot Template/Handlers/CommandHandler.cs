using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MINECRAFT_RU_v2.Helpers;
using MINECRAFT_RU_v2.Services;
using SHENii_Bot_Template;

namespace MINECRAFT_RU_v2.Handlers
{
   public class CommandHandler
    {
        private readonly CommandService _commands;
        private readonly DiscordSocketClient _discord;
        private readonly IServiceProvider _services;

        public CommandHandler(IServiceProvider services)
        {
            _commands = services.GetRequiredService<CommandService>();
            _discord = services.GetRequiredService<DiscordSocketClient>();
            _services = services;
        }

        public async Task InitializeAsync()
        {
            // Register modules that are public and inherit ModuleBase<T>.
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
            Global.Client = _discord;
        }

        public async Task HandleCommandAsync(SocketMessage s)
        {
            if (!(s is SocketUserMessage msg)) return;
            if (msg.Channel is SocketDMChannel) return;

            var context = new SocketCommandContext(_discord, msg);
            if (context.User.IsBot) return;

            var argPos = 0;
            if ((msg.HasCharPrefix('!', ref argPos) || msg.HasMentionPrefix((_discord).CurrentUser, ref argPos)))
            {
                var cmdSearchResult = _commands.Search(context, argPos);
                if (!cmdSearchResult.IsSuccess) return;

                var executionTask = _commands.ExecuteAsync(context, argPos, _services);
                #pragma warning disable CS4014
                executionTask.ContinueWith(task =>
                  {
                      if (task.Result.IsSuccess || task.Result.Error == CommandError.UnknownCommand) return;
                      const string errTemplate = "{0}, Ошибка: {1}.";
                      Console.WriteLine(task.Result.Error);
                      var embed = EmbedHelper.SendEmbed("Ошибка ", context.User,
                          ErrorHandler.CheckError(task.Result.Error.ToString()), null, null, true);
                      var errMessage = string.Format(errTemplate, context.User.Mention, task.Result.ErrorReason);
                      
                      context.Channel.SendMessageAsync(embed:embed);
                  });
                #pragma warning restore CS4014

            }
        }

        private static bool CheckPrefix(ref int argPos, SocketCommandContext context)
        {
            if (context.Guild is null) return false;
            var prefixes = GuildManager.GetGuildAsync(Constants.guildid).Result.Prefixes;
            var tmpArgPos = 0;

            var success = prefixes.Any(pre =>
            {
                if (!context.Message.Content.StartsWith(pre)) return false;
                tmpArgPos = pre.Length;

                return true;
            });
            argPos = tmpArgPos;
            return success;
        }
    }
}
