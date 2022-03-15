using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MINECRAFT_RU_v2.Entities
{
    [BsonIgnoreExtraElements]
    public class GuildMongo
    {
        [BsonId]
        public ObjectId _id { get; set; }
        public ulong Id { get; set; }
        public ulong ticketnumber { get; set; } = 0;
        public ulong AnnouncementChannelId { get; private set; }
        public IReadOnlyList<string> Prefixes { get; private set; } = new List<string>();
        public IReadOnlyList<string> WelcomeMessages { get; private set; } = new List<string> { };
        public IReadOnlyList<string> LeaveMessages { get; private set; } = new List<string>();
        public Dictionary<string, string> Tags { get; private set; } = new Dictionary<string, string>();
        public int ServerActivityLog { get; set; }
        public ulong LogChannelId { get; set; }
        public ulong CommandChannelId { get; set; }
        public ulong CaptchaChannelId { get; set; }

        public ulong bumpreward { get; set; }
        public ulong likereward { get; set; }
        public ulong supreward { get; set; }
        public ulong dbumpreward { get; set; }
        public ulong dailybonus { get; set; }
        public ulong dailybonuspremium { get; set; }
        public double transfernalog { get; set; }

        public string RoleOnJoin { get; set; }
    }
}
