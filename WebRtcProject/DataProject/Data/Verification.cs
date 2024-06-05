using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProject.Data
{
   
    public class Verification
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public  string UserId { get;set; }
        public string Code { get; set; }
        public DateTime ExpiresOn { get; set; }
        public bool IsExpired => DateTime.UtcNow > ExpiresOn;
        public virtual User User { get; set; }

    }
}
