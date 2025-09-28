using Microsoft.AspNetCore.Mvc;
using National_Park_Web_App.Models;
using National_Park_Web_App.Models.ViewModels;
using National_Park_Web_App.Repository.IRepository;
using System.Diagnostics;
using System.Threading.Tasks;

namespace National_Park_Web_App.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly INationalParkRepository _nationalParkRepository;
        private readonly ITrailRepository _trailrepository;

        public HomeController(ILogger<HomeController> logger, ITrailRepository trailrepository, INationalParkRepository nationalParkRepository)
        {
            _logger = logger;
            _trailrepository = trailrepository;
            _nationalParkRepository = nationalParkRepository;

        }

        public async Task<IActionResult> Index()
        {
            IndexVM indexVM = new()
            {
                NationalParkList = await _nationalParkRepository.GetAllAsync(SD.NationalParkAPIPath),
                TrailList = await _trailrepository.GetAllAsync(SD.TrailAPIPath)

            };
            return View(indexVM);
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

        public IActionResult Appointment()
        {
            return View(); 
        }
    }
}
