using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sneakers.Models.SneakersViewModels
{
    public class MyProfileViewModel
    {
        public ApplicationUser CurrUser { get; set; }
        public IFormFile Photo { get; set; }

    }
}
