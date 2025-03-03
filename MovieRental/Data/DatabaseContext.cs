namespace MovieRental.Data;

using Entity;
using Microsoft.EntityFrameworkCore;

public class DatabaseContext(DbContextOptions<DatabaseContext> options) : DbContext(options) {
    public DbSet<Users> Users => Set<Users>();
    public DbSet<Movie> Movies => Set<Movie>();
    public DbSet<Genre> Genres => Set<Genre>();
    public DbSet<Rentals> Rentals => Set<Rentals>();

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.Entity<Users>(entity => {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                .IsRequired()
                .HasDefaultValueSql("NEWID()");
            entity.Property(e => e.Username).IsRequired().HasMaxLength(30);
            entity.Property(e => e.Mail).IsRequired().HasMaxLength(255);
            entity.Property(e => e.PasswordHash).IsRequired().HasMaxLength(255);
            entity.Property(e => e.RefreshToken);
            entity.Property(e => e.RefreshTokenExpiryTime);
        });

        modelBuilder.Entity<Movie>(movie => {
            movie.HasKey(e => e.Id);
            movie.Property(e => e.Id).ValueGeneratedOnAdd();
            movie.Property(e => e.Title).IsRequired().HasMaxLength(100);
            movie.Property(e => e.Description).IsRequired();
            movie.Property(e => e.GenreId);
            movie.Property(e => e.ReleaseYear)
                .HasConversion(v => v.ToDateTime(TimeOnly.MinValue), v => DateOnly.FromDateTime(v))
                .IsRequired();
            movie.HasOne(e => e.Genre)
                .WithMany(m => m.Movies)
                .HasForeignKey(k => k.GenreId);
        });

        modelBuilder.Entity<Genre>(genre => {
            genre.HasKey(m => m.Id);
            genre.Property(m => m.Id).ValueGeneratedOnAdd();
            genre.Property(m => m.Name).IsRequired();
        });

        modelBuilder.Entity<Rentals>(rental => {
            rental.HasKey(r => r.Id);
            rental.Property(r => r.Id).ValueGeneratedOnAdd();
            rental.Property(r => r.UserId).IsRequired();
            rental.Property(r => r.MovieId).IsRequired();
            rental.Property(r => r.RentalDate)
                .HasConversion(v => v.ToDateTime(TimeOnly.MinValue), v => DateOnly.FromDateTime(v))
                .IsRequired();
            rental.Property(r => r.ReturnDate)
                .HasConversion(v => v.ToDateTime(TimeOnly.MinValue), v => DateOnly.FromDateTime(v))
                .IsRequired();
            rental.HasOne(r => r.User)
                .WithMany(u => u.Rentals)
                .HasForeignKey(k => k.UserId);
            rental.HasOne(r => r.Movie)
                .WithMany(m => m.Rentals)
                .HasForeignKey(k => k.MovieId);
        });
    }
}
