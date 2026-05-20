using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;

namespace ChatApp.Core.Utilities.Middleware
{
	// --- ÖZEL EXCEPTION SINIFLARI (Middleware dışına, bağımsız olarak taşındı) ---
	public class UserNotFoundException : Exception
	{
		public UserNotFoundException(string message) : base(message) { }
	}

	public class StatusValidationException : Exception
	{
		public StatusValidationException(string message) : base(message) { }
	}


	// 401 Unauthorized durumları için özel sınıf
	public class UnauthorizedException : Exception
	{
		public UnauthorizedException(string message) : base(message) { }
	}


	// --- MIDDLEWARE ---
	public class ExceptionMiddleware
	{
		private readonly RequestDelegate _next;

		public ExceptionMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			try
			{
				await _next(context);
			}
			catch (Exception ex)
			{
				await HandleExceptionAsync(context, ex);
			}
		}

		private static Task HandleExceptionAsync(HttpContext context, Exception ex)
		{
			context.Response.ContentType = "application/json";

			// Varsayılan olarak 500 Internal Server Error kabul ediyoruz
			var statusCode = HttpStatusCode.InternalServerError;
			string message = "Sistemsel bir hata oluştu."; // Güvenlik için genel mesaj

			// Fırlatılan hatanın tipine göre HTTP Durum Kodunu ve Mesajı eşliyoruz
			switch (ex)
			{
				case UserNotFoundException:
					statusCode = HttpStatusCode.NotFound; // 404
					message = ex.Message;
					break;

				case StatusValidationException:
					statusCode = HttpStatusCode.BadRequest; // 400
					message = ex.Message;
					break;
				case UnauthorizedException:
					statusCode = HttpStatusCode.Unauthorized; // 401
					message = ex.Message;
					break;


				// İleride başka exception tipleri eklemek istersen buraya case ekleyebilirsin
				default:
					// Sistemsel hatalarda ex.Message'ı loglayabilirsin ama dışarıya vermemek güvenlidir.
					break;
			}

			context.Response.StatusCode = (int)statusCode;

			// Standart bir JSON objesi oluşturuyoruz: { "message": "..." }
			var responseObj = new { Message = message };
			var jsonResult = JsonSerializer.Serialize(responseObj);

			return context.Response.WriteAsync(jsonResult);
		}
	}
}