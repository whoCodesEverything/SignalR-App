using ChatApp.Core.DataAccess.EFEntity;
using ChatApp.DataAccess.Abstract;
using ChatApp.DataAccess.Concrete.DbContexts;
using ChatApp.Entities.Dtos;
using ChatApp.Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.DataAccess.Concrete
{
    public class ChatRepository : EfRepositoryBase<Chat>, IChatRepository
    {


        //public async Task<List<Chat>> GetChatsAsync(Guid userId, Guid toUserId, CancellationToken cancellationToken)
        //{
        //   return await _context.Chats.Where(x =>
        //        (x.UserId == userId && x.ToUserId == toUserId) ||
        //        (x.UserId == toUserId && x.ToUserId == userId))
        //    .OrderBy(x => x.Date)
        //    .ToListAsync(cancellationToken);
        //}

        //public async Task<List<User>> GetUsersAsync(CancellationToken cancellationToken)
        //{
        //   return await _context.Users.OrderBy(x=>x.Name).ToListAsync(cancellationToken);
        //}

        //public async Task SendMessageAsync(Chat chat, CancellationToken cancellationToken)
        //{
        //    await _context.Chats.AddAsync(chat, cancellationToken);
        //    await _context.SaveChangesAsync();
        //}
        public ChatRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<List<ChatDetailDto>> GetChatDetails(Guid id)
        {
            var result = (from chat in _context.Set<Chat>()
                          join user in _context.Set<User>()
                          on chat.UserId equals user.ID
                          where chat.ID == id
                          select new ChatDetailDto
                          {
                              ChatId = chat.ID,
                              Message = chat.Message,
                              Name = user.Name
                          });

            return await result.ToListAsync();
        }
    }
}
