namespace MovieRental.Frontend.Dtos;

using System.ComponentModel.DataAnnotations;

public class RentMovieDto {
	public int MovieId { get; set; }
	public decimal TotalPrice { get; set; }

	[Required]
	[Range(typeof(DateOnly), "2023-01-01", "2050-12-31", ErrorMessage = "Date out of range.")]
	public DateOnly ReturnDate { get; set; }
}
