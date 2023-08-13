using DapperDemo.Models;
using DapperDemo.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DapperDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBonusRepository _bonRepo;

        public HomeController(ILogger<HomeController> logger, IBonusRepository bonRepo)
        {
            _logger = logger;
            _bonRepo = bonRepo;
        }


        //////////////////////////////////////////////
        /////////////////////////////////////////////////
        public IActionResult Index()
        {
            IEnumerable<Company> companies = _bonRepo.GetAllCompanyWithEmployees();

            return View(companies);
        }


        //////////////////////////////////////////////
        /////////////////////////////////////////////////
        public IActionResult AddTestRecords()
        {
            return View();
        }

        public IActionResult RemoveTestRecords()
        {
            return View();
        }


        //////////////////////////////////////////////
        /////////////////////////////////////////////////
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}