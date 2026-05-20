using ChatApp.Business.Abstract;
using ChatApp.DataAccess.Concrete.DbContexts;
using ChatApp.Entities.Models;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace ChatApp.Business.Hub
{
    /*public class ChatHub(ApplicationDbContext context) : Microsoft.AspNetCore.SignalR.Hub
    {

        public static Dictionary<string, Guid> Users = new();

        public async Task Connect(Guid userId)
        {
            Users.Add(Context.ConnectionId, userId);
            User? user = await context.Users.FindAsync(userId);

            if(user is not null)
            {
                user.Status = "online";
                await context.SaveChangesAsync();
                await Clients.All.SendAsync("Users", user);
            }
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            Guid userId;
            Users.TryGetValue(Context.ConnectionId, out userId);
            User? user = await context.Users.FindAsync(userId);

            if(user is not null)
            {
                user.Status = "offline";
                await context.SaveChangesAsync();
                await Clients.All.SendAsync("Users", user);
            }
        }
    }*/


    public class ChatHub : Microsoft.AspNetCore.SignalR.Hub
    {

        private readonly IUserService _userService;
        public static readonly ConcurrentDictionary<string, Guid> ConnectedUsers = new();

        public ChatHub(IUserService userService)
        {
         _userService= userService;   
        }

        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var userIdStr = httpContext?.Request.Query["userId"].ToString();

            if (Guid.TryParse(userIdStr, out Guid userId))
            {
                ConnectedUsers.AddOrUpdate(Context.ConnectionId, userId, (key, val) => userId);

                await _userService.UpdateUserStatusAsync(userId, "online");

                await Clients.All.SendAsync("Users", new { id = userId, status = "online" });
            }
            else
            {
                Context.Abort();
            }

              await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {

            if (ConnectedUsers.TryRemove(Context.ConnectionId, out Guid userId))
            {
                await _userService.UpdateUserStatusAsync(userId, "offline");
                await Clients.All.SendAsync("Users", new { id = userId, status = "offline" });
            }
            await base.OnDisconnectedAsync(exception);

        }


    }


}

