using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MovieRental.Data;
using MovieRental.Endpoints;
using MovieRental.Services;
using Scalar.AspNetCore;

namespace MovieRental
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            
            builder.Services.AddOpenApi();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MovieRental API", Version = "v1" });
            });
            
            builder.Services.AddSqlite<DatabaseContext>(builder.Configuration.GetConnectionString("DefaultConnection"));

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = builder.Configuration["AppSettings:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["AppSettings:Audience"],
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(builder.Configuration["AppSettings:Token"]!)
                        ),
                    ValidateIssuerSigningKey = true
                    
                };
            });

            builder.Services.AddAuthorization();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IRentalService, RentalService>();
            
            var app = builder.Build();

            app.MapAuthRoutes();
            app.MapMovieRoutes();
            app.MapRentalRoutes();
            
            // Użyj OpenAPI
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "MovieRental API v1");
                });
                app.MapOpenApi();
                app.MapScalarApiReference();
            }

            await app.MigrateDbAsync();
            
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            

            await app.RunAsync();
        }
    }
}