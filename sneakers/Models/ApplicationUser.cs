using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace sneakers.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {

        }

        // overriden class of the user 
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        public string ImgPath { get; set; }
        public double Rating { get; set; }
        public virtual ICollection<Sneaker> Sneakers { get; set; }

        public virtual ICollection<Review> Reviews { get; set; }

    }
}
