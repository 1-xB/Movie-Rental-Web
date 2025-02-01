using Microsoft.EntityFrameworkCore;
using MovieRental.Entity;

namespace MovieRental.Data;

public class DatabaseContext(DbContextOptions<DatabaseContext> options) : DbContext(options)
{
    public DbSet<Users> Users => Set<Users>();
    public DbSet<Movie> Movies => Set<Movie>();
    public DbSet<Genre> Genres => Set<Genre>();
    public DbSet<Rentals> Rentals => Set<Rentals>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Users>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Mail).IsRequired();
            entity.Property(e => e.Password).IsRequired().HasMaxLength(100);
            entity.HasData(
                new Users { Id = 1, Mail = "user1@example.com", Password = "password1" },
                new Users { Id = 2, Mail = "user2@example.com", Password = "password2" }
            );
        });

        modelBuilder.Entity<Movie>(movie =>
        {
            movie.HasKey(e => e.Id);
            movie.Property(e => e.Title).IsRequired().HasMaxLength(100);
            movie.Property(e => e.Description).IsRequired();
            movie.Property(e => e.GenreId);
            movie.Property(e => e.ReleaseYear).IsRequired();
            movie.HasOne(e => e.Genre)
                .WithMany(m => m.Movies)
                .HasForeignKey(k => k.GenreId);
            movie.HasData(
                new Movie { Id = 1, Title = "Movie1", Description = "Description1", GenreId = 1, ReleaseYear = new DateOnly(2020, 1, 1) },
                new Movie { Id = 2, Title = "Movie2", Description = "Description2", GenreId = 2, ReleaseYear = new DateOnly(2021, 1, 1) }
            );
        });

        modelBuilder.Entity<Genre>(genre =>
        {
            genre.HasKey(m => m.Id);
            genre.Property(m => m.Name).IsRequired();
            genre.HasData(
                new Genre { Id = 1, Name = "Genre1" },
                new Genre { Id = 2, Name = "Genre2" }
            );
        });

        modelBuilder.Entity<Rentals>(rental =>
        {
            rental.HasKey(r => r.Id);
            rental.Property(r => r.UserId).IsRequired();
            rental.Property(r => r.MovieId).IsRequired();
            rental.Property(r => r.RentalDate).IsRequired();
            rental.Property(r => r.ReturnDate).IsRequired();
            
            rental.HasOne(r => r.User)
                .WithMany(u => u.Rentals)
                .HasForeignKey(k => k.UserId);
            
            rental.HasOne(r => r.Movie)
                .WithMany(m => m.Rentals)
                .HasForeignKey(k => k.MovieId);
            
            rental.HasData(
                new Rentals { Id = 1, UserId = 1, MovieId = 1, RentalDate = new DateOnly(2022, 1, 1), ReturnDate = new DateOnly(2022, 1, 10) },
                new Rentals { Id = 2, UserId = 2, MovieId = 2, RentalDate = new DateOnly(2022, 2, 1), ReturnDate = new DateOnly(2022, 2, 10) }
            );
        });
    }
}