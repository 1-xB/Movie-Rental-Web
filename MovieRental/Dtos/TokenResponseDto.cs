﻿namespace Auth_course.Entity.Models
{
    public class TokenResponseDto
    {
        public required string AccessToken { get; set; }
        public required string RefreshToken { get; set; }
        public required Guid UserId { get; set; }
    }
}
