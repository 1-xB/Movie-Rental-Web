using Auth_course.Entity.Models;
using Microsoft.AspNetCore.Authorization;
using MovieRental.Dtos;
using MovieRental.Services;

namespace MovieRental.Endpoints;

public static class AuthEndpoints
{
    public static RouteGroupBuilder MapAuthRoutes(this WebApplication app)
    {
        var group = app.MapGroup("api/auth");

        group.MapPost("/register", async (IAuthService authService, UserRegisterDto request) =>
        {
            var user = await authService.RegisterAsync(request);
            return user is null ? Results.BadRequest("Username already exists!") : Results.Ok(user);
        });
        
        group.MapPost("/login", async (IAuthService authService, UserLoginDto request) =>
        {
            var response = await authService.LoginAsync(request);
            return response is null ? Results.BadRequest("Login or Password is wrong!") : Results.Ok(response);
        });

        group.MapGet("/", [Authorize](HttpContext httpContext) => Results.Ok("You are authenticated!"));
        group.MapGet("/admin-only", [Authorize(Roles = "Admin")](HttpContext httpContext) => Results.Ok("Hi admin!"));
        
        group.MapPost("/refresh-tokens", async (IAuthService authService, RefreshTokenRequestDto request, HttpContext http) =>
        {
            var authHeader = http.Request.Headers["Authorization"].ToString();
            Console.WriteLine(authHeader);
            if (string.IsNullOrWhiteSpace(authHeader) || !authHeader.StartsWith("Bearer "))
            {
                return Results.BadRequest("Invalid or missing access token!");
            }
            
            var accessToken = authHeader.Split(" ")[1];
            var response = await authService.RefreshTokensAsync(request, accessToken);
            return response is null ? Results.BadRequest("Invalid request!") : Results.Ok(response);
        });
        
        group.MapPost("/register-admin", [Authorize(Roles = "Admin")] async (HttpContext httpContext,IAuthService authService, UserRegisterDto request) =>
        {
            var accessToken = httpContext.Request.Headers.Authorization.ToString().Replace("Bearer ", "");
            if (!authService.IsAccessTokenValid(accessToken))
            {
                return Results.Unauthorized();
            }
            var user = await authService.RegisterAdminAsync(request);
            return user is null ? Results.BadRequest("Username already exists or invalid data!") : Results.Ok(user);
        });
        
        return group;
    }
}