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
        var accessTokenResult = await protectedLocalStorage.GetAsync<string>("accessToken");
        var usernameResult = await protectedLocalStorage.GetAsync<string>("username");

        if (accessTokenResult.Value is null || usernameResult.Value is null)
        {
            await MarkUserAsLoggedOut();
            return;
        }

        if (accessTokenResult.Value != null && await IsTokenValid(accessTokenResult.Value))
        {
            var jwtToken = new JwtSecurityTokenHandler().ReadToken(accessTokenResult.Value) as JwtSecurityToken;
            var roleClaim = jwtToken?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            if (usernameResult.Value != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, usernameResult.Value)
                };

                if (roleClaim != null)
                {
                    claims.Add(new Claim(ClaimTypes.Role, roleClaim));
                }

                identity = new ClaimsIdentity(claims, "apiauth");
            }
            else
            {
                await MarkUserAsLoggedOut();
            }
        }
        else
        {
            if (await RefreshTokens())
            {
                var jwtToken = new JwtSecurityTokenHandler().ReadToken(accessTokenResult.Value) as JwtSecurityToken;
                var roleClaim = jwtToken?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

                if (usernameResult.Value != null)
                {
                    var claims = new List<Claim>
                    {
                        new(ClaimTypes.Name, usernameResult.Value)
                    };

                    if (roleClaim != null)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, roleClaim));
                    }

                    identity = new ClaimsIdentity(claims, "apiauth");
                }
            }
            else
            {
                await MarkUserAsLoggedOut();
            }
        }
    }
    catch
    {
        await MarkUserAsLoggedOut();
    }

    _currentUser = new ClaimsPrincipal(identity);
    Console.WriteLine("User authenticated: " + GetUserRole() + _currentUser.IsInRole("Admin") + _currentUser.IsInRole("Client"));
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
        try
        {
            await protectedLocalStorage.DeleteAsync("username");
            await protectedLocalStorage.DeleteAsync("accessToken");
            await protectedLocalStorage.DeleteAsync("refreshToken");
        }
        catch
        {
            // ignored
        }
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_currentUser)));
    }
    
    public string? GetUserRole() => _currentUser.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
}