using System.ComponentModel.DataAnnotations;

namespace MovieRental.Frontend.Models;

public class LoginModel
{
    [Required, StringLength(255)] public string UsernameOrMail { get; set; }
    [Required, StringLength(30), DataType(DataType.Password), MinLength(6)] public string Password { get; set; }
}