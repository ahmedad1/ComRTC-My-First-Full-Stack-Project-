using DataProject.Data.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProject.CQRS.Commands
{
    public record ValidateCodeOfVerification(string email, int code) : IRequest<JWT_RefreshToken>;
}
