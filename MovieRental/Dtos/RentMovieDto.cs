namespace MovieRental.Dtos;

public class RentMovieDto
{
    public int MovieId { get; set; }
    public DateOnly ReturnDate { get; set; }
}