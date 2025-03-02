namespace MovieRental.Entity;

using System.Text.Json.Serialization;

public class Users {
	public Guid Id { get; set; }
	public string Username { get; set; }
	public string Mail { get; set; }
	public string PasswordHash { get; set; }
	public string? RefreshToken { get; set; }
	public DateTime? RefreshTokenExpiryTime { get; set; }
	public string Role { get; set; }
	[JsonIgnore] public ICollection<Rentals> Rentals { get; set; } = new List<Rentals>();
}
