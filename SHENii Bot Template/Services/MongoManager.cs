using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.WebSocket;
using MINECRAFT_RU_v2.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace MINECRAFT_RU_v2.Services
{
    public class MongoManager
    {

        public static MongoClient _client;

        private readonly IServiceProvider _service;

        public MongoManager(IServiceProvider serviceProvider)
        {
            _service = serviceProvider;
        }

        public static IMongoDatabase MinecraftMongo => _client.GetDatabase("MinecraftRU");

        public async Task InitializeAsync(string mongoConnection)
        {
            Console.WriteLine(DateTime.Now.ToString("HH:mm:ss") + " MongoDB     Connecting");

            _client = new MongoClient(mongoConnection);

            await _client.StartSessionAsync();

            Console.WriteLine(DateTime.Now.ToString("HH:mm:ss") + " MongoDB     Connected");
        }

        public static IMongoCollection<UserMongo> Users => MinecraftMongo.GetCollection<UserMongo>("users");
        
        public static async Task<UserMongo> GetUserAsync(ulong id)
        {
            var user = Users.Find(a => a.Id == id).FirstOrDefault();
            if (user != null) return user;

            if (user == null) user = await SignUp(id);

            return user;
        }

        public static async Task<bool> UpdateManyAsync(List<UserMongo> users)
        {
            var updates = new List<WriteModel<UserMongo>>();
            foreach (var user in users)
            {
                var filter = Builders<UserMongo>.Filter.Where(u => u.Id == user.Id);
                updates.Add(new ReplaceOneModel<UserMongo>(filter, user));
            }
            await Users.BulkWriteAsync(updates, new BulkWriteOptions() { IsOrdered = false });
            return await Task.FromResult(true);
        }

        public static async Task<UserMongo> SignUpNewModel(ulong id, uint xpcount, ulong diamond,ulong msgs,long voicetime,long voiceactive )
        { 
            var user = new UserMongo()
            {
                Id = id,
                IdS = $"{id}",
                XP = xpcount,
                Diamonds = diamond,
                message = msgs,
                VoiceTime = voicetime,
                VoiceActive = voiceactive
            };

            await InsertAsync(user);

            return user;
        }

        public static async Task<UserMongo> SignUp(ulong id)
        {
            var user = new UserMongo()
            {
                Id = id,
                IdS = $"{id}",
            };

            await InsertAsync(user);

            return user;
        }
        public static async Task<bool> UpdateAsync(UserMongo Buser)
        {
            Users.ReplaceOne(a => a.Id == Buser.Id, Buser);
            return await Task.FromResult(true);
        }

        public static async Task<bool> DeleteAsync(ulong id)
        {
            Users.DeleteOne(a => a.Id == id);
            return await Task.FromResult(true);
        }

        static async Task<bool> InsertAsync(UserMongo Buser)
        {
            Users.InsertOne(Buser);
            return await Task.FromResult(true);
        }

    }
}

