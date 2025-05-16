using company.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace company.Repositories
{
    public interface ICompanyRepository
    {
        Task SaveCompanyAsync(CompanyItem company);
        Task<IEnumerable<CompanyItem>> GetAllCompaniesAsync();

    }
}
