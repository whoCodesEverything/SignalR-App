using ChatApp.Business.Abstract;
using ChatApp.Entities.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ChatController : ControllerBase
    {
        private readonly IChatManager _chatManager;

        public ChatController(IChatManager chatManager)
        {
            _chatManager = chatManager;
        }


        [HttpGet("GetChats")]

        public async Task<IActionResult> GetChats(Guid userId, Guid toUserId, CancellationToken cancellationToken)
        {

            var chats = await _chatManager.GetChatsAsync(userId, toUserId, cancellationToken);
            return Ok(chats);

        }

        [HttpGet("SendMessage")]
        public async Task<IActionResult> SendMessage(SendMessageDto request, CancellationToken cancellationToken)
        {

            await _chatManager.SendMessageAsync(request, cancellationToken);
            return Ok();
        }

    }
}
