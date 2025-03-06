namespace MovieRental.Frontend.Services;

using System.Net.Http.Headers;
using Dtos;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Models;

public class GenreMethodsService(ProtectedLocalStorage protectedLocalStorage) {
	public async Task<string?> AddGenreAsync(HttpClient http, AddGenreDto? genre) {
		var accessToken = await protectedLocalStorage.GetAsync<string>("accessToken");
		if (accessToken.Value is null) {
			return "Access token is null";
		}
		var request = new HttpRequestMessage(HttpMethod.Post, "api/movie/genres") {
			Content = JsonContent.Create(genre)
		};
		request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken.Value);
		var response = await http.SendAsync(request);
		if (response.IsSuccessStatusCode) {
			return "Genre added successfully";
		}
		return await response.Content.ReadAsStringAsync();
	}

	public async Task<string?> UpdateGenreAsync(HttpClient http, GenreModel genre) {
		var accessToken = await protectedLocalStorage.GetAsync<string>("accessToken");
		if (accessToken.Value is null) {
			return "Access token is null";
		}

		var request = new HttpRequestMessage(HttpMethod.Put, $"api/movie/genres/{genre.Id}") {
			Content = JsonContent.Create(genre)
		};
		request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken.Value);
		var response = await http.SendAsync(request);
		if (response.IsSuccessStatusCode) {
			return "Genre updated successfully";
		}

		return await response.Content.ReadAsStringAsync();
	}
}
