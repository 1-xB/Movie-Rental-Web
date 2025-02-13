using System.Text.Json.Serialization;

namespace MovieRental.Entity;

public class Genre
{
    public int Id { get; set; }
    public required string Name { get; set; }
    [JsonIgnore] public ICollection<Movie> Movies { get; set; } = new List<Movie>();
}