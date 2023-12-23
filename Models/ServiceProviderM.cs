using System.ComponentModel.DataAnnotations;

namespace QueueSystem.Models
{
    public class ServiceProviderM
    {
        [Display(Name = "Service Provider Id")]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Password { get; set; }
        

        [Display(Name = "Service Point Id")]
        public int ServicepointId { get; set; }
    }
}
