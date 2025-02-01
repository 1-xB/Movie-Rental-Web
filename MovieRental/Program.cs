using MovieRental.Data;

namespace MovieRental
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            
            // Connect to the database
            const string connectionString = "Data Source=movie-rental.db"; // TODO - Update this connection string
            builder.Services.AddSqlite<DatabaseContext>(connectionString);
            
            var app = builder.Build();
            
            app.MapGet("/", () => "Hello World!");

            await app.MigrateDbAsync();

            app.Run();
        }
    }
}
