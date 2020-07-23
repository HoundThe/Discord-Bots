using System;

namespace HoundBot.Models
{
    public class Quote
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string InvokerName { get; set; }
        public DateTime Date { get; set; }
    }
}