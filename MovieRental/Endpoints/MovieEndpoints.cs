using Microsoft.EntityFrameworkCore;
using MovieRental.Data;
using MovieRental.Dtos;
using Microsoft.AspNetCore.Authorization;
using MovieRental.Entity;
using MovieRental.Services;

namespace MovieRental.Endpoints;

public static class MovieEndpoints
{
    public static RouteGroupBuilder MapMovieRoutes(this WebApplication app)
    {
        var group = app.MapGroup("api/movie");

        group.MapGet("/available", async (DatabaseContext context) =>
        {
            var availableMovies = await context.Movies.Where(m => m.AvailableCopies > 0).ToListAsync();
            return Results.Ok(availableMovies);
        });
        
        group.MapGet("/", async (DatabaseContext context) => Results.Ok(await context.Movies.ToListAsync()));

        group.MapGet("/{id:int}", async (int id, DatabaseContext context) =>
        {
            var movie = await context.Movies.FindAsync(id);
            return movie is null ? Results.BadRequest("Movie with this id does not exist") : Results.Ok(movie);
        });

        group.MapGet("/search", async (string? title, DatabaseContext context) =>
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                return Results.BadRequest("Parameter 'title' is required.");
            }
            
            var movie = await context.Movies.FirstOrDefaultAsync(m => m.Title == title.Replace("-", " "));
            Console.WriteLine(movie);
            return movie is null ? Results.NotFound("Movie not found ") : Results.Ok(movie);
        });

        group.MapPost("/", [Authorize(Roles = "Admin")] async (HttpContext httpContext, IAuthService authService ,AddMovieDto newMovie, DatabaseContext context) =>
        {
            var accessToken = httpContext.Request.Headers.Authorization.ToString().Replace("Bearer ", "");
            if (!authService.IsAccessTokenValid(accessToken))
            {
                return Results.Unauthorized();
            }
            var movie = new Movie
            {
                Title = newMovie.Title,
                Description = newMovie.Description,
                GenreId = newMovie.GenreId,
                ReleaseYear = newMovie.ReleaseYear,
                TotalCopies = newMovie.TotalCopies,
                AvailableCopies = newMovie.TotalCopies
            };
            await context.Movies.AddAsync(movie);
            await context.SaveChangesAsync();
            
            return Results.Ok(movie);
        });
        
        group.MapPut("/{id:int}", [Authorize(Roles = "Admin")] async (HttpContext httpContext, IAuthService authService ,int id, UpdateMovieDto newMovie, DatabaseContext context) =>
        {
            var movie = await context.Movies.FindAsync(id);
            if (movie is null)
            {
                return Results.BadRequest("Movie with this id does not exist");
            }

            movie.Title = newMovie.Title;
            movie.Description = newMovie.Description;
            movie.GenreId = newMovie.GenreId;
            movie.ReleaseYear = newMovie.ReleaseYear;
            movie.TotalCopies = newMovie.TotalCopies;
            movie.AvailableCopies = newMovie.TotalCopies;

            await context.SaveChangesAsync();
            
            return Results.Ok(movie);
        });

        group.MapDelete("/{id:int}", [Authorize(Roles = "Admin")] async (HttpContext httpContext, IAuthService authService , int id, DatabaseContext context) =>
        {
            var movie = await context.Movies.FindAsync(id);
            if (movie is null)
            {
                return Results.BadRequest("Movie with this id does not exist");
            }
            context.Movies.Remove(movie);
            await context.SaveChangesAsync();
            return Results.NoContent();
        });

        group.MapGet("/genres", async (DatabaseContext context) => await context.Genres.ToListAsync());
        
        group.MapPost("/genres", [Authorize(Roles = "Admin")] async (AddGenreDto newGenre, DatabaseContext context) =>
        {
            var genre = new Genre
            {
                Name = newGenre.Name
            };
            await context.Genres.AddAsync(genre);
            await context.SaveChangesAsync();
            
            return Results.Ok(genre);
        });
        
        group.MapDelete("/genres/{id:int}", [Authorize(Roles = "Admin")] async (HttpContext httpContext, IAuthService authService ,int id, DatabaseContext context) =>
        {
            var genre = await context.Genres.FindAsync(id);
            if (genre is null)
            {
                return Results.BadRequest("Genre with this id does not exist");
            }
            context.Genres.Remove(genre);
            await context.SaveChangesAsync();
            return Results.NoContent();
        });
        
        group.MapPut("/genres/{id:int}", [Authorize (Roles = "Admin")] async (HttpContext httpContext, IAuthService authService , int id, UpdateGenreDto newGenre, DatabaseContext context) =>
        {
            var genre = await context.Genres.FindAsync(id);
            if (genre is null)
            {
                return Results.BadRequest("Genre with this id does not exist");
            }

            genre.Name = newGenre.Name;

            await context.SaveChangesAsync();
            
            return Results.Ok(genre);
        });

        return group;
    }
}