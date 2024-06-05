using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProject.Data
{
    public class Group
    {
        public int Id { get; set; }
        public string GroupName { get; set; }
        public virtual List<User> Users { get; set; } = new List<User>();
    }
}
