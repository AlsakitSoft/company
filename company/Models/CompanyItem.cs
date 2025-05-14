using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace company.Models
{
    public class CompanyItem
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
        public int AddUserId { get; set; }
        public DateTime AddDate { get; set; } = DateTime.Now;
        public int? EditUserId { get; set; }
        public DateTime? EditDate { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
