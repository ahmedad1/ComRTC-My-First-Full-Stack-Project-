using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProject.Data
{
    public  class UserConnection
    {
        public string ConnectionId { get; set; }
        public string UserId { get; set; }
        public virtual User User { get; set; }
    }
}
