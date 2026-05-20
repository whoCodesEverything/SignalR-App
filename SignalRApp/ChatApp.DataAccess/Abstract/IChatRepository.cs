using ChatApp.Core.DataAccess;
using ChatApp.Entities.Dtos;
using ChatApp.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.DataAccess.Abstract
{
    public interface IChatRepository:IEntityRepository<Chat>
    {
        //Task<List<User>> GetUsersAsync(CancellationToken cancellationToken);

        //Task<List<Chat>> GetChatsAsync(Guid userId, Guid toUserId, CancellationToken cancellationToken);

        //Task SendMessageAsync(Chat chat, CancellationToken cancellationToken);

        Task<List<ChatDetailDto>> GetChatDetails(Guid id);
    }
}
