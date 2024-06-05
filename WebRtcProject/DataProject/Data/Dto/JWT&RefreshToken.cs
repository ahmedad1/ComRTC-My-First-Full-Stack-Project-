using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProject.Data.Dto
{
    public class JWT_RefreshToken
    {
        public string JwtToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime Expiration { get; set; }
        public string? Email { get; set; }
        public string? Username { get; set; }
        public string? FullName { get; set; }
        public bool EmailConfirmed { get; set; } = true;
        public bool Success=true;
        
    }
}
