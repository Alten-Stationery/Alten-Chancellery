namespace ServiceLayer.DTOs
{
    public class RefreshTokenDTO
    {
        public string UserId { get; set; } = null!;
        public UserDTO User { get; set; } = null!;
        public string Token { get; set; } = null!;
        public DateTime ExpirationDate { get; set; }
    }
}
