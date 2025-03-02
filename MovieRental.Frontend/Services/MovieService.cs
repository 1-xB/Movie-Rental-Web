namespace MovieRental.Frontend.Services;

using System.Net;
using System.Net.Http.Headers;
using Dtos;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

public class MovieService(
	ProtectedLocalStorage protectedLocalStorage,
	NavigationManager navigationManager,
	CustomAuthenticationStateProvider authenticationStateProvider) {
	public async Task<bool> AddMovieAsync(HttpClient http, AddMovieDto? movie) {
		var accessToken = await protectedLocalStorage.GetAsync<string>("accessToken");
		if (string.IsNullOrEmpty(accessToken.Value)) {
			await authenticationStateProvider.MarkUserAsLoggedOut();
			navigationManager.NavigateTo("/login");
			return false;
		}

		var request = new HttpRequestMessage(HttpMethod.Post, "/api/movie") { Content = JsonContent.Create(movie) };
		request.Headers.Authorization =
			new AuthenticationHeaderValue("Bearer", accessToken.Value);

		var response = await http.SendAsync(request);
		return response.StatusCode == HttpStatusCode.OK;
	}

	public async Task<bool> DeleteMovieAsync(HttpClient http, int id) {
		var accessToken = await protectedLocalStorage.GetAsync<string>("accessToken");
		if (string.IsNullOrEmpty(accessToken.Value)) {
			return false;
		}

		var request = new HttpRequestMessage(HttpMethod.Delete, $"/api/movie/{id}");
		request.Headers.Authorization =
			new AuthenticationHeaderValue("Bearer", accessToken.Value);

		var response = await http.SendAsync(request);

		return response.StatusCode == HttpStatusCode.NoContent;
	}
}
