namespace MovieRental.Frontend.Dtos;

using System.ComponentModel.DataAnnotations;

public class AddUserDto {
	[Required] [MinLength(4)] public required string? Username { get; set; }
	[Required] [EmailAddress] public required string? Mail { get; set; }
	[Required] [MinLength(6)] public required string Password { get; set; }
}
