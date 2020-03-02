using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UrChat.Extensions;
using UrChat.Extensions.Pagination;
using UrChat.Forms;
using UrChat.Services;

namespace UrChat.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly MessageService _messageService;

        public MessageController(MessageService messageService)
        {
            _messageService = messageService;
        }

        // GET api/message/
        [HttpGet]
        public async Task<IActionResult> GetMessages([FromQuery] PaginationParams paginationParams)
        {
            return this.FromServiceOperationResult(
                await _messageService.GetMessagesAsync(paginationParams)
            );
        }
        
        // POST api/message
        [HttpPost]
        public async Task<IActionResult> PostMessage([FromBody] PostMessageForm form)
        {
            return this.FromServiceOperationResult(
                await _messageService.SendMessageAsync(this.GetRequestUserId(), form)
            );
        }
    }
}