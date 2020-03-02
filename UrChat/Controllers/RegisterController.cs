using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UrChat.Extensions;
using UrChat.Forms;
using UrChat.Services;

namespace UrChat.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class RegisterController : ControllerBase
    {
        private readonly RegistrationService _registrationService;

        public RegisterController(RegistrationService registrationService)
        {
            _registrationService = registrationService;
        }


        // POST /api/register
        public async Task<IActionResult> Register([FromBody] RegistrationForm registrationForm)
        {
            return this.FromServiceOperationResult(
                await _registrationService.RegisterUserAsync(registrationForm)
            );
        }
    }
}