using DataProject.CQRS.Commands;
using DataProject.CQRS.Queries;
using DataProject.Data;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.IdentityModel.Tokens.Jwt;


namespace DataProject.SignalR
{
    [Authorize]
    public  class MainHub:Hub
    {
        
        IMediator mediator;
     
        public MainHub(IMediator mediator,AppDbContext context)
        {
       
            this.mediator = mediator;

        }
        public  override async  Task OnConnectedAsync()
        {

            if (!Context.GetHttpContext()!.Request.Cookies.TryGetValue("jwt", out string? val))
            {
                 await base.OnConnectedAsync();
            }
            val  = (string)new JwtSecurityTokenHandler().ReadJwtToken(val).Payload[JwtRegisteredClaimNames.NameId];
            var user=await  mediator.Send(new AddUserConnection(val, Context.ConnectionId));
            var groupsOfUser = user.Groups.ToList();
            if(groupsOfUser != null)
            foreach (var group in groupsOfUser)
            {
               await Groups.AddToGroupAsync(Context.ConnectionId, group.GroupName);
               await  Clients.Group(group.GroupName).SendAsync("isOnline", user.UserName);


            }
            await  base.OnConnectedAsync();
        }

        public async Task SendToGroup(string group,string message)
        {
            if (!Context.GetHttpContext()!.Request.Cookies.TryGetValue("jwt", out string? val))
                return;
            var from =(string) new JwtSecurityTokenHandler().ReadJwtToken(val).Payload[JwtRegisteredClaimNames.Name];
            await Clients.Group(group).SendAsync("groupRecieveMessage", from, message);

        }
        public async Task SendSdp(string username,string fullname, string sdp,string destuser)
        {


           var dest= await mediator.Send(new GetConnectionIdOfUser(destuser));


            await Clients.Client(dest).SendAsync("getSdpOfUser",username,fullname, sdp);
         
           

        }
        public async Task GetUsersOfGroup(string groupName)
        {
            var groups=await mediator.Send(new GetUsersOfGroup(groupName));
            await  Clients.Caller.SendAsync("getUsers", groups);

        }
        public async Task ReplySdp(string from,string username,string sdp)
        {
            //var from = await mediator.Send(new GetByUserConnection(Context.ConnectionId));
            var connectionId=await mediator.Send(new GetConnectionIdOfUser(username));
           
            await Clients.Client(connectionId).SendAsync("getSdpReplyOfUser", from, sdp);
        }
        public async Task AddToGroup(string groupName)
        {
            var user = await mediator.Send(new GetByUserConnection(Context.ConnectionId));
            foreach (var userConnection in user.UserConnections)
            {
                await Groups.AddToGroupAsync(userConnection.ConnectionId, groupName);

            }
            await mediator.Send(new AddUserToGroup(groupName, user));
         
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            
            var user = await mediator.Send(new GetByUserConnection(Context.ConnectionId));
            if (user != null)
            {
                await mediator.Send(new RemoveUserConnection(user, Context.ConnectionId));

            }

            var groups = user!.Groups.ToList();

            foreach (var group in groups)
            {
               await Clients.Group(group.GroupName).SendAsync("isOffline", user.UserName);
            }
            //delete all groups related
            await mediator.Send(new FireUserFromHisGroup(user));
          
            
            await  base.OnDisconnectedAsync(exception);
        }

        
    }
}
