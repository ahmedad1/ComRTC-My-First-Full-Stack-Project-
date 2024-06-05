using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProject.Data.Dto
{
    public class LoginDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Recaptcha { get; set; }
    }
}
