using ChatApp.Business.Abstract;
using ChatApp.Core.Utilities.Middleware;
using ChatApp.DataAccess.Abstract;
using ChatApp.Entities.Dtos;
using ChatApp.Entities.Models;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace ChatApp.Business.Concete
{
	public class UserManager : IUserService
	{

		private readonly IUserRepository _userRepository;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public UserManager(IUserRepository userRepository, IHttpContextAccessor httpContextAccessor)
		{
			_userRepository = userRepository;
			_httpContextAccessor = httpContextAccessor;

		}

		public async Task<List<User>> GetUser(CancellationToken cancellationToken)
		{
			return await _userRepository.GetAllAsync(x => true, cancellationToken);
		}

		public async Task<User> RegisterUser(RegisterDto request, CancellationToken ct)
		{

			if (await _userRepository.IsNameExistsAsync(request.Name, ct))
				throw new ValidationException("Bu kullanıcı adı zaten kullanılıyor");

			var user = new User
			{
				Name = request.Name,
				Avatar = request.Avatar,
				Status = "offline"
			};

			await _userRepository.AddAsync(user, ct);


			return user;
		}

		public async Task<LoginResponseDto> LoginUser(LoginDto request, CancellationToken cancellationToken = default)
		{
			var user = await _userRepository.GetAsync(x => x.Name == request.Name, cancellationToken);

			if (user == null)
				throw new ValidationException("kullanıcı adı veya şifre hatalı");

			var token = Guid.NewGuid().ToString();
			return new LoginResponseDto
			{
				//Name = user.Name,
				Token = token

			};
		}

		public async Task<User> GetAuthenticatedUserAsync(CancellationToken cancellationToken = default)
		{
			// 1. İstek atan kullanıcının ClaimsPrincipal nesnesini alıyoruz
			var userPrincipal = _httpContextAccessor.HttpContext?.User;

			// Kullanıcı giriş yapmadıysa: Core'daki UnauthorizedException fırlatılıyor (HTTP 401)
			if (userPrincipal == null || userPrincipal.Identity?.IsAuthenticated != true)
			{
				throw new UnauthorizedException("Sisteme giriş yapmış bir kullanıcı bulunamadı.");
			}

			// 2. Token içerisine gömdüğünüz ID claim'ini okuyoruz
			var userIdClaim = userPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			if (string.IsNullOrEmpty(userIdClaim))
			{
				throw new UnauthorizedException("Kullanıcı kimlik bilgisi doğrulanamadı.");
			}

			// 3. String ID'yi Guid'e güvenli bir şekilde çeviriyoruz
			// Doğrudan Guid.Parse yerine TryParse kullanarak sistemsel çökme (FormatException) riskini sıfırlıyoruz
			if (!Guid.TryParse(userIdClaim, out Guid userId))
			{
				throw new StatusValidationException("Geçersiz kullanıcı kimliği formatı."); // HTTP 400
			}

			// Veritabanından kullanıcıyı çekiyoruz
			var user = await _userRepository.GetAsync(x => x.ID == userId, cancellationToken);

			// Kullanıcı veritabanında yoksa: Core'daki UserNotFoundException fırlatılıyor (HTTP 404)
			if (user == null)
			{
				throw new UserNotFoundException("Giriş yapmış kullanıcı veritabanında bulunamadı.");
			}

			return user;
			
		}

		public async Task UpdateUserStatusAsync(Guid userId, string status, CancellationToken cancellationToken = default)
		{
			var user = await _userRepository.GetAsync(x => x.ID == userId, cancellationToken);

			if (user != null)
			{
				user.Status = status;

				await _userRepository.UpdateAsync(user, cancellationToken);
			}
		}
	}
}
