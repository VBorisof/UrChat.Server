using System;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Configuration;
using UrChat.Data.UnitOfWork;
using UrChat.Forms;
using UrChat.Services.Shared;

namespace UrChat.Services
{
    public class LoginService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public LoginService(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }
        
        
        public ServiceOperationResult<string> Login(LoginForm form)
        {
            var user = _unitOfWork.Users
                .GetAsQueryable()
                .SingleOrDefault(u => u.Username == form.Username);

            if (user == null)   
            {
                return ServiceOperationResult.InvalidPassword<string>("Login/Password pair is invalid.");
            }
            
            if (!PasswordHasher.VerifyPasswordHash(form.Password, user.PasswordHash, user.PasswordSalt))   
            {
                return ServiceOperationResult.InvalidPassword<string>("Login/Password pair is invalid.");
            }

            return ServiceOperationResult.Ok(
                JwtTokenGenerator.GetTokenString(
                    user,
                    TimeSpan.FromDays(7),
                    Encoding.ASCII.GetBytes(_configuration["AppSecret"])
                ));
        }
    }
}