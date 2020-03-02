namespace UrChat.Data.Models
{
    public class Message : ModelBase
    {
        public string Content { get; set; }
        
        public long SenderId { get; set; }
        public User Sender { get; set; }
    }
}