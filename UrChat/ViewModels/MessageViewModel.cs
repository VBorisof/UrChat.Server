using System;

namespace UrChat.ViewModels
{
    public class MessageViewModel
    {
        public long Id { get; set; }
        public string Message { get; set; }
        public long SenderId { get; set; }
        public string Sender { get; set; }
        public DateTime SentDate { get; set; }
    }
}