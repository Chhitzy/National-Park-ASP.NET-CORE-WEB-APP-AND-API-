using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using National_Park_Web_App.Models.ViewModels;
using National_Park_Web_App.Repository.IRepository;
using System.Threading.Tasks;

namespace National_Park_Web_App.Controllers
{
    public class TrailController : Controller
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ITrailRepository _trailRepository;
        private readonly INationalParkRepository _nationalParkRepository;

        public TrailController(ITrailRepository trailRepository, INationalParkRepository nationalParkRepository)
        {
            _trailRepository = trailRepository;
            _nationalParkRepository = nationalParkRepository;
        }
        public IActionResult Index()
        {
            return View();
        }

        #region APis

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Json(new { data = await _trailRepository.GetAllAsync(SD.TrailAPIPath) });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var status = await _trailRepository.DeleteAsync(SD.TrailAPIPath, id);
            if (status)
            {
                return Json(new { success = true, message = "data deleted successfully" });
            }
            else
            {
                return Json(new { success = false, message = "Something went wrong while data" });
            }
           
        }



        #endregion

        public async Task<IActionResult> Upsert (int? id)
        {
            var nationalParkList = await _nationalParkRepository.GetAllAsync(SD.NationalParkAPIPath); //check

            TrailVM trailVM = new()
            {
                NationalParkList = nationalParkList.Select(n => new SelectListItem()
                {
                    Text = n.Name,
                    Value = n.Id.ToString()
                }),
                Trail = new()
            };

            if(id == null)
            {
                return View(trailVM);
            }
            trailVM.Trail = await _trailRepository.GetAsync(SD.TrailAPIPath, id.GetValueOrDefault());
            if (trailVM.Trail == null) return NotFound();

            return View(trailVM);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public  async Task<IActionResult> Upsert (TrailVM trailVM)
        {
            if (trailVM == null) return BadRequest();
            if(!ModelState.IsValid) return BadRequest();

            if(trailVM.Trail.Id == 0) 
                await _trailRepository.CreateAsync(SD.TrailAPIPath, trailVM.Trail);
            else
                await _trailRepository.UpdateAsync(SD.TrailAPIPath,trailVM.Trail);
            return RedirectToAction(nameof(Index));
        }
    }
}

