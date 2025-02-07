namespace MovieRental.Frontend.Dtos;

public class RegisterUserDto
{
    public required string Username { get; set; }
    public required string Mail { get; set; }
    public required string Password { get; set; }
}