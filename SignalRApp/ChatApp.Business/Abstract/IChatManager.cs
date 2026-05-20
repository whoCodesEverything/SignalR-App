using ChatApp.Entities.Dtos;
using ChatApp.Entities.Models;

namespace ChatApp.Business.Abstract
{
    public interface IChatManager
    {
       // List<User> GetUser();

        

        Task<List<Chat>> GetChatsAsync(Guid userId, Guid toUserId, CancellationToken cancellationToken);

        Task SendMessageAsync(SendMessageDto request,CancellationToken cancellationToken);
    }
}
