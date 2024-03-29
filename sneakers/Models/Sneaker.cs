﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace sneakers.Models
{
    public class Sneaker
    {
        [Key]
        public int SneakerId { get; set; }
        [Required]
        [Display(Name = "Size")]
        public int SizeId { get; set; }
        [Required]
        public Size Size { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        [Display(Name = "Brand")]
        public int BrandId { get; set; }
        [Required]
        public Brand Brand { get; set; }
        public bool IsSold { get; set; }
        [Required]
        [Display(Name = "Condition")]
        public int ConditionId { get; set; }
        [Required]
        public Condition Condition { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public double Price { get; set; }
        [Display(Name = "Photo")]
        public string ImgPath { get; set; }

        [Required]
        public string UserId { get; set; }
        [Required]
        public ApplicationUser User { get; set; }
        public virtual ICollection<Message> Messages { get; set; }
        public Sneaker()
        {
            IsSold = false;
        }
    }
}