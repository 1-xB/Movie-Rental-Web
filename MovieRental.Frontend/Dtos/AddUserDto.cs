using System.ComponentModel.DataAnnotations;

namespace MovieRental.Frontend.Dtos;

public class AddUserDto
{
    [Required, MinLength(4)] public required string? Username { get; set; }
    [Required, EmailAddress] public required string? Mail { get; set; }
    [Required, MinLength(6)] public required string Password { get; set; }
}