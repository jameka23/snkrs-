using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace sneakers.Models
{
    public class Brand
    {
        [Key]
        public int BrandId { get; set; }
        [Required]
        public string BrandType { get; set; }
    }
}