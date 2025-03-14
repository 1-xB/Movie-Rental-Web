namespace MovieRental.Frontend;

using Components;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Services;

public class Program {
	public static void Main(string[] args) {
		var builder = WebApplication.CreateBuilder(args);

		// Add services to the container.
		builder.Services.AddRazorComponents()
			.AddInteractiveServerComponents();
		// https://localhost:7242
		builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://movierentalbackend.azurewebsites.net") });
		//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7242") });

		builder.Services.AddScoped<UserService>();
		builder.Services.AddScoped<MovieService>();
		builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
		builder.Services.AddScoped<CustomAuthenticationStateProvider>();
		builder.Services.AddSingleton<GenreService>();
		builder.Services.AddScoped<GenreMethodsService>();

		builder.Services.AddAuthorizationCore(options => {
			options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
		});

		builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
			.AddCookie();

		var app = builder.Build();

		// Configure the HTTP request pipeline.
		if (!app.Environment.IsDevelopment()) {
			app.UseExceptionHandler("/Error");
			// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
			app.UseHsts();
		}

		app.UseHttpsRedirection();

		app.UseAntiforgery();

		app.MapStaticAssets();
		app.MapRazorComponents<App>()
			.AddInteractiveServerRenderMode();

		app.Run();
	}
}
