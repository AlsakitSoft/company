using company.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

using Microsoft.AspNetCore.Mvc;
using company.Models;

using company.Repositories;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Linq;
using company.Controllers;

namespace MainWebProject.Controllers
{
    public class HomeController : Controller
    {
        //private readonly ILogger<HomeController> _logger;

        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}

        //public IActionResult Index()
        //{
        //    return View();
        //}
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // جلب الشركات
            var items = await _companyRepo.GetAllCompaniesAsync();

            // تحويل CompanyItem إلى CompanyViewModel
            var companiesVm = items.Select(ci => new CompanyViewModel
            {
                ComId = ci.ComId,
                ComArabicName = ci.ComArabicName,
                ComEnglishName = ci.ComEnglishName,
                ShortArabicName = ci.ShortArabicName,
                ShortEnglishName = ci.ShortEnglishName,
                ComWebsite = ci.ComWebsite,
                ComAddress = ci.ComAddress,
                ComNote = ci.ComNote,
                IsDefault = ci.IsDefault
            });

            var vm = new IndexViewModel
            {
                NewCompany = new CompanyViewModel(),
                Companies = companiesVm
            };

            return View(vm);
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
        //public async Task<IActionResult> SaveCompany(CompanyViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View("Create", model);
        //    }

        //    var company = (CompanyItem)model;
        //    await _companyRepo.SaveCompanyAsync(company);

        //    TempData["SuccessMessage"] = "تم حفظ بيانات الشركة بنجاح.";
        //    return RedirectToAction("Index");
        //}
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> SaveCompany([FromBody] CompanyViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(new
        //        {
        //            success = false,
        //            message = "بيانات غير صحيحة"
        //        });
        //    }

        //    var company = (CompanyItem)model;
        //    await _companyRepo.SaveCompanyAsync(company);

        //    // إذا كان طلب AJAX يتوقع JSON:
        //    if (Request.Headers["X-Requested-With"] == "XMLHttpRequest" ||
        //        Request.ContentType.Contains("application/json"))
        //    {
        //        return Json(new
        //        {
        //            success = true,
        //            message = "تم حفظ بيانات الشركة بنجاح.",
        //            redirectUrl = Url.Action("Index")
        //        });
        //    }

        //    // خلاف ذلك، سلوك الويب التقليدي:
        //    TempData["SuccessMessage"] = "تم حفظ بيانات الشركة بنجاح.";
        //    return RedirectToAction("Index");
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveCompany([Bind(Prefix = "NewCompany")] CompanyViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "بيانات غير صحيحة"
                });
            }

            var company = (CompanyItem)model;
            await _companyRepo.SaveCompanyAsync(company);

            return Json(new
            {
                success = true,
                message = "تم حفظ بيانات الشركة بنجاح.",
                redirectUrl = Url.Action("Index")
            });
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

