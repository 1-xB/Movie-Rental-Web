namespace MovieRental.Frontend.Dtos;

using System.ComponentModel.DataAnnotations;

public class AddMovieDto {
	[Required]
	[MinLength(2, ErrorMessage = "The title must have at least 2 characters!")]
	[MaxLength(100, ErrorMessage = "The title must not be more than 100 characters long!")]
	public string Title { get; set; }

	[Required] public string Description { get; set; }

	[Required]
	[Range(1.00, 500, ErrorMessage = "Price must be greater than 1 $")]
	public double Price { get; set; }

	[Required] public int GenreId { get; set; }

	[Required]
	[Range(typeof(DateOnly), "1900-01-01", "2050-12-31", ErrorMessage = "Date out of range.")]
	public DateOnly ReleaseYear { get; set; }

	[Required]
	[Range(1, double.MaxValue, ErrorMessage = "Enter the number of copies")]
	public int TotalCopies { get; set; }

	public int AvailableCopies { get; set; }
	public string? ImageBase64 { get; set; }
	public string? ImageType { get; set; }
	public string? ImageName { get; set; }
}
