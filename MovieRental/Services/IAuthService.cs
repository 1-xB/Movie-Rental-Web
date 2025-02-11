using Auth_course.Entity.Models;
using MovieRental.Dtos;
using MovieRental.Entity;

namespace MovieRental.Services;

public interface IAuthService
{
    Task<Users?> RegisterAsync(UserRegisterDto request);
    Task<TokenResponseDto?> LoginAsync(UserLoginDto request);
    Task<TokenResponseDto?> RefreshTokensAsync(RefreshTokenRequestDto request, string s);
}