using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sneakers.Models.SneakersViewModels
{
    public class SneakersCreateViewModel
    {
        public Sneaker Sneaker { get; set; }
        public List<Brand> AvailableBrands { get; set; }
        public List<Condition> AvailableConditions { get; set; }
        public List<Size> AvailableSizes { get; set; }

        // must create the options for the view to actually hold 
        public List<SelectListItem> BrandOptions
        {
            get
            {
                if (AvailableBrands == null)
                {
                    return null;
                }
                var ab = AvailableBrands?.Select(a => new SelectListItem(a.BrandType, a.BrandId.ToString())).ToList();
                ab.Insert(0, new SelectListItem("Select a brand", null));

                return ab;
            }
        }
        public List<SelectListItem> ConditionOptions
        {
            get
            {
                if (AvailableConditions == null)
                {
                    return null;
                }
                var ab = AvailableConditions?.Select(a => new SelectListItem(a.ConditionType, a.ConditionId.ToString())).ToList();
                ab.Insert(0, new SelectListItem("Select a condition", null));

                return ab;
            }
        }
        public List<SelectListItem> SizeOptions
        {
            get
            {
                if (AvailableSizes == null)
                {
                    return null;
                }
                var ab = AvailableSizes?.Select(a => new SelectListItem(a.ShoeSize, a.SizeId.ToString())).ToList();
                ab.Insert(0, new SelectListItem("Select a size", null));

                return ab;
            }
        }
        public IFormFile Photo { get; set; }

    }
}
