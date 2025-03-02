namespace MovieRental.Dtos;

public class UserRegisterDto {
	public required string Username { get; set; }
	public required string Mail { get; set; }
	public required string Password { get; set; }
}
