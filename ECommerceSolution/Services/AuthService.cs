using BCrypt.Net;
using ECommerceSolution.DTOs.Auth;
using ECommerceSolution.Entities;
using ECommerceSolution.Helpers;
using ECommerceSolution.Interfaces;

namespace ECommerceSolution.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly JwtHelper _jwtHelper;
        
        //hello

        public AuthService(
            IUserRepository userRepository,
            JwtHelper jwtHelper)
        {
            _userRepository = userRepository;
            _jwtHelper = jwtHelper;
        
        }

        public async Task<AuthResponseDto> RegisterAsync(
            RegisterDto dto)
        {
            var existingUser =
                await _userRepository
                .GetByEmailAsync(dto.Email);

            if (existingUser != null)
            {
                throw new Exception(
                    "Email already exists.");
            }

            var user = new User
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                PasswordHash =
                    BCrypt.Net.BCrypt.HashPassword(
                        dto.Password),

                Role = "Customer"
            };

            await _userRepository
                .AddUserAsync(user);

            await _userRepository
                .SaveChangesAsync();

            // =========================
            // SEND WELCOME EMAIL
            

            var token =
                _jwtHelper.GenerateToken(user);

            return new AuthResponseDto
            {
                Token = token,
                Email = user.Email,
                Role = user.Role
            };
        }

        public async Task<AuthResponseDto> LoginAsync(
            LoginDto dto)
        {
            var user =
                await _userRepository
                .GetByEmailAsync(dto.Email);

            if (user == null)
            {
                throw new Exception(
                    "Invalid email or password.");
            }

            bool isValid =
                BCrypt.Net.BCrypt.Verify(
                    dto.Password,
                    user.PasswordHash);

            if (!isValid)
            {
                throw new Exception(
                    "Invalid email or password.");
            }

            var token =
                _jwtHelper.GenerateToken(user);

            return new AuthResponseDto
            {
                Token = token,
                Email = user.Email,
                Role = user.Role,
                FullName =
                    $"{user.FirstName} {user.LastName}"
            };
        }
    }
}