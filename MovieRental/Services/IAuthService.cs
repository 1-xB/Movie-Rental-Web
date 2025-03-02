namespace MovieRental.Services;

using Auth_course.Entity.Models;
using Dtos;
using Entity;

public interface IAuthService {
	Task<Users?> RegisterAsync(UserRegisterDto request);
	Task<Users?> RegisterAdminAsync(UserRegisterDto request);
	Task<TokenResponseDto?> LoginAsync(UserLoginDto request);
	Task<TokenResponseDto?> RefreshTokensAsync(RefreshTokenRequestDto request, string s);
	bool IsAccessTokenValid(string accessToken);
}
