using Microsoft.EntityFrameworkCore;


namespace DataProject.Data
{

    public class RefreshToken
    {
        public string Id { get; set; }
        public string Token { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ExpiresOn { get; set; }
        public DateTime? RevokedOn { get; set; }
        public bool IsExpired => DateTime.UtcNow.ToLocalTime() > ExpiresOn;
        public bool IsActive => !IsExpired && RevokedOn == null;
        public virtual User User { get; set; }
        public string UserId { get; set; }
        public RefreshToken()
        {
           Id  = Guid.NewGuid().ToString();
        }

    }
}
