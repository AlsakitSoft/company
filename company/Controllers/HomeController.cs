using company.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

using Microsoft.AspNetCore.Mvc;
using company.Models;

using company.Repositories;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace MainWebProject.Controllers
{
    public class HomeController : Controller
    {
        //private readonly ILogger<HomeController> _logger;

        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}

        public IActionResult Index()
        {
            return View();
        }
        private readonly ICompanyRepository _companyRepo;

        public HomeController(ICompanyRepository companyRepo)
        {
            _companyRepo = companyRepo;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SaveCompany(CompanyViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Create", model);
            }

            var company = (CompanyItem)model;
            await _companyRepo.SaveCompanyAsync(company);

            TempData["SuccessMessage"] = "تم حفظ بيانات الشركة بنجاح.";
            return RedirectToAction("Index");
        }
    
    public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

