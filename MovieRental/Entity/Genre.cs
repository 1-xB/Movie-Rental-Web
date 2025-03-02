namespace MovieRental.Entity;

using System.Text.Json.Serialization;

public class Genre {
	public int Id { get; set; }
	public required string Name { get; set; }
	[JsonIgnore] public ICollection<Movie> Movies { get; set; } = new List<Movie>();
}
