using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using MovieRental.Dtos;
using MovieRental.Entity;
using MovieRental.Services;

namespace MovieRental.Endpoints;

public static class RentalEndpoints
{
    public static RouteGroupBuilder MapRentalRoutes(this WebApplication app)
    {
        var group = app.MapGroup("api/rental");

        group.MapPost("/rent-movie", [Authorize] async (HttpContext httpContext, IRentalService rentalService, RentMovieDto request, IAuthService authService) =>
        {
            var accessToken = httpContext.Request.Headers.Authorization.ToString().Replace("Bearer ", "");
            if (!authService.IsAccessTokenValid(accessToken))
            {
                return Results.Unauthorized();
            }
            var id = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(id))
            {
                return Results.Unauthorized();
            }
            
            var rental = await rentalService.RentMovie(request, id);
            return rental is null ? Results.BadRequest("Invalid request!") : Results.Ok(rental);
        });

        group.MapGet("/rent-movie", [Authorize] async (HttpContext httpContext, IRentalService rentalService, IAuthService authService) =>
        {
            var accessToken = httpContext.Request.Headers.Authorization.ToString().Replace("Bearer ", "");
            if (!authService.IsAccessTokenValid(accessToken))
            {
                return Results.Unauthorized();
            }
            var username =  httpContext.User.FindFirst(ClaimTypes.Name)?.Value;
            if (username == null) return Results.Unauthorized();
            
            List<Rentals>? rentals = await rentalService.GetRentalsByUsername(username);
            if (rentals != null && rentals.Count <= 0)
            {
                return Results.NoContent();
            }
            return Results.Ok(rentals);
        });

        group.MapPost("/rent-movie/return", [Authorize] async (HttpContext httpContext, IRentalService rentalService, IAuthService authService, int id) =>
        {
            var accessToken = httpContext.Request.Headers.Authorization.ToString().Replace("Bearer ", "");
            if (!authService.IsAccessTokenValid(accessToken))
            {
                return Results.Unauthorized();
            }

            var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(userId))
            {
                return Results.Unauthorized();
            }

            var rental = await rentalService.ReturnMovie(id, userId);
            return rental is null ? Results.BadRequest() : Results.Ok(rental);
        });
        
        return group;
    }
}