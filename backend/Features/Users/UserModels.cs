using System.ComponentModel.DataAnnotations;

namespace FaceItRadar.Features.Users
{
    public class User
    {
        [Key]
        public string user_id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [EmailAddress]
        public string email { get; set; } = string.Empty;

        [Required]
        public string password_hash { get; set; } = string.Empty;

        public void login(string email, string password)
        {
            // simple login function for the user model 

        }
    }

    public class CreateUserRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(6)]
        public string Password { get; set; } = string.Empty;
    }

}
