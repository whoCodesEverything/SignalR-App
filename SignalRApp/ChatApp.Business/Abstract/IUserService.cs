using ChatApp.Entities.Dtos;
using ChatApp.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Business.Abstract
{
    public interface IUserService
    {

        Task<List<User>> GetUser(CancellationToken cancellationToken);
        Task<User> RegisterUser(RegisterDto request, CancellationToken cancellationToken);
        Task<LoginResponseDto> LoginUser(LoginDto request, CancellationToken cancellationToken = default);
        Task UpdateUserStatusAsync(Guid userId, string status, CancellationToken cancellationToken = default);
        Task<User> GetAuthenticatedUserAsync(CancellationToken cancellationToken = default);


		
	}
}
