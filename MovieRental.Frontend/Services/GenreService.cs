using MovieRental.Frontend.Models;

namespace MovieRental.Frontend.Services;

public class GenreService()
{
    private List<GenreModel>? _genres { get; set; }

    public async Task<List<GenreModel>?> GetGenresAsync(HttpClient http)
    {
        if (_genres is null)
        {
            _genres = await http.GetFromJsonAsync<List<GenreModel>>("api/movie/genres");
        }
        return _genres;
    }
}