using System.Linq;
using System.Threading.Tasks;
using UrChat.Data.Models;
using UrChat.Data.UnitOfWork;
using UrChat.Forms;
using UrChat.Services.Shared;
using UrChat.ViewModels;

namespace UrChat.Services
{
    public class RegistrationService
    {
        private readonly IUnitOfWork _unitOfWork;

        public RegistrationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        
        
        public async Task<ServiceOperationResult<UserViewModel>> RegisterUserAsync(RegistrationForm form)
        {
            var user = _unitOfWork.Users.GetAsQueryable()
                .SingleOrDefault(u => u.Username == form.Username);

            // If the User already exists, abort the registration
            if (user != null)
            {
                return ServiceOperationResult.Conflict<UserViewModel>(
                    "Username already taken!"
                );
            }

            user = new User()
            {
                Username = form.Username
            };

            var result = await ValidateAndStoreUserAsync(user, form.Password);
            if (!result.IsSuccessful)
            {				
                // TODO: Log the error
                return ServiceOperationResult<UserViewModel>.FromDataless(result);
            }
            return ServiceOperationResult.Ok(new UserViewModel
            {
                Id = user.Id,
                Username = user.Username,
            });
        }
        
        private async Task<ServiceOperationResult> ValidateAndStoreUserAsync(User user, string password)
        {
            if (string.IsNullOrEmpty(user.Username) || string.IsNullOrEmpty(password))
            {
                return ServiceOperationResult.BadRequest("Email and password cannot be empty!");
            }

            var userExists = _unitOfWork.Users
                .GetAsQueryable()
                .Any(au => au.Username == user.Username);

            if (userExists)
            {
                return ServiceOperationResult.Conflict($"User {user.Username} already exists");
            }

            PasswordHasher.CreatePasswordHash(password, out var passwordHash, out var passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            
            _unitOfWork.Users.Add(user);
            await _unitOfWork.SaveAsync();

            return ServiceOperationResult.Ok();
        }
    }
}