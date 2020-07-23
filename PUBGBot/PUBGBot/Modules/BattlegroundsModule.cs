using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using PUBGBot.Models;
using PUBGBot.Services;

namespace PUBGBot.Modules
{
    public class BattlegroundsModule : ModuleBase<SocketCommandContext>
    {
        private readonly BattlegroundsService _battlegroundsService;
        
        public BattlegroundsModule(BattlegroundsService battlegroundsService)
        {
            _battlegroundsService = battlegroundsService;
        }
        
        // This is hardcoded for now, as it is easiest option and barely ever changes
        private static readonly List<ulong> RoleIds = new List<ulong>
        {
            647255266630303755,    // expert 5000+
            647255255159013411,    // specialist 4000+
            647255219582926898,    // skilled 3000+
            647255264013189120,    // 2000+ experienced
            398171202016772106,    // high kd 3.5+
            398171206127321088     // 30%+ wr
        };

        [Command("rank", RunMode = RunMode.Async)]
        [Alias("rating")]
        [Summary("Sets user role based on in-game statistics")]
        public async Task SetRankAsync(string mode, string teamSize)
        {
            if (!(Context.User is IGuildUser guildUser))
                return;

            using var state = Context.Channel.EnterTypingState();

            var nickname = guildUser.Nickname ?? guildUser.Username;

            PlayerModel model;

            try
            {
                model = await _battlegroundsService.GetPlayerStats(nickname);
            }
            catch (HttpRequestException ex)
            {
                await ReplyAsync(ex.Message);
                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }

                  
            switch (mode.ToLower())
            {
                case "fpp":
                    break;
                case "tpp": 
                    break;
                default:
                    await ReplyAsync("Incorrect mode, use tpp or fpp.");
                    return;
            }

            int team = 4;
            switch (teamSize.ToLower())
            {
                case "squad":
                    team = 4;
                    break;
                case "duo":
                    team = 2;
                    break;
                case "solo":
                    await ReplyAsync("Solo doesn't count.");
                    return;
                default:
                    await ReplyAsync("Incorrect team size, use duo or squad.");
                    return;
            }

            Duo playerData = null;
            
            switch (mode)
            {
                case "fpp":
                {
                    if (team == 4)
                    {
                        playerData = model.Data.Attributes.GameModeStats.SquadFpp;
                    } 
                    else if (team == 2)
                    {
                        playerData = model.Data.Attributes.GameModeStats.DuoFpp;
                    }
                    else
                    {
                        return;
                    }

                    break;
                }
                case "tpp":
                {
                    if (team == 4)
                    {
                        playerData = model.Data.Attributes.GameModeStats.SoloFpp;
                    } 
                    else if (team == 2)
                    {
                        playerData = model.Data.Attributes.GameModeStats.Duo;
                    }
                    else
                    {
                        return;
                    }
                    break;
                }
                default:
                    return;
            }

            if (playerData.RoundsPlayed == 0)
            {
                await ReplyAsync("***No games played!***");
                return;
            }
            
            var roleIds = guildUser.RoleIds.Intersect(RoleIds);
            var oldRoles = Context.Guild.Roles.Where(t => roleIds.Contains(t.Id));
            
            await guildUser.RemoveRolesAsync(oldRoles); // remove any old role

            long gamesPlayed = playerData.Losses + playerData.Wins;
            float winRate = (float)playerData.Wins / gamesPlayed * 100;
            float kd = (float) playerData.Kills / playerData.Losses;
            var newRoleIds = GetRoles(playerData.RankPoints, winRate, kd);
            var newRoles = Context.Guild.Roles.Where(t => newRoleIds.Contains(t.Id));

            string description = "You've got these roles:\n";

            int i = 0;
            foreach (var role in newRoles)
            {
                i++;
                description += $"{role.Mention}\n";
            }

            if (i == 0)
                description = String.Empty;

            await guildUser.AddRolesAsync(newRoles);
            
            var embedBuilder = new EmbedBuilder()
                .WithColor(Color.Orange)
                .WithAuthor(mode.ToUpper() + " " + teamSize.ToUpper(), "http://www.csie.ase.ro/Media/Default/images/Statistics_logo.png")
                .WithTitle($"Statistics for {nickname}")
                .WithDescription($"{guildUser.Mention}\n{description}")
                .AddField("Rating:", Math.Round(playerData.RankPoints, 0), true)
                .AddField("Games played:", playerData.Wins + playerData.Losses, true)
                .AddField("Victories:", playerData.Wins, true)
                .AddField("WR:", $"{Math.Round(winRate, 2)}%", true)
                .AddField("ADR:", Math.Round(playerData.DamageDealt / gamesPlayed, 1), true)
                .AddField("KDR:", Math.Round(kd, 2), true)
                .WithThumbnailUrl(Context.Guild.IconUrl)
                .WithCurrentTimestamp()
                .WithFooter("Source: https://api.playbattlegrounds.com", "https://png.icons8.com/ios/1600/bot.png");

            await ReplyAsync(embed: embedBuilder.Build());
        }

        private static IEnumerable<ulong> GetRoles(double rating, float wr, float kd)
        {
            var roles = new List<ulong>();
            
            if (rating >= 5000) roles.Add(RoleIds[0]);
            else if (rating >= 4000) roles.Add(RoleIds[1]);
            else if (rating >= 3000) roles.Add(RoleIds[2]);
            else if (rating >= 2000) roles.Add(RoleIds[3]);
            
            if(kd >= 3.5) roles.Add(RoleIds[4]);
            if(wr >= 30) roles.Add(RoleIds[5]);

            return roles;
        }
    }
}