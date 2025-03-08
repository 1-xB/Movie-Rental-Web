namespace MovieRental.Dtos;

public class AddMovieDto {
	public required string Title { get; set; }
	public required string Description { get; set; }
	public double Price { get; set; }
	public int GenreId { get; set; }
	public DateOnly ReleaseYear { get; set; }

	public int TotalCopies { get; set; }
	public int AvailableCopies { get; set; }

	public string? ImageBase64 { get; set; }
	public string? ImageType { get; set; }
	public string? ImageName { get; set; }
}
