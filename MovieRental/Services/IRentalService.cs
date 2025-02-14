using MovieRental.Dtos;
using MovieRental.Entity;

namespace MovieRental.Services;

public interface IRentalService
{
    Task<Rentals?> RentMovie(RentMovieDto request, string id);
    Task<List<Rentals>?> GetRentalsByUsername(string username);
    Task<Rentals?> ReturnMovie(int id);
}