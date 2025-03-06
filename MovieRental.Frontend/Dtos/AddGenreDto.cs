namespace MovieRental.Frontend.Dtos;

using System.ComponentModel.DataAnnotations;

public class AddGenreDto {
	[Required, MinLength(2), MaxLength(50)]public required string Name { get; set; }
}
