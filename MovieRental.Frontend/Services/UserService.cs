namespace MovieRental.Frontend.Services;

using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using Dtos;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

public class UserService(
	HttpClient http,
	NavigationManager navigationManager,
	ProtectedLocalStorage protectedLocalStorage,
	CustomAuthenticationStateProvider provider,
	AuthenticationStateProvider authenticationStateProvider) {
	public async Task<string> RegisterUserAsync(RegisterUserDto user) {
		var response = await http.PostAsJsonAsync("/api/auth/register", user);
		if (response.IsSuccessStatusCode) {
			navigationManager.NavigateTo("/login");
			return "Success";
		}

		var error = await response.Content.ReadAsStringAsync();
		return error.Contains("Username already exists") ? "Username already exists!" : "An error occurred: " + error;
	}

	public async Task<string?> RegisterAdministratorAsync(RegisterUserDto user) {
		var accessToken = await protectedLocalStorage.GetAsync<string>("accessToken");
		if (string.IsNullOrEmpty(accessToken.Value)) {
			await provider.MarkUserAsLoggedOut();
			navigationManager.NavigateTo("/login");
			return "Something went wrong!";
		}

		var request =
			new HttpRequestMessage(HttpMethod.Post, "api/auth/register-admin") { Content = JsonContent.Create(user) };
		request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken.Value);

		var response = await http.SendAsync(request);
		if (response.StatusCode == HttpStatusCode.OK) {
			navigationManager.NavigateTo("/admin-panel");
			return null;
		}

		var error = await response.Content.ReadAsStringAsync();
		return error.Contains("Username already exists") ? "Username already exists!" : "An error occurred: " + error;
	}

	public async Task<string> LoginUserAsync(LoginUserDto user) {
		var response = await http.PostAsJsonAsync("api/auth/login", user);
		if (response.StatusCode == HttpStatusCode.OK) {
			var tokenResponse = await response.Content.ReadFromJsonAsync<LoginResponseDto>();

			if (tokenResponse is null) {
				return "Something went wrong with getting tokens!";
			}

			await protectedLocalStorage.SetAsync("accessToken", tokenResponse.AccessToken);
			await protectedLocalStorage.SetAsync("refreshToken", tokenResponse.RefreshToken);
			var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(tokenResponse.AccessToken);
			var username = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
			if (username == null) {
				await protectedLocalStorage.SetAsync("username", user.Username);
			}
			else {
				await protectedLocalStorage.SetAsync("username", username);
			}
			provider.MarkUserAsAuthenticated(user.Username);
			navigationManager.NavigateTo("/", true);
			return "Success";
		}

		var error = await response.Content.ReadAsStringAsync();
		return error.Contains("Login or Password is wrong!")
			? "Login or Password is wrong!"
			: "An error occurred: " + error;
	}
}
