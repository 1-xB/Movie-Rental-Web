using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using MovieRental.Frontend.Dtos;

namespace MovieRental.Frontend.Services;

public class MovieService(ProtectedLocalStorage protectedLocalStorage, NavigationManager navigationManager, CustomAuthenticationStateProvider authenticationStateProvider)
{
    public async Task<bool> TaskAddMovieAsync(HttpClient http, AddMovieDto? movie)
    {
        var accessToken = await protectedLocalStorage.GetAsync<string>("accessToken");
        if (string.IsNullOrEmpty(accessToken.Value))
        {
            await authenticationStateProvider.MarkUserAsLoggedOut();
            navigationManager.NavigateTo("/login");
            return false;
        }

        var request = new HttpRequestMessage(HttpMethod.Post, "/api/movie")
        {
            Content = JsonContent.Create(movie)
        };
        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken.Value);
        
        var response = await http.SendAsync(request);
        if (response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            navigationManager.NavigateTo("/movies");
            return true;
        }
        return false;

    }
}