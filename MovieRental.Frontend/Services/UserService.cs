using System.Net;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MovieRental.Frontend.Dtos;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace MovieRental.Frontend.Services;

public class UserService(HttpClient http, IJSRuntime jsRuntime, NavigationManager navigationManager, ProtectedLocalStorage protectedLocalStorage, CustomAuthenticationStateProvider provider)
{
    public async Task<string> RegisterUserAsync(RegisterUserDto user)
    {
        var response = await http.PostAsJsonAsync("/api/auth/register", user);
        if (response.IsSuccessStatusCode)
        {
            navigationManager.NavigateTo("/login");
            return "Success";
        }
        
        var error = await response.Content.ReadAsStringAsync();
        return error.Contains("Username already exists") ? "Username already exists!" : "An error occurred: " + error;
    }

    public async Task<string> LoginUserAsync(LoginUserDto user)
    {
        var response = await http.PostAsJsonAsync("api/auth/login", user);
        if (response.StatusCode == HttpStatusCode.OK)
        {
            var tokenResponse = await response.Content.ReadFromJsonAsync<LoginResponseDto>();
            
            if (tokenResponse is null) return "Something went wrong with getting tokens!";
            
            await protectedLocalStorage.SetAsync("accessToken", tokenResponse.AccessToken);
            await protectedLocalStorage.SetAsync("refreshToken", tokenResponse.RefreshToken);
            await protectedLocalStorage.SetAsync("username", user.UsernameOrMail);
            provider.MarkUserAsAuthenticated(user.UsernameOrMail);
            navigationManager.NavigateTo("/", true);
            return "Success";

        }

        var error = await response.Content.ReadAsStringAsync();
        return error.Contains("Login or Password is wrong!")
            ? "Login or Password is wrong!"
            : "An error occurred: " + error;
    }

    
}