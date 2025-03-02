namespace MovieRental.Services;

using Dtos;
using Entity;

public interface IRentalService {
	Task<Rentals?> RentMovie(RentMovieDto request, string id);
	Task<List<Rentals>?> GetRentalsByUsername(string username);
	Task<Rentals?> ReturnMovie(int id, string userId);
}
