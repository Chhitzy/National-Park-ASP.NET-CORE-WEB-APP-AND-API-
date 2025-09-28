using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using National_Park_Project.Model;
using National_Park_Project.Model.DTOs;
using National_Park_Project.Repository;
using National_Park_Project.Repository.IRepository;

namespace National_Park_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class TrailController : ControllerBase
    {
        private readonly ITrailRepository _trailRepository;
        private readonly IMapper _mapper;

        public TrailController(ITrailRepository Trail_Repository, IMapper Mapper)
        {
            _trailRepository = Trail_Repository;
            _mapper = Mapper;
        }

        [HttpGet]
        public IActionResult GetTrails()
        {
            return Ok(_trailRepository.GetTrails().Select(_mapper.Map<TrailDTO>));
        }
        [HttpGet("{trailid:int}",Name="GetTrail")]
        
        public IActionResult GetTrail(int id)
        {
            var trail = _trailRepository.GetTrail(id);
            if(trail == null)  return NotFound();
            var trailDto = _mapper.Map<TrailDTO>(trail);
            return Ok(trailDto);
        }

        [HttpPost]

        public IActionResult CreateTrail([FromBody] TrailDTO trail)
        {
            if (trail == null) return NotFound();
            if(!ModelState.IsValid) return BadRequest();
            if (_trailRepository.TrailExists(trail.Name))
            {
                ModelState.AddModelError("", "Trail Exists In Database");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            var Trail_Variable = _mapper.Map<Trail>(trail);
            if (!_trailRepository.CreateTrail(Trail_Variable))
            {
                ModelState.AddModelError("", "Error While Creating Trail");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return CreatedAtRoute("GetTrail", new {trailId = trail.Id},Trail_Variable);
        }
        [HttpPut]

        public IActionResult UpdateTrail([FromBody] TrailDTO trailDTO)
        {
            if (trailDTO == null) return BadRequest();
            if(!ModelState.IsValid) return BadRequest();    
            var trail = _mapper.Map<Trail>(trailDTO);
            if (!_trailRepository.UpdateTrail(trail))
            {
                ModelState.AddModelError("", "Something went wrong while updating Trail ");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return NoContent();
        }

        [HttpDelete("{trailId:int}")]
        public IActionResult DeleteTrail(int trailId)
        {
            if(!_trailRepository.TrailExists(trailId)) return NotFound();
            var trail = _trailRepository.GetTrail(trailId);
            if (trail == null) return NotFound();
            if (!_trailRepository.DeleteTrail(trail))
            {
                ModelState.AddModelError("", "Something went wrong while Deleting Trail of Park");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return Ok();
        }


    }
}
