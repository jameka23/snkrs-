using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sneakers.Models.ReviewsViewModels
{
    public class ReviewsCreateViewModel
    {
        public Review Review { get; set; }
        public ApplicationUser User { get; set; }
    }
}
