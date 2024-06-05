using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProject.Data
{
    public class BlackListJwt
    {
        [Key]
        public string TokenString { get; set; }
    }
}
