using System.ComponentModel.DataAnnotations;

namespace MovieRental.Frontend.Dtos;

public class LoginUserDto
{
    [Required] public required string UsernameOrMail { get; set; }
    [Required] public required string Password { get; set; }
}