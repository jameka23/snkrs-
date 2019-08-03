using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sneakers.Models.SneakersViewModels
{
    public class UserProfileViewModel
    {
        public ApplicationUser User { get; set; }

        public List<Sneaker> AllSneakers { get; set; }
    }
}
