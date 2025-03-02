namespace MovieRental.Frontend.Models;

using System.ComponentModel.DataAnnotations;

public class LoginModel {
	[Required] [StringLength(255)] public string Username { get; set; }

	[Required]
	[StringLength(30)]
	[DataType(DataType.Password)]
	[MinLength(6)]
	public string Password { get; set; }
}
