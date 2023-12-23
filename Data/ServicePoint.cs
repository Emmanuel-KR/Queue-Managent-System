using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace QueueSystem.Data
{
    public class ServicePoint
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        [ForeignKey("ServiceProviderId")]
        public User User { get; set; }
        public int ServiceProviderId { get; set; }
    }
}
