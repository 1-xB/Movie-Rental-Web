using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using MovieRental.Frontend.Dtos;

namespace MovieRental.Frontend.Services;

public class CustomAuthenticationStateProvider(ProtectedLocalStorage protectedLocalStorage, NavigationManager navigationManager, HttpClient http) : AuthenticationStateProvider
{
    private ClaimsPrincipal _currentUser = new(new ClaimsIdentity());
    private bool _isInitialized = false;

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        if (!_isInitialized)
        {
            await InitializeAsync();
        }

        return new AuthenticationState(_currentUser);
    }

    public async Task InitializeAsync()
    {
        if (_isInitialized) return;

        var identity = new ClaimsIdentity();

        try
        {
            var accessTokenResult = await protectedLocalStorage.GetAsync<string>("accessToken");

            if (await IsTokenValid(accessTokenResult.Value))
            {
                var usernameResult = await protectedLocalStorage.GetAsync<string>("username");
                identity = new ClaimsIdentity([
                    new Claim(ClaimTypes.Name, usernameResult.Value)
                ], "apiauth");

                Console.WriteLine($"Przywrócono sesję użytkownika: {usernameResult.Value}");
            }
            else
            {
                if (await RefreshTokens())
                {
                    var usernameResult = await protectedLocalStorage.GetAsync<string>("username");
                    identity = new ClaimsIdentity(
                        new List<Claim> { new Claim(ClaimTypes.Name, usernameResult.Value) }.AsReadOnly(),
                        "apiauth");

                    Console.WriteLine($"Przywrócono sesję użytkownika: {usernameResult.Value}");
                }
                else
                {
                    Console.WriteLine("Nie udało się przywrócić sesji użytkownika");
                    navigationManager.NavigateTo("/login");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Błąd podczas ładowania sesji: {ex.Message}");
        }

        _currentUser = new ClaimsPrincipal(identity);
        _isInitialized = true;

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_currentUser)));
    }
    public async Task<bool> RefreshTokens()
    {
        var RefreshToken = await protectedLocalStorage.GetAsync<string>("refreshToken");
        if (string.IsNullOrEmpty(RefreshToken.ToString())) return false;
        var result = await http.PostAsJsonAsync("api/auth/refresh-tokens", new RefreshTokenRequestDto { RefreshToken = RefreshToken.ToString() });
        if (result.StatusCode == HttpStatusCode.OK)
        {
            var tokenResponse = await result.Content.ReadFromJsonAsync<LoginResponseDto>();
            if (tokenResponse is null) return false;
            await protectedLocalStorage.SetAsync("accessToken", tokenResponse.AccessToken);
            await protectedLocalStorage.SetAsync("refreshToken", tokenResponse.RefreshToken);
            return true;
        }
        return false;
    }
    
    private async Task<bool> IsTokenValid(string token)
    {
        var jwtToken = new JwtSecurityTokenHandler().ReadToken(token) as JwtSecurityToken;
        var username = jwtToken?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
        if (username != null) await protectedLocalStorage.SetAsync("username", username.ToString());
        return jwtToken != null && jwtToken.ValidTo > DateTime.UtcNow;
    }
    public void MarkUserAsAuthenticated(string username)
    {
        var identity = new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Name, username)
        }, "apiauth");

        _currentUser = new ClaimsPrincipal(identity);
        _isInitialized = true;
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_currentUser)));
    }

    public async Task MarkUserAsLoggedOut()
    {
        _currentUser = new ClaimsPrincipal(new ClaimsIdentity());
        _isInitialized = false;
        await protectedLocalStorage.DeleteAsync("username");
        await protectedLocalStorage.DeleteAsync("accessToken");
        await protectedLocalStorage.DeleteAsync("refreshToken");
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_currentUser)));
    }
}
