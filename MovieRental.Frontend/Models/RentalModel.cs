namespace MovieRental.Frontend.Models;

public class RentalModel {
	public int Id { get; set; }
	public Guid UserId { get; set; }
	public decimal TotalPrice { get; set; }
	public int MovieId { get; set; }
	public DateOnly RentalDate { get; set; }
	public DateOnly ReturnDate { get; set; }
	public bool Returned { get; set; }
}
