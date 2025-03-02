namespace MovieRental.Frontend.Dtos;

using System.ComponentModel.DataAnnotations;

public class LoginUserDto {
	[Required] public required string UsernameOrMail { get; set; }
	[Required] public required string Password { get; set; }
}
