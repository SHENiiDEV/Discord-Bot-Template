using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MINECRAFT_RU_v2.Helpers
{
    public class EmbedHelper
    {
        public static Embed SendEmbed(string title,IUser user,string body,string footertext,string footerimg, bool withTimeStamp = false,int type = 0,  string thumbnail = null)
        {
            var eb = new EmbedBuilder();

            eb.WithAuthor($"{title} • {user.Username}", $"{user.GetAvatarUrl()}").WithImageUrl("https://i.imgur.com/5PAAPj7.png").WithFooter(footertext, footerimg);
            if (type == 0) eb.WithDescription($"```{body}```");
            else if (type == 1) eb.WithDescription($"{body}");
            if (withTimeStamp)
            {
                eb.WithCurrentTimestamp();
            }
            if (thumbnail != null)
            {
                eb.WithThumbnailUrl(thumbnail);
            }
            
            return eb.Build();
        }
    }
}
