namespace UrChat.Data.Models
{
    public class User : ModelBase
    {
        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
    }
}