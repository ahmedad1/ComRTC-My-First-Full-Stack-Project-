using Castle.Components.DictionaryAdapter.Xml;
using Castle.Core.Internal;
using DataProject.CQRS.Queries;
using DataProject.Data;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace SmallProject.Middlewares
{
    public class PreventBlackListTokensActionFilter:IAsyncActionFilter
    {
        
        IMediator mediator;
      
        
        public PreventBlackListTokensActionFilter( IMediator mediator)
        {
           
           
            this.mediator = mediator;
        }
       
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {

            var auth = context.HttpContext.GetEndpoint()?.Metadata.GetMetadata<AuthorizeAttribute>();

            if (auth == null)
            {
                await next();
                return;
            }
            var token = context.HttpContext.Request.Cookies["jwt"];

        
            if (await mediator.Send(new InBlackList(token)))
            {
                context.HttpContext.Response.StatusCode = 401;
                await context.HttpContext.Response.WriteAsync("Invalid Token");
            }
            else
                await next();
        }
    }
}
