using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UrChat.Extensions;
using UrChat.Forms;
using UrChat.Services;

namespace UrChat.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly LoginService _loginService;

        public LoginController(LoginService loginService)
        {
            _loginService = loginService;
        }


        // POST /api/login
        public async Task<IActionResult> Login([FromBody] LoginForm loginForm)
        {
            return this.FromServiceOperationResult(
                _loginService.Login(loginForm)
            );
        }
    }
}