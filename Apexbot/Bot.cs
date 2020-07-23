using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using ApexBot.Modules;
using ApexBot.Services;

namespace ApexBot
{
    public sealed class Bot
    {
        private readonly IConfigurationRoot _config;
        private readonly ServiceProvider _services;
        private readonly DiscordSocketClient _discordClient;
        private readonly CommandService _commandService;
        public Bot()
        {
            _discordClient = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Warning
            });

            _commandService = new CommandService(new CommandServiceConfig
            {
                CaseSensitiveCommands = false,
                LogLevel = LogSeverity.Warning,
                DefaultRunMode = RunMode.Sync
            });
            
            _config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("config.json")
                .Build();
            
            _services = new ServiceCollection()
                .AddSingleton(_discordClient)
                .AddSingleton(_commandService)
                .AddSingleton(_config)
                .AddSingleton<CommandHandlingService>()
                .AddSingleton<StatisticService>()
                .BuildServiceProvider();

        }

        public async Task Run()
        {
            _discordClient.Log += Log;
            _commandService.Log += Log;
            
            await _discordClient.LoginAsync(TokenType.Bot, _config["DiscordToken"]);
            await _discordClient.StartAsync();
            await _discordClient.SetGameAsync(_config["StatusMessage"]);
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