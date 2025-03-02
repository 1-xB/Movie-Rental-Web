namespace MovieRental.Services;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Auth_course.Entity.Models;
using Data;
using Dtos;
using Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

public class AuthService(DatabaseContext context, IConfiguration configuration) : IAuthService {
	public async Task<Users?> RegisterAsync(UserRegisterDto request) {
		if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password) ||
		    string.IsNullOrWhiteSpace(request.Mail)) {
			return null;
		}

		if (await context.Users.AnyAsync(u => u.Username == request.Username)) {
			return null;
		}

		var user = new Users();

		var hashedPassword = new PasswordHasher<Users>().HashPassword(user, request.Password);

		user.Username = request.Username;
		user.Mail = request.Mail;
		user.PasswordHash = hashedPassword;
		user.Role = "Client";

		await context.Users.AddAsync(user);
		await context.SaveChangesAsync();

		return user;
	}

	public async Task<TokenResponseDto?> LoginAsync(UserLoginDto request) {
		try {
			var user = await context.Users.FirstOrDefaultAsync(u => u.Username == request.UsernameOrMail) ??
			           await context.Users.FirstOrDefaultAsync(u => u.Mail == request.UsernameOrMail);
			if (user is null) {
				return null;
			}

			if (new PasswordHasher<Users>().VerifyHashedPassword(user, user.PasswordHash, request.Password) ==
			    PasswordVerificationResult.Failed) {
				return null;
			}

			return await CreateTokenResponse(user);
		}
		catch {
			return null;
		}
	}

	public async Task<TokenResponseDto?> RefreshTokensAsync(RefreshTokenRequestDto request, string accessToken) {
		if (!IsAccessTokenExpired(accessToken)) {
			return null;
		}

		var user = await ValidateRefreshTokenAsync(request.RefreshToken);
		if (user == null) {
			return null;
		}

		return await CreateTokenResponse(user);
	}

	public bool IsAccessTokenValid(string accessToken) {
		var tokenHandler = new JwtSecurityTokenHandler();
		var key = Encoding.UTF8.GetBytes(configuration.GetValue<string>("AppSettings:Token")!);
		try {
			tokenHandler.ValidateToken(accessToken,
				new TokenValidationParameters // Zwraca wyjątek jeśli token jest niepoprawny
				{
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(key),
					ValidateIssuer = true,
					ValidIssuer = configuration.GetValue<string>("AppSettings:Issuer"),
					ValidateAudience = true,
					ValidAudience = configuration.GetValue<string>("AppSettings:Audience"),
					ClockSkew = TimeSpan.Zero
				}, out _);

			return true;
		}
		catch {
			return false;
		}
	}

	public async Task<Users?> RegisterAdminAsync(UserRegisterDto request) {
		if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password) ||
		    string.IsNullOrWhiteSpace(request.Mail)) {
			return null;
		}

		if (await context.Users.AnyAsync(u => u.Username == request.Username)) {
			return null;
		}

		var user = new Users();

		var hashedPassword = new PasswordHasher<Users>().HashPassword(user, request.Password);

		user.Username = request.Username;
		user.Mail = request.Mail;
		user.PasswordHash = hashedPassword;
		user.Role = "Admin";

		await context.Users.AddAsync(user);
		await context.SaveChangesAsync();

		return user;
	}

	private async Task<Users?> ValidateRefreshTokenAsync(string refreshToken) {
		var user = await context.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);

		if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow) {
			return null;
		}

		user.RefreshToken = null;
		user.RefreshTokenExpiryTime = null;
		await context.SaveChangesAsync();
		return user;
	}


	private string CreateToken(Users user) {
		var claims = new List<Claim> {
			new(ClaimTypes.NameIdentifier, user.Id.ToString()),
			new(ClaimTypes.Name, user.Username),
			new(ClaimTypes.Email, user.Mail),
			new(ClaimTypes.Role, user.Role)
		};
		var key = new SymmetricSecurityKey(
			Encoding.UTF8.GetBytes(configuration.GetValue<string>("AppSettings:Token")!));
		var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

		var tokenDescriptor = new JwtSecurityToken(
			configuration.GetValue<string>("AppSettings:Issuer"),
			configuration.GetValue<string>("AppSettings:Audience"),
			claims,
			expires: DateTime.UtcNow.AddMinutes(30),
			signingCredentials: creds
		);

		return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
	}

	private async Task<string> GenerateAndSaveRefreshTokenAsync(Users user) {
		var refreshToken = GenerateRefreshToken();
		user.RefreshToken = refreshToken;
		user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
		await context.SaveChangesAsync();

		return refreshToken;
	}

	private string GenerateRefreshToken() {
		var randomNumber = new byte[32];
		using var rng = RandomNumberGenerator.Create();
		rng.GetBytes(randomNumber);
		return Convert.ToBase64String(randomNumber);
	}

	private async Task<TokenResponseDto> CreateTokenResponse(Users? user) =>
		new() { AccessToken = CreateToken(user), RefreshToken = await GenerateAndSaveRefreshTokenAsync(user) };

	private bool IsAccessTokenExpired(string accessToken) {
		var tokenHandler = new JwtSecurityTokenHandler();
		var key = Encoding.UTF8.GetBytes(configuration.GetValue<string>("AppSettings:Token")!);
		try {
			tokenHandler.ValidateToken(accessToken,
				new TokenValidationParameters // Zwraca wyjątek jeśli token jest niepoprawny
				{
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(key),
					ValidateIssuer = true,
					ValidIssuer = configuration.GetValue<string>("AppSettings:Issuer"),
					ValidateAudience = true,
					ValidAudience = configuration.GetValue<string>("AppSettings:Audience"),
					ClockSkew = TimeSpan.Zero
				}, out _);

			return true;
		}
		catch (SecurityTokenExpiredException) {
			return true;
		}
		catch {
			return false;
		}
	}
}
