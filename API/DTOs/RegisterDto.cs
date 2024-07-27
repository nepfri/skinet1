using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class RegisterDto
    {
        [MaxLength(100)]
        [Required]
        public required string Username { get; set; }
        [Required]
        public required string Password { get; set; }
    }
}
