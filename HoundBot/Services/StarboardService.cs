using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using HoundBot.Models;
using HoundBot.Repository;
using Microsoft.EntityFrameworkCore;

namespace HoundBot.Services
{
    public class StarboardService
    {
        private readonly Config _config;

        public StarboardService(DiscordSocketClient client, Config config)
        {
            _config = config;
            client.ReactionAdded += ReactionAdded;
        }

        private async Task ReactionAdded(Cacheable<IUserMessage, ulong> cachedMessage, ISocketMessageChannel mesChannel,
            SocketReaction reaction)
        {
            if (reaction.Emote.Name != _config.StarboardUnicodeReaction)
                return;

            var message = await cachedMessage.GetOrDownloadAsync();

            await using var databaseContext = new DatabaseContext(_config.ConnectionString);

            if (await databaseContext.StarboardMessages.AnyAsync(x => x.MessageId == message.Id))
                return;

            if (message.Reactions[reaction.Emote].ReactionCount >= _config.StarboardReactionMinimum)
            {
                await databaseContext.AddAsync(new StarboardMessage
                {
                    MessageId = message.Id,
                    Author = message.Author.Username + "#" + message.Author.Discriminator,
                    Content = message.Content
                });
                await databaseContext.SaveChangesAsync();

                IGuild guild = (mesChannel as IGuildChannel)?.Guild;
                if (guild == null)
                    return;
                
                var builder = new EmbedBuilder()
                    .WithColor(Color.LightOrange)
                    .WithAuthor(message.Author)
                    .WithThumbnailUrl(message.Author.GetAvatarUrl())
                    .WithTimestamp(message.Timestamp)
                    .WithDescription(new StringBuilder()
                        .AppendLine(
                            $"_Posted by {message.Author.Username}#{message.Author.Discriminator} **[Jump]({message.GetJumpUrl()})**_")
                        .AppendLine()
                        .AppendLine("**Message**")
                        .AppendLine(message.Content)
                        .ToString()
                    );


                if (!TryAddImageAttachment(message, builder))
                    if (!TryAddImageEmbed(message, builder))
                        if (!TryAddThumbnailEmbed(message, builder))
                            TryAddOtherAttachment(message, builder);
                AddOtherEmbed(message, builder);

                var starboardChannel = await guild.GetTextChannelAsync(_config.StarboardChannelId);
                if (starboardChannel == null)
                    return;
                // Sending it to the Starboard text channel
                await starboardChannel.SendMessageAsync("", false, builder.Build());


            }
            bool TryAddImageAttachment(IMessage message, EmbedBuilder embed)
                {
                    var firstAttachment = message.Attachments.FirstOrDefault();
                    if (firstAttachment?.Height == null)
                        return false;

                    embed.WithImageUrl(firstAttachment.Url);

                    return true;
                }

                bool TryAddOtherAttachment(IMessage message, EmbedBuilder embed)
                {
                    var firstAttachment = message.Attachments.FirstOrDefault();
                    if (firstAttachment == null) return false;

                    embed.AddField($"Attachment (Size: {firstAttachment.Size / 1024}Kb)", firstAttachment.Url);

                    return true;
                }

                bool TryAddImageEmbed(IMessage message, EmbedBuilder embed)
                {
                    var imageEmbed = message.Embeds.Select(x => x.Image).FirstOrDefault(x => x is { });
                    if (imageEmbed is null)
                        return false;

                    embed.WithImageUrl(imageEmbed.Value.Url);

                    return true;
                }

                bool TryAddThumbnailEmbed(IMessage message, EmbedBuilder embed)
                {
                    var thumbnailEmbed = message.Embeds.Select(x => x.Thumbnail).FirstOrDefault(x => x is { });
                    if (thumbnailEmbed is null)
                        return false;

                    embed.WithImageUrl(thumbnailEmbed.Value.Url);

                    return true;
                }

                void AddOtherEmbed(IMessage message, EmbedBuilder embed)
                {
                    if (message.Embeds.Count == 0) return;

                    embed.AddField("Embed Type", message.Embeds.First().Type, true);
                }
        }
    }
}