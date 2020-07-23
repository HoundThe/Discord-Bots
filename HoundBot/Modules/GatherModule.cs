using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace HoundBot.Modules
{
    public class MixModule : ModuleBase<SocketCommandContext>
    {

        /// <summary>
        /// Create random teams from X users in the voice channel
        /// </summary>
        /// <param name="teamSize">Count of players in one team</param>
        /// <returns>Outputs random teams</returns>
        [Command("mix", RunMode = RunMode.Async)]
        [Alias("gather", "scramble")]
        public async Task RandomGatherAsync(int teamSize = 4)
        {
            var voiceChannel = (Context.User as IGuildUser)?.VoiceChannel as SocketVoiceChannel;
            if (voiceChannel == null)
            {
                await ReplyAsync("You must be in a voice.");
                return;
            }

            List<string> playerNames = voiceChannel.Users.Select(x => x.Nickname ?? x.Username).ToList();

            if (teamSize <= 0)
            {
                await ReplyAsync("Team size cannot be less than 1.");
                return;
            }
            
            if (playerNames.Count < teamSize * 2 - 1)
            {
                await ReplyAsync("Not enough players for 2 teams.");
                return;
            }

            Random rnd = new Random();

            var team = new StringBuilder();

            var embed = new EmbedBuilder()
                .WithTitle("Mixed squads:")
                .WithColor(Color.Orange)
                .WithCurrentTimestamp();
            
            var teamNumber = 1;
            while (playerNames.Count > 0)
            {
                for (var j = 0; j < teamSize; j++)
                {
                    if (playerNames.Count == 0)
                        break;
                    var randomIndex = rnd.Next(0, playerNames.Count);
                    team.Append($" {playerNames[randomIndex]}");
                    playerNames.RemoveAt(randomIndex);
                }
                embed.AddField($"**Squad {teamNumber++}:**", team, true);
                team.Clear();
            }

            await ReplyAsync("", false, embed.Build());
        }

        /// <summary>
        /// Pick X captains from all players from users voice channel
        /// </summary>
        /// <param name="teamSize">Amount of players in one team</param>
        /// <returns>Prints N random captains</returns>
        [Command("cpt",  RunMode = RunMode.Async)]
        [Alias("kapitani", "captains", "capt", "kap", "kpt")]
        public async Task PickCaptainsAsync(int cptCount = 4)
        {
            var voiceChannel = (Context.User as IGuildUser)?.VoiceChannel as SocketVoiceChannel;
            if (voiceChannel == null)
            {
                await ReplyAsync("You must be in a voice.");
                return;
            }

            List<string> playerNames = voiceChannel.Users.Select(x => x.Nickname ?? x.Username).ToList();

            if (cptCount >= playerNames.Count)
            {
                await ReplyAsync("Too many captains.");
                return;
            }

            if (cptCount <= 0)
            {
                await ReplyAsync("Too few captains.");
                return;
            }
            Random rnd = new Random();

            var output = new StringBuilder();

            var embed = new EmbedBuilder()
                .WithTitle("Captains:")
                .WithColor(Color.Orange)
                .WithCurrentTimestamp();
            
            for (int i = 0; i < cptCount; ++i)
            {
                int randomIndex = rnd.Next(0, playerNames.Count-1);
                output.AppendLine(playerNames[randomIndex]);
                playerNames.RemoveAt(randomIndex);
                
                embed.AddField($"**Captain {i + 1}:**", output.ToString(), true);
                output.Clear();
            }

            await ReplyAsync("", false, embed.Build());
        }
   }
}