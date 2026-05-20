using ChatApp.Business.Abstract;
using ChatApp.Business.Hub;
using ChatApp.Core.Entities;
using ChatApp.DataAccess.Abstract;
using ChatApp.DataAccess.Concrete.DbContexts;
using ChatApp.Entities.Dtos;
using ChatApp.Entities.Models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Business.Concete
{
    public class ChatManager : IChatManager
    {
        private readonly IChatRepository _chatRepository;
        private readonly IUserRepository _userRepository;
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly ApplicationDbContext _context;
        public ChatManager(IChatRepository chatRepository, IUserRepository userRepository, IHubContext<ChatHub> hubContext)
        {
            _chatRepository = chatRepository;
            _userRepository = userRepository;
            _hubContext = hubContext;
        }

        public async Task<List<Chat>> GetChatsAsync(Guid userId, Guid toUserId, CancellationToken cancellationToken)
        {
            return await _chatRepository.GetAllAsync(x => (x.UserId == userId && x.ToUserId == toUserId) ||
                 (x.UserId == toUserId && x.ToUserId == userId),
            cancellationToken);
        }

        public async Task<List<User>> GetUser(CancellationToken cancellationToken)
        {
            return await _userRepository.GetAllAsync(x => true, cancellationToken);


            //throw new NotImplementedException();
        }

        public async Task SendMessageAsync(SendMessageDto request, CancellationToken cancellationToken)
        {
            Chat chat = new()
            {
                UserId = request.UserId,
                ToUserId = request.ToUserId,
                Message = request.Message,
                Date = DateTime.Now

            };

            await _chatRepository.AddAsync(chat, cancellationToken);
            await _context.Chats.AddAsync(chat, cancellationToken);



            var userConnection = ChatHub.ConnectedUsers.FirstOrDefault(p => p.Value == chat.ToUserId);

            if (!string.IsNullOrEmpty(userConnection.Key))
            {
                await _hubContext.Clients.Client(userConnection.Key).SendAsync("Messages",chat,cancellationToken);
            }


        }
    }
}
