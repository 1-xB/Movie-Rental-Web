namespace MovieRental.Frontend.Dtos;

using System.ComponentModel.DataAnnotations;

public class LoginUserDto {
	[Required] public required string Username { get; set; }
	[Required] public required string Password { get; set; }
}
