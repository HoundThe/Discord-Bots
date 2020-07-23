namespace HoundBot.Models
{
    public class Config
    {
        public string BotToken { get; set; }
        public string BotPrefix { get; set; }
        public string StatusMessage { get; set; }
        public ulong StarboardChannelId { get; set; }
        public string ConnectionString { get; set; }
        public string StarboardUnicodeReaction { get; set; }
        public int StarboardReactionMinimum { get; set; }
    }
}