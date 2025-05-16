using company.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace company.Controllers
{
    public class IndexViewModel
    {
        public CompanyViewModel NewCompany { get; set; }
        public IEnumerable<CompanyViewModel> Companies { get; set; }
    }

}
