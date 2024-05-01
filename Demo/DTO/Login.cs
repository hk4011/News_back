using System.ComponentModel.DataAnnotations;

namespace Demo.DTO
{
    public class Login
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
