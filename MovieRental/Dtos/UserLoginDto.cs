namespace Auth_course.Entity.Models;

public class UserLoginDto
{
    public required string UsernameOrMail { get; set; }
    public required string Password { get; set; }
}