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
        public List<Review> UserReviews { get; set; }
        public List<Sneaker> UserSneakers { get; set; }
    }
}
