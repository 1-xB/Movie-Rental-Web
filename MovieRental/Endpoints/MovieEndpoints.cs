namespace MovieRental.Endpoints;

using Data;
using Dtos;
using Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services;

public static class MovieEndpoints {
	public static RouteGroupBuilder MapMovieRoutes(this WebApplication app) {
		var group = app.MapGroup("api/movie");

		group.MapGet("/available", async (DatabaseContext context) => {
			var availableMovies = await context.Movies.Where(m => m.AvailableCopies > 0).ToListAsync();
			return Results.Ok(availableMovies);
		});

		group.MapGet("/", async (DatabaseContext context) => Results.Ok(await context.Movies.ToListAsync()));

		group.MapGet("/{id:int}", async (int id, DatabaseContext context) => {
			var movie = await context.Movies.FindAsync(id);
			return movie is null ? Results.BadRequest("Movie with this id does not exist") : Results.Ok(movie);
		});

		group.MapGet("/search", async (string? title, DatabaseContext context) => {
			if (string.IsNullOrWhiteSpace(title)) {
				return Results.BadRequest("Parameter 'title' is required.");
			}

			var movie = await context.Movies.FirstOrDefaultAsync(m => m.Title == title.Replace("-", " "));
			Console.WriteLine(movie);
			return movie is null ? Results.NotFound("Movie not found ") : Results.Ok(movie);
		});


		group.MapPost("/", [Authorize(Roles = "Admin")] [IgnoreAntiforgeryToken] async (HttpContext httpContext,
		    IAuthService authService, [FromBody] AddMovieDto newMovie, DatabaseContext context) =>
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
		        AvailableCopies = newMovie.TotalCopies,
		        Price = newMovie.Price
		    };

		    if (!string.IsNullOrEmpty(newMovie.ImageBase64))
		    {
		        string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

		        if (!Directory.Exists(uploadsFolder))
		        {
		            Directory.CreateDirectory(uploadsFolder);
		        }

		        string fileName = Guid.NewGuid() + Path.GetExtension(newMovie.ImageName);
		        string filePath = Path.Combine(uploadsFolder, fileName);

		        try
		        {
		            var base64Data = newMovie.ImageBase64.Split(',').Last();
		            byte[] imageBytes = Convert.FromBase64String(base64Data);
		            await File.WriteAllBytesAsync(filePath, imageBytes);

		            movie.ImageUrl = $"/images/{fileName}";
		        }
		        catch (Exception ex)
		        {
		            // ignore
		        }
		    }

		    await context.Movies.AddAsync(movie);
		    await context.SaveChangesAsync();

		    return Results.Ok(movie);
		});
		group.MapPut("/{id:int}", [Authorize(Roles = "Admin")]
			async (HttpContext httpContext, IAuthService authService, int id, UpdateMovieDto newMovie,
				DatabaseContext context, IWebHostEnvironment env) => {
				var movie = await context.Movies.FindAsync(id);
				if (movie is null) {
					return Results.BadRequest("Movie with this id does not exist");
				}

				movie.Title = newMovie.Title;
				movie.Description = newMovie.Description;
				movie.GenreId = newMovie.GenreId;
				movie.ReleaseYear = newMovie.ReleaseYear;
				movie.TotalCopies = newMovie.TotalCopies;
				movie.AvailableCopies = newMovie.TotalCopies;
				movie.Price = newMovie.Price;

				if (!string.IsNullOrEmpty(newMovie.ImageBase64))
				{
					string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

					if (!Directory.Exists(uploadsFolder))
					{
						Directory.CreateDirectory(uploadsFolder);
					}

					string fileName = Guid.NewGuid() + Path.GetExtension(newMovie.ImageName);
					string filePath = Path.Combine(uploadsFolder, fileName);
					string oldFilePath = Path.Combine(uploadsFolder, movie.ImageUrl.Replace("/images/", ""));
					try
					{
						var base64Data = newMovie.ImageBase64.Split(',').Last();
						byte[] imageBytes = Convert.FromBase64String(base64Data);
						await File.WriteAllBytesAsync(filePath, imageBytes);

						movie.ImageUrl = $"/images/{fileName}";

						if (File.Exists(oldFilePath))
						{
							File.Delete(oldFilePath);
						}
					}
					catch (Exception ex)
					{
						// ignore
					}
				}


				await context.SaveChangesAsync();

				return Results.Ok(movie);
			});

		group.MapDelete("/{id:int}", [Authorize(Roles = "Admin")]
			async (HttpContext httpContext, IAuthService authService, int id, DatabaseContext context) => {
				var movie = await context.Movies.FindAsync(id);
				if (movie is null) {
					return Results.BadRequest("Movie with this id does not exist");
				}
				string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
				string oldFilePath = Path.Combine(uploadsFolder, movie.ImageUrl.Replace("/images/", ""));

				if (File.Exists(oldFilePath))
				{
					File.Delete(oldFilePath);
				}

				context.Movies.Remove(movie);
				await context.SaveChangesAsync();
				return Results.NoContent();
			});

		group.MapGet("/genres", async (DatabaseContext context) => await context.Genres.ToListAsync());

		group.MapPost("/genres", [Authorize(Roles = "Admin")] async (AddGenreDto newGenre, DatabaseContext context) => {
			var genre = new Genre { Name = newGenre.Name };
			await context.Genres.AddAsync(genre);
			await context.SaveChangesAsync();

			return Results.Ok(genre);
		});

		group.MapDelete("/genres/{id:int}", [Authorize(Roles = "Admin")]
			async (HttpContext httpContext, IAuthService authService, int id, DatabaseContext context) => {
				var genre = await context.Genres.FindAsync(id);
				if (genre is null) {
					return Results.BadRequest("Genre with this id does not exist");
				}

				context.Genres.Remove(genre);
				await context.SaveChangesAsync();
				return Results.NoContent();
			});

		group.MapPut("/genres/{id:int}", [Authorize(Roles = "Admin")]
			async (HttpContext httpContext, IAuthService authService, int id, UpdateGenreDto newGenre,
				DatabaseContext context) => {
				var genre = await context.Genres.FindAsync(id);
				if (genre is null) {
					return Results.BadRequest("Genre with this id does not exist");
				}

				genre.Name = newGenre.Name;


				await context.SaveChangesAsync();

				return Results.Ok(genre);
			});

		return group;
	}
}
