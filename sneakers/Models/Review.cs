using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace sneakers.Models
{
    public class Review
    {
        [Key]
        public int ReviewId { get; set; }
        [Range(1, 5,
        ErrorMessage = "Rating must be between 1 and 5.")]
        [Required]
        public int Rating { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public ApplicationUser User { get; set; }
    }
}