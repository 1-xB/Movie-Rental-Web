namespace MovieRental.Entity;

public class Movie
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public double Price { get; set; }
    public int GenreId { get; set; }
    public Genre? Genre { get; set; }
    public DateOnly ReleaseYear { get; set; }

    public int TotalCopies { get; set; }
    public int AvailableCopies { get; set; }
    public ICollection<Rentals> Rentals { get; set; } = new List<Rentals>();

}