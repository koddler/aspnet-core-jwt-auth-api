using System.ComponentModel.DataAnnotations;

namespace AuthApi.Models
{
    public class User
    {
        public long Id { get; set; }

        [Required]
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
