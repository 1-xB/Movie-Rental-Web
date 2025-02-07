namespace MovieRental.Frontend.Dtos;

public class LoginUserDto
{
    public required string UsernameOrMail { get; set; }
    public required string Password { get; set; }
}