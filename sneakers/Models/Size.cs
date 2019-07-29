using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace sneakers.Models
{
    public class Size
    {
        [Key]
        public int SizeId { get; set; }
        [Required]
        public string ShoeSize { get; set; }
    }
}