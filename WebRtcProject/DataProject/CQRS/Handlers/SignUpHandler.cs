using AutoMapper;
using DataProject.CQRS.Commands;
using DataProject.CQRS.Queries;
using DataProject.Data;
using DataProject.Data.Dto;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DataProject.CQRS.Handlers
{
    public class SignUpHandler : IRequestHandler<SignUp, bool>
    {
        private readonly UserManager<User> userManager;
        private readonly IMapper mapper;
        private readonly IMediator mediator;


        public SignUpHandler(UserManager<User> userManager, IMapper mapper,IMediator mediator)
        {
            this.userManager = userManager;
            this.mapper = mapper;
            this.mediator = mediator;
        }
        public async Task<bool>Handle(SignUp request, CancellationToken cancellationToken)
        {
            
            var user=mapper.Map<User>(request.signUpDto);
            var IsAlreadyInDataBase = await mediator.Send(new IsAlreadyInDatabase(user));

            if (IsAlreadyInDataBase)
            {
                return false;
            }
            var result = await userManager.CreateAsync(user,request.signUpDto.Password);

            if (result.Succeeded)
            {
                return true;
               
            }
            return false;
        }
    }
}
