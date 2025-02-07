using System.Net;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MovieRental.Frontend.Dtos;
using System.Net.Http.Json;

namespace MovieRental.Frontend.Services;

public class UserService(HttpClient http, IJSRuntime jsRuntime, NavigationManager navigationManager)
{
    public async Task<string> RegisterUserAsync(RegisterUserDto user)
    {
        var response = await http.PostAsJsonAsync("/api/auth/register", user);
        if (response.IsSuccessStatusCode)
        {
            navigationManager.NavigateTo("/login");
            Console.WriteLine(4);
            return "Success";
        }
        
        var error = await response.Content.ReadAsStringAsync();
        return error.Contains("Username already exists") ? "Username already exists!" : "An error occurred: " + error;
    }

    public async Task<string> LoginUserAsync(LoginUserDto user)
    {
        var response = await http.PostAsJsonAsync("api/auth/login", user);
        Console.WriteLine(response.StatusCode);
        if (response.StatusCode == HttpStatusCode.OK)
        {
            var tokenResponse = await response.Content.ReadFromJsonAsync<LoginResponseDto>();
            
            if (tokenResponse is null) return "Something went wrong with getting tokens!";
            
            await jsRuntime.InvokeVoidAsync("localStorage.setItem", "accessToken", tokenResponse.AccessToken);
            await jsRuntime.InvokeVoidAsync("localStorage.setItem", "refreshToken", tokenResponse.RefreshToken);
            await jsRuntime.InvokeVoidAsync("localStorage.setItem", "Id", tokenResponse.UserId);
            navigationManager.NavigateTo("/");
            return "Success!";

        }

        var error = await response.Content.ReadAsStringAsync();
        return error.Contains("Login or Password is wrong!")
            ? "Login or Password is wrong!"
            : "An error occurred: " + error;
    }
}