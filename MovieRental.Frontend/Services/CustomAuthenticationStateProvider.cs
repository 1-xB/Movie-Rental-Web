using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using MovieRental.Frontend.Dtos;

namespace MovieRental.Frontend.Services;

public class CustomAuthenticationStateProvider(
    ProtectedLocalStorage protectedLocalStorage,
    NavigationManager navigationManager,
    HttpClient http) : AuthenticationStateProvider
{
    private ClaimsPrincipal _currentUser = new(new ClaimsIdentity());

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        await InitializeAsync();
        return new AuthenticationState(_currentUser);
    }

    private async Task InitializeAsync()
    {
        var identity = new ClaimsIdentity();

        try
        {
            var usernameResult = await protectedLocalStorage.GetAsync<string>("username");
            var accessTokenResult = await protectedLocalStorage.GetAsync<string>("accessToken");


            if (accessTokenResult.Value != null && await IsTokenValid(accessTokenResult.Value))
            {
                if (usernameResult.Value != null)
                {
                    identity = new ClaimsIdentity(
                        new List<Claim> { new Claim(ClaimTypes.Name, usernameResult.Value) }.AsReadOnly(), "apiauth");
                }
            }
            else
            {
                if (await RefreshTokens())
                {
                    if (usernameResult.Value != null)
                    {
                        identity = new ClaimsIdentity(
                            new List<Claim> { new Claim(ClaimTypes.Name, usernameResult.Value) }.AsReadOnly(),
                            "apiauth");
                    }
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Bład : " + e.Message );
        }

        _currentUser = new ClaimsPrincipal(identity);
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_currentUser)));
    }

    public async Task<bool> RefreshTokens()
    {
        var refreshToken = await protectedLocalStorage.GetAsync<string>("refreshToken");
        if (string.IsNullOrEmpty(refreshToken.Value)) return false;

        var accessToken = await protectedLocalStorage.GetAsync<string>("accessToken");
        if (string.IsNullOrEmpty(accessToken.Value)) return false;

        var request = new HttpRequestMessage(HttpMethod.Post, "api/auth/refresh-tokens")
        {
            Content = JsonContent.Create(new RefreshTokenRequestDto { RefreshToken = refreshToken.Value })
        };
        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken.Value);

        var response = await http.SendAsync(request);
        if (response.StatusCode == HttpStatusCode.OK)
        {
            var tokenResponse = await response.Content.ReadFromJsonAsync<LoginResponseDto>();
            if (tokenResponse is null) return false;
            await protectedLocalStorage.SetAsync("accessToken", tokenResponse.AccessToken);
            await protectedLocalStorage.SetAsync("refreshToken", tokenResponse.RefreshToken);
            return true;
        }
        else
        {
            var error = await response.Content.ReadAsStringAsync();
            Console.WriteLine(error);
        }

        return false;
    }

    public Task<bool> IsTokenValid(string token)
    {
        var jwtToken = new JwtSecurityTokenHandler().ReadToken(token) as JwtSecurityToken;
        return Task.FromResult(jwtToken != null && jwtToken.ValidTo > DateTime.UtcNow);
    }

    public void MarkUserAsAuthenticated(string username)
    {
        var identity = new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Name, username)
        }, "apiauth");

        _currentUser = new ClaimsPrincipal(identity);
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_currentUser)));
    }

    public async Task MarkUserAsLoggedOut()
    {
        _currentUser = new ClaimsPrincipal(new ClaimsIdentity());
        await protectedLocalStorage.DeleteAsync("username");
        await protectedLocalStorage.DeleteAsync("accessToken");
        await protectedLocalStorage.DeleteAsync("refreshToken");
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_currentUser)));
    }
    
    public async Task<string?> GetUsername()
    {
        return (await protectedLocalStorage.GetAsync<string>("username")).Value;
    }
}