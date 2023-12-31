﻿using QueueSystem.Data;
using System.ComponentModel.DataAnnotations;

namespace QueueSystem.Models
{
    public class ServicePointM
    {
        [Display(Name = "Service Point Id")]
        public int Id { get; set; }

        [Display(Name = "Service Point Name")]
        [Required]
        public string Name { get; set; }
        public User User { get; set; }

        [Display(Name = "Service Provider Id")]
        [Required]
        public int ServiceProviderId { get; set; }
    }
}
