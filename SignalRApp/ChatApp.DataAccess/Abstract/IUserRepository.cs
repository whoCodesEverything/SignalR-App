using ChatApp.Core.DataAccess;
using ChatApp.Entities.Models;

namespace ChatApp.DataAccess.Abstract
{
    public interface IUserRepository : IEntityRepository<User>
    {
        //Task<bool> IsNameExistAsync(string name, CancellationToken cancellationToken);

        //Task<User> GetByNameAsync(string name, CancellationToken cancellationToken);

        //Task AddUserAsync(User user, CancellationToken cancellationToken);

        //Task UpdateUserAsync(User user, CancellationToken cancellationToken);

        Task<bool> IsNameExistsAsync(string name, CancellationToken cancellationToken);
    }
}
