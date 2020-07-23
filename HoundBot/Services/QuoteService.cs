using System;
using System.Linq;
using System.Threading.Tasks;
using Discord.WebSocket;
using HoundBot.Models;
using HoundBot.Repository;
using Microsoft.EntityFrameworkCore;

namespace HoundBot.Services
{
    public class QuoteService
    {
        private readonly  Config _config;
        
        public QuoteService(DiscordSocketClient client, Config config)
        {
            _config = config;
        }

        public async Task<Quote> GetQuoteAsync()
        {
            Random rnd = new Random();
            await using var dbContext = new DatabaseContext(_config.ConnectionString);
            Random rand = new Random();
            int toSkip = rand.Next(0, dbContext.Quotes.Count());
            return await dbContext.Quotes.Skip(toSkip).Take(1).FirstOrDefaultAsync();
        }
        
        public async Task SaveQuoteAsync(Quote quote)
        {
            await using var dbContext = new DatabaseContext(_config.ConnectionString);
            await dbContext.Quotes.AddAsync(quote);
            await dbContext.SaveChangesAsync();
        }
    }
}