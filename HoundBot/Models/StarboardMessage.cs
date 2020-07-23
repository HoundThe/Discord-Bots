namespace HoundBot.Models
{
    public class StarboardMessage
    {
        public int Id { get; set; }
        public ulong MessageId { get; set; }
        public string Content { get; set; }
        public string Author { get; set; }
    }
}