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
    [Route("api/NationalPark")]
    [ApiController]
    
    public class NationalParkController : Controller
    {
        private readonly INationalParkRepository _nationalParkRepository;
        private readonly IMapper _mapper;

        public NationalParkController(INationalParkRepository nationalParkRepository,IMapper mapper)
        {
            _nationalParkRepository = nationalParkRepository;
            _mapper = mapper;
        }
        [HttpGet]
        public IActionResult GetNationalParks()
        {
            var nationalParkDtoList = _nationalParkRepository.GetNationalParks().Select(_mapper.Map<NationalParkDTOs>);
            return Ok(nationalParkDtoList);
        }

        [HttpPost]
        public IActionResult CreateNationalPark([FromBody] NationalParkDTOs nationalParkDto) 
        {
            if(nationalParkDto == null) return BadRequest();
            if (!ModelState.IsValid) return BadRequest();
            if (_nationalParkRepository.NationalParkExists(nationalParkDto.Name))
            {
                ModelState.AddModelError("", "National park Is Already In Database");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            var nationalPark = _mapper.Map<NationalParkDTOs,NationalPark>(nationalParkDto);
            if (!_nationalParkRepository.CreateNationalPark(nationalPark))
            {
                ModelState.AddModelError("", "Something went Wrong creating national park");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            //return Ok();
            //Displaying the input data straight after getting it
            return CreatedAtRoute("GetNationalPark", new { nationalParkId = nationalPark.Id }, nationalPark);
            
        }

        
        [HttpGet("{nationalParkId = int }",Name = "GetNationalPark")]

        public IActionResult GetNationalPark(int nationalParkId)
        {
            var nationalPark = _nationalParkRepository.GetNationalPark(nationalParkId);
            if (nationalPark == null)
            {
                return NotFound();
            }
            var nationalParkDto = _mapper.Map<NationalPark, NationalParkDTOs>(nationalPark);
            return Ok(nationalParkDto);
        }

        [HttpPatch]
        public IActionResult UpdateNationalPark([FromBody]NationalParkDTOs nationalPark)
        {
            if(nationalPark == null) return BadRequest();
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var NationalPark = _mapper.Map<NationalPark>(nationalPark);
            if (!_nationalParkRepository.UpdateNationalPark(NationalPark))
            {
                ModelState.AddModelError("", "Something went wrong while updating National Park");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return NoContent();
        }

        [HttpDelete("{nationalParkId:int}")]
        public IActionResult DeleteNationalPark(int nationalParkId)
        {
            if(!_nationalParkRepository.NationalParkExists(nationalParkId)) return NotFound();
            var nationalPark = _nationalParkRepository.GetNationalPark(nationalParkId);
            if(nationalPark == null)return NotFound();
            if (!_nationalParkRepository.DeleteNationalPark(nationalPark))
            {
                ModelState.AddModelError("", "Something went wrong while Deleting National Park");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return Ok();
        }


    }
}
