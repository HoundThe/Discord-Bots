using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using HoundBot.Models;
using HoundBot.Repository;
using HoundBot.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HoundBot
{
    public class HoundBot
    {
        private readonly Config _config;
        private readonly ServiceProvider _services;
        private readonly DiscordSocketClient _discordClient;
        private readonly CommandService _commandService;
        public HoundBot()
        {
            _discordClient = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Warning,
                MessageCacheSize = 200
            });

            _commandService = new CommandService(new CommandServiceConfig
            {
                CaseSensitiveCommands = false,
                LogLevel = LogSeverity.Warning,
                DefaultRunMode = RunMode.Sync
            });

            var builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("config.json", false, true);

            var configuration = builder.Build();

            _config = configuration.Get<Config>();
            
            var starboardService = new StarboardService(_discordClient, _config);
            
            _services = new ServiceCollection()
                .AddSingleton(_discordClient)
                .AddSingleton(_commandService)
                .AddSingleton(_config)
                .AddSingleton(starboardService)
                .AddSingleton<QuoteService>()
                .AddSingleton<CommandHandlingService>()
                .BuildServiceProvider();
        }

        public async Task Run()
        {
            _discordClient.Log += Log;
            _commandService.Log += Log;
            
            await _discordClient.LoginAsync(TokenType.Bot, _config.BotToken);
            await _discordClient.StartAsync();
            await _discordClient.SetGameAsync(_config.StatusMessage);
            await _services.GetRequiredService<CommandHandlingService>().InitializeAsync();

            await Task.Delay(-1);
        }
        
        private static Task Log(LogMessage message)
        {
            Console.WriteLine(message.ToString());
            return Task.CompletedTask;
        }
    }
}