namespace MovieRental.Frontend.Models;

public class MovieModel {
	public int Id { get; set; }
	public string Title { get; set; }
	public string Description { get; set; }
	public double Price { get; set; }
	public int GenreId { get; set; }
	public DateOnly ReleaseYear { get; set; }

	public int TotalCopies { get; set; }
	public int AvailableCopies { get; set; }
	public string ImageUrl { get; set; }
}
