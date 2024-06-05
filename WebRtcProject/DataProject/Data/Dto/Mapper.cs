using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProject.Data.Dto
{
    public class Mapper : Profile
    {
        public Mapper()
        {
            CreateMap<SignUpDto, User>();
            CreateMap<LoginDto, User>();
           
        }
    }
}
