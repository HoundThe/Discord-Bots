using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using HoundBot.Models;
using HoundBot.Preconditions;
using HoundBot.Services;
using Microsoft.EntityFrameworkCore;

namespace HoundBot.Modules
{
    public class MiscModule : ModuleBase<SocketCommandContext>
    {
        private QuoteService _quoteService;

        public MiscModule(QuoteService quoteService)
        {
            _quoteService = quoteService;
        }
        
        [Command("cicina", RunMode = RunMode.Async)]
        [Ratelimit(1, 5, Measure.Minutes)]
        public async Task CicinaAsync()
        {
            double length;

            Random rnd = new Random();
            length = rnd.NextDouble() * 35;
            length = Math.Round(length, 1);

            if (length < 0.5) await ReplyAsync(@"¯\_(ツ)_/¯ Nemáš cikulu");
            else
                await ReplyAsync(Context.User.Mention + " Dĺžka tvojej ciciny je " + length +
                                 " centimetrov <:resttW:406445345745141780>");
        }
        
        [Command("iq", RunMode = RunMode.Async)]
        [Ratelimit(1, 5, Measure.Minutes)]
        public async Task IqAsync()
        {
            Random rnd = new Random();
            var length = rnd.Next(0, 200);
            await ReplyAsync(
                Context.User.Mention + " Tvoje IQ je " + length + " gratuluju <:resttW:406445345745141780>");
        }
        
        [Command("azkaban")]
        public async Task AzkabanAsync()
        {
            var channel = Context.Client.Guilds.FirstOrDefault(x => x.Id == 406434164082999306)
                ?.Channels.FirstOrDefault(x => x.Id == 406440517862162455);
            if(channel == null)
                return;

            var traktor = Context.Client.Guilds.FirstOrDefault(x => x.Id == 406434164082999306)?.GetUser(305105812689256450);

            if (traktor != null) await traktor.ModifyAsync(x => x.Channel = Optional.Create(channel as IVoiceChannel));
        }
        
        [Command("hlaska")]
        public async Task GetQuoteAsync() =>
            await ReplyAsync((await _quoteService.GetQuoteAsync()).Content);

        [Command("add hlasku")]
        [Alias("add hlaska")]
        public async Task AddQuoteAsync([Remainder] string input)
        {
            await _quoteService.SaveQuoteAsync(new Quote
            {
                Content = input,
                Date = DateTime.Now,
                InvokerName = Context.User.Username + "#" + Context.User.Discriminator
            });
            await ReplyAsync("Successfully added.");
        }
    }
}