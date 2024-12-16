namespace DBLayer.Models
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public User User { get; set; } = null!;
        public string Token { get; set; } = null!;
        public DateTime ExpirationDate { get; set; }
    }
}
