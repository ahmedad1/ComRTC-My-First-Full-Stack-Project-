using DataProject.Data;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProject.CQRS.Commands
{
    public record RemoveUserConnection(User user, string userConnection):IRequest;
    
}
