using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProject.Data
{
    public class IdentityToken
    {
        public string UserId  { get; set; }
        public string Token { get; set; }
        public DateTime ExpiresAt { get; set; }
        public virtual User User { get; set; }
    }
}
