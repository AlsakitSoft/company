using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace company.Models
{
    public class CompanyViewModel
    {
        public int ComId { get; set; }

        public string ComArabicName { get; set; }
        public string ComEnglishName { get; set; }
        public string ShortArabicName { get; set; }
        public string ShortEnglishName { get; set; }
        public string ComWebsite { get; set; }
        public string ComAddress { get; set; }
        public string ComNote { get; set; }

        public bool IsDefault { get; set; } = false;

        // تحويل من ViewModel إلى الكيان
        public static explicit operator CompanyItem(CompanyViewModel model)
        {
            return new CompanyItem
            {
                ComId = model.ComId,
                ComArabicName = model.ComArabicName,
                ComEnglishName = model.ComEnglishName,
                ShortArabicName = model.ShortArabicName,
                ShortEnglishName = model.ShortEnglishName,
                ComWebsite = model.ComWebsite,
                ComAddress = model.ComAddress,
                ComNote = model.ComNote,
                IsDefault = model.IsDefault
            };
        }
    }
}
