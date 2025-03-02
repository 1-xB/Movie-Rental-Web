namespace MovieRental.Frontend.Models;

using System.ComponentModel.DataAnnotations;

public class RegisterModel {
	[Required]
	[StringLength(30)]
	[MinLength(3)]
	public string Username { get; set; }

	[Required]
	[StringLength(255)]
	[EmailAddress]
	public string Mail { get; set; }

	[Required]
	[StringLength(255)]
	[DataType(DataType.Password)]
	[MinLength(6)]
	public string Password { get; set; }

	[Required]
	[StringLength(255)]
	[DataType(DataType.Password)]
	[Compare("Password", ErrorMessage = "Passwords do not match.")]
	[MinLength(6)]
	public string ConfirmPassword { get; set; }
}
