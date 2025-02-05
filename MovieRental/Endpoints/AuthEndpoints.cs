﻿using Auth_course.Entity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using MovieRental.Data;
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
            if (response is null)
            {
                return Results.BadRequest("Login or Password is wrong!");
            }

            return Results.Ok(response);
        });

        group.MapGet("/", [Authorize](HttpContext httpContext) =>
        {
            return Results.Ok("You are authenticated!");
        });
        
        
        return group;
    }
}