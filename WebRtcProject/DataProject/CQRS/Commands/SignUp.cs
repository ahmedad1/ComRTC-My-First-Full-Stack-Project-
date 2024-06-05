using DataProject.Data.Dto;
using MediatR;

namespace DataProject.CQRS.Commands
{
  public  record class SignUp(SignUpDto signUpDto):IRequest<bool>;
    
}
