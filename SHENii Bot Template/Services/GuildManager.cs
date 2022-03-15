using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using MINECRAFT_RU_v2.Entities;
using MongoDB.Driver;

namespace MINECRAFT_RU_v2.Services
{
    public class GuildManager
    {
        private readonly MongoManager _mongoManager;

        public GuildManager(IServiceProvider services)
        {
            _mongoManager = services.GetRequiredService<MongoManager>();
        }

        public static IMongoCollection<GuildMongo> Guild => MongoManager.MinecraftMongo.GetCollection<GuildMongo>("guilds");

        public static async Task<GuildMongo> GetGuildAsync(ulong id)
        {
            var guild = Guild.Find(a => a.Id == id).FirstOrDefault();
            if (guild != null) return guild;
            if (guild == null) guild = await SignUp(id);

            return guild;
        }

        public static async Task<GuildMongo> SignUp(ulong id)
        {
            var guild = new GuildMongo()
            {
                Id = id,
            };

            await InsertAsync(guild);

            return guild;
        }

        public static async Task<bool> UpdateAsync(GuildMongo Buser)
        {
            Guild.ReplaceOne(a => a.Id == Buser.Id, Buser);
            return await Task.FromResult(true);
        }

        public static async Task<bool> DeleteAsync(ulong id)
        {
            Guild.DeleteOne(a => a.Id == id);
            return await Task.FromResult(true);
        }

        static async Task<bool> InsertAsync(GuildMongo Buser)
        {
            Guild.InsertOne(Buser);
            return await Task.FromResult(true);
        }
    }
}
