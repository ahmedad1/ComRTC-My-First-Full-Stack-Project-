using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProject.Data.Dto
{
    public class ContactUsDto
    {
        public string? Email { get; set; }  
        public string Message { get; set; }
        public string Recaptcha { get; set; }
    }
}
