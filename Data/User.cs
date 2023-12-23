using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace QueueSystem.Data
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }

        [ForeignKey("ServicePointId")]
        public ServicePoint ServicePoint { get; set; }
        public int ServicePointId { get; set; }
    }
}
