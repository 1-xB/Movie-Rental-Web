namespace MovieRental.Dtos;

public class UpdateMovieDto {
	public required string Title { get; set; }
	public required string Description { get; set; }
	public int GenreId { get; set; }
	public DateOnly ReleaseYear { get; set; }
	public int TotalCopies { get; set; }
	public int AvailableCopies { get; set; }
}
