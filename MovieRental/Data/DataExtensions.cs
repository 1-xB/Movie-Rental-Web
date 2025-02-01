using Microsoft.EntityFrameworkCore;

namespace MovieRental.Data;

public static class DataExtensions
{
    public static async Task MigrateDbAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope(); // Tworzy zakres, który reprezentuje zakres życia serwisu
        var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>(); // Pobiera serwis z kontenera serwisów
        await dbContext.Database.MigrateAsync();
    }
}