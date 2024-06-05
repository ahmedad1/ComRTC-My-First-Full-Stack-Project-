using DataProject.Data.Dto;
using MediatR;


namespace DataProject.CQRS.Queries
{
    public record Login(LoginDto loginDto):IRequest<JWT_RefreshToken>;
   
}
