using MovieRental.Dtos;
using MovieRental.Entity;

namespace MovieRental.Services;

public interface IRentalService
{
    Task<Rentals?> RentMovie(RentMovieDto request, string username);
}