using ECommerceSolution.DTOs.Auth;

namespace ECommerceSolution.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterAsync(
            RegisterDto dto);

        Task<AuthResponseDto> LoginAsync(
            LoginDto dto);
    }
}