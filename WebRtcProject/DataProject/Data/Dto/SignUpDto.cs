using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProject.Data.Dto
{
    public class SignUpDto
    {
       
        public required string FullName { get; set; }
       
        public  required string UserName { get; set; }
        
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string Recaptcha { get; set; }

    }
}
