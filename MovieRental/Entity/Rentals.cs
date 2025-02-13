namespace MovieRental.Entity;

public class Rentals
{
    public int Id { get; set; }
    public Guid UserId { get; set; }
    public Users? User { get; set; }
    public int MovieId { get; set; }
    public Movie? Movie { get; set; }
    public DateOnly RentalDate { get; set; }
    public DateOnly ReturnDate { get; set; }
    public bool Returned { get; set; }
}