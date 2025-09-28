using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using National_Park_Project.Model;
using National_Park_Project.Repository.IRepository;

namespace National_Park_Project.Controllers
{
    [Route("api/[controller]")]
    
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        public AccountController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] User user)
        {
            if (ModelState.IsValid)
            {
                var isUniqueUser = _userRepository.IsUniqueUser(user.UserName);
                if (!isUniqueUser) return BadRequest("User IN Use");
                var userInfo = _userRepository.Register(user.UserName, user.Password);

                if (userInfo == null) return NotFound();
                user = userInfo;

            }
            return Ok(user);
        }

        [HttpPost("Login")]
        public IActionResult Login([FromBody] User user)
        {
            var userFromDb = _userRepository.Authenticate(user.UserName, user.Password);
            if(userFromDb == null) return BadRequest("Wrong User /pwd");
            return Ok(userFromDb);  
        }

    }
}
