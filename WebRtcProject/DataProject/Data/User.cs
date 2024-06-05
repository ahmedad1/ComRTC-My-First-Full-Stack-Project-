



using Microsoft.AspNetCore.Identity;

namespace DataProject.Data
{
    public class User:IdentityUser
    {

        public string FullName { get; set; }
        public virtual RefreshToken RefreshToken { get; set; }
        public virtual Verification VerificationCode { get; set; }
        public virtual IdentityToken IdentityToken { get; set; }
        public virtual List<Group> Groups { get; set; }=new List<Group>();
        public virtual List<UserConnection> UserConnections { get; set; }=new List<UserConnection>();

    }
}
