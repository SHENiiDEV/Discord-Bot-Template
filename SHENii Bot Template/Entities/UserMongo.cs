using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MINECRAFT_RU_v2.Entities
{
    [BsonIgnoreExtraElements]
    public class UserMongo
    {
            [BsonId]
            public ObjectId _id { get; set; }
            public ulong Id { get; set; }
            public string IdS { get; set; }
            public uint Warn { get; set; } = 0;
            public int Violation { get; set; } = 0;
            public uint XP { get; set; } = 0;
            public int LevelNumber
            {
                get
                {
                    return (int)Math.Sqrt(XP / 81);
                }
            }

            public int Reputation { get; set; } = 0;
            public ulong Diamonds { get; set; } = 0;
            public bool premium { get; set; } = false;
            public ulong message { get; set; } = 0;
            public int invitecount { get; set; } = 0;
            public string minecraftusername { get; set; } = "";
            public string minecraftverification { get; set; } = "";
            public ulong ticketchanid { get; set; } = 0;
            public int lastcalladmin { get; set; }
            public int banticketsytem { get; set; }
            public int bancalltheadmin { get; set; }
            public DateTime LastDaily { get; set; } = DateTime.UtcNow.AddHours(-6);
            public DateTime LastRep { get; set; } = DateTime.UtcNow.AddDays(-2);
            public DateTime LastMessage { get; set; } = DateTime.UtcNow;
            public DateTime LastTicket { get; set; } = DateTime.Now.AddMinutes(-16);
            public long VoiceTime { get; set; } = 0;
            public long VoiceActive { get; set; } = 0;
            public int MuteType { get; set; } = 0;
            public int channelid { get; set; } = 0;
            public long UnMuteTime { get; set; } = 0;
            public List<ReminderEntry> Reminders { get; internal set; } = new List<ReminderEntry>();
            public List<WarnEntry> Warns { get; set; } = new List<WarnEntry>();
            public List<WarnEntry> AllWarns { get; set; } = new List<WarnEntry>();

            public class WarnEntry
            {
                public DateTime dueDate;
                public uint warncount;
                public string reason;
                public ulong moderatorid;

                public WarnEntry(DateTime date, ulong moderator, uint warn, string description)
                {
                    dueDate = date;
                    moderatorid = moderator;
                    warncount = warn;
                    reason = description;
                }
            }
            public class ReminderEntry
            {
                public DateTime DueDate;
                public string Description;

                public ReminderEntry(DateTime dueDate, string description)
                {
                    DueDate = dueDate;
                    Description = description;
                }
            }
    }
}
