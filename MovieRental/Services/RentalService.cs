using Microsoft.EntityFrameworkCore;
using MovieRental.Data;
using MovieRental.Dtos;
using MovieRental.Entity;

namespace MovieRental.Services;

public class RentalService(DatabaseContext context) : IRentalService
{
    public async Task<Rentals?> RentMovie(RentMovieDto request, string id)
    {
        var guidId = Guid.Parse(id);
        var user = await context.Users.FindAsync(guidId);
        if (user is null)
        {
            return null;
        }

        
        var movie = await context.Movies.FindAsync(request.MovieId);
        if (movie is null || movie.AvailableCopies <= 0)
        {
            return null;
        }

        movie.AvailableCopies--;

        var rental = new Rentals()
        {
            UserId = user.Id,
            MovieId = request.MovieId,
            RentalDate = DateOnly.FromDateTime(DateTime.UtcNow),
            ReturnDate = request.ReturnDate,
            Returned = false
        };
        
        await context.Rentals.AddAsync(rental);
        await context.SaveChangesAsync();

        return rental;
    }

    public async Task<List<Rentals>?> GetRentalsByUsername(string username)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.Username == username);
        if (user is null)
        {
            return null;
        }
        return await context.Rentals.Where(r => r.UserId == user.Id).ToListAsync();
    }
    
    
    
}