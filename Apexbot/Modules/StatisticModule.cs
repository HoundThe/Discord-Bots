using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApexBot.Services;
using ApexBot.Services.Model;
using Discord;
using Discord.Commands;

namespace ApexBot.Modules
{
    public class StatisticModule : ModuleBase<SocketCommandContext>
    {
        public StatisticService? StatService { get; set; } //public for dependency injection
        
        // This is hardcoded for now, as it is easiest option and barely ever changes
        private static readonly List<ulong> RoleIds = new List<ulong>
        {
            552082441238609920,
            549038408501166090,
            549038311348371471,
            549038130372804614,
            549037609293185040,
            549037589387149313,
            549037564309274628,
            549037062163005453,
            549036077026115585,
            549035902677024828
        };

        [Command("rank", RunMode = RunMode.Async)]
        [Summary("Sets user role based on in-game statistics")]
        public async Task SetRankAsync()
        {
            using var state = Context.Channel.EnterTypingState();
            
            if (!(Context.User is IGuildUser guildUser))
                return;
                
            var nickname = guildUser.Nickname ?? guildUser.Username;

            StatisticsModel statistics;

            try
            {
                statistics = await StatService.RequestStats(nickname);
            }
            catch (Exception e)
            {
                await ReplyAsync(e.Message);
                return;
            }

            var tmp = guildUser.RoleIds.Intersect(RoleIds);
            var oldRoles = Context.Guild.Roles.Where(t => tmp.Contains(t.Id));

            await guildUser.RemoveRolesAsync(oldRoles); // remove any old role

            await guildUser.AddRoleAsync(Context.Guild.GetRole(GetLevelRole(statistics.Data.Metadata.Level))); // add recent role
                
            var embedBuilder = new EmbedBuilder()
                .WithColor(Color.Orange)
                .WithAuthor("", "http://www.csie.ase.ro/Media/Default/images/Statistics_logo.png")
                .WithTitle($"Statistics for {nickname}")
                .WithDescription
                    ($"**Level **: {statistics.Data.Metadata.Level}")
                .WithThumbnailUrl(statistics.Data.Metadata.AvatarUrl)
                .WithCurrentTimestamp()
                .WithFooter("Source: https://apex.tracker.gg/", "https://png.icons8.com/ios/1600/bot.png");

            await ReplyAsync(embed: embedBuilder.Build());
        }
        

        private static ulong GetLevelRole(int level)
        {
            if (level >= 100) return RoleIds[0];
            if (level >= 90) return RoleIds[1];
            if (level >= 80) return RoleIds[2];
            if (level >= 70) return RoleIds[3];
            if (level >= 60) return RoleIds[4];
            if (level >= 50) return RoleIds[5];
            if (level >= 40) return RoleIds[6];
            if (level >= 30) return RoleIds[7];
            if (level >= 20) return RoleIds[8];
            return RoleIds[9];
        }
    }
}