namespace MovieRental.Entity;

public class Users
{
    public int Id { get; set; }
    public string Mail { get; set; }
    public string Password { get; set; }
    public ICollection<Rentals> Rentals { get; set; } = new List<Rentals>();

}