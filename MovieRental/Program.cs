namespace MovieRental;

using System.Text;
using Data;
using Endpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Services;

public class Program {
	public static async Task Main(string[] args) {
		var builder = WebApplication.CreateBuilder(args);



		// Dodaj usługi Swaggera
		builder.Services.AddEndpointsApiExplorer();
		builder.Services.AddSwaggerGen(c => {
			c.SwaggerDoc("v1", new OpenApiInfo { Title = "MovieRental API", Version = "v1" });
		});
		builder.Services.AddControllers(options =>
		{
			options.Filters.Add(new Microsoft.AspNetCore.Mvc.IgnoreAntiforgeryTokenAttribute());
		});


		//builder.Services.AddOpenApi();
		//builder.Services.AddEndpointsApiExplorer();
		//builder.Services.AddSwaggerGen(c => {
		//	c.SwaggerDoc("v1", new OpenApiInfo { Title = "MovieRental API", Version = "v1" });
		//});

		builder.Services.AddDbContext<DatabaseContext>(options =>
			options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


		builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => {
			options.TokenValidationParameters = new TokenValidationParameters {
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
		app.MapControllers();

		app.MapAuthRoutes();

		app.MapMovieRoutes();
		app.MapRentalRoutes();

		if (app.Environment.IsDevelopment()) {
			app.UseSwagger();
			app.UseSwaggerUI(c => {
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "MovieRental API v1");
			});
		}

		await app.MigrateDbAsync();
		app.UseStaticFiles();
		app.UseRouting();
		app.UseHttpsRedirection();
		app.UseAuthentication();
		app.UseAuthorization();



		await app.RunAsync();
	}
}

