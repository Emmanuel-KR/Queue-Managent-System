using System.ComponentModel.DataAnnotations;

namespace QueueSystem.Models
{
    public class LoginM
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Password { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberLogin { get; internal set; }
    }
}
