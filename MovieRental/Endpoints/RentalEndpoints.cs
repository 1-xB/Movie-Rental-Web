using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using MovieRental.Dtos;
using MovieRental.Services;

namespace MovieRental.Endpoints;

public static class RentalEndpoints
{
    public static RouteGroupBuilder MapRentalRoutes(this WebApplication app)
    {
        var group = app.MapGroup("api/rental");

        group.MapPost("/rent-movie", [Authorize] async (HttpContext httpContext, IRentalService rentalService, RentMovieDto request) =>
        {
            var username = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(username))
            {
                return Results.Unauthorized();
            }
            
            var rental = await rentalService.RentMovie(request, username);
            return rental is null ? Results.BadRequest("Invalid request!") : Results.Ok(rental);
        });
        
        return group;
    }
}