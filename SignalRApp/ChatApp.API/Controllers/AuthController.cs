using ChatApp.Business.Abstract;
using ChatApp.Entities.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;

        }
        [HttpGet("GetUser")]
        public async Task<IActionResult> GetUser(CancellationToken cancellationToken)
        {
            var users = await _userService.GetUser(cancellationToken);
            return Ok(users);
        }

        [HttpPost("RegisterUser")]
        public async Task<IActionResult> RegisterUser(RegisterDto request, CancellationToken cancellationToken)
        {
            var user = await _userService.RegisterUser(request, cancellationToken);
            return Ok(user);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto request, CancellationToken cancellationToken)
        {
            var result = await _userService.LoginUser(request);
            return Ok(result);
        }


		[Authorize] // Sadece giriş yapmış kullanıcılar erişebilir
		[HttpGet("GetAuthenticatedUser")]
		public async Task<IActionResult> GetAuthenticatedUser(CancellationToken cancellationToken)
		{
			// Controller'da hazır bulunan 'User' nesnesi (ClaimsPrincipal), istek atan token'ın bilgilerini taşır.
			var user = await _userService.GetAuthenticatedUserAsync(cancellationToken);

			if (user == null)
				return NotFound("Giriş yapmış kullanıcı bulunamadı.");

			return Ok(user);
		}

		[HttpPut("UpdateStatus")]
		public async Task<IActionResult> UpdateUserStatus(Guid userId, string status, CancellationToken cancellationToken)
		{
			// Try-Catch YOK! Hata çıkarsa otomatik olarak yukarıdaki Middleware yakalayacak.
			await _userService.UpdateUserStatusAsync(userId, status, cancellationToken);

			return Ok(new { Message = "Kullanıcı durumu başarıyla güncellendi." });
		}
	}

}

