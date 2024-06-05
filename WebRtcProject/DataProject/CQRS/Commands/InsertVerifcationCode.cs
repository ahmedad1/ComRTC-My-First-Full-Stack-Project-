using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProject.CQRS.Commands
{
    public record InsertVerifcationCode (string email,string code):IRequest<bool>;
    
}
