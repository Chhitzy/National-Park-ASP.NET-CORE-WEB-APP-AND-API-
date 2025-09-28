using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using National_Park_Project.Data;
using National_Park_Project.Model;
using National_Park_Project.Repository.IRepository;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace National_Park_Project.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly AppSettings _appSettings;

        public UserRepository(ApplicationDbContext context,IOptions<AppSettings> appSettings)
        {
            _context = context;
            _appSettings = appSettings.Value;
        }
        public User Authenticate(string username, string password)
        {
            var userInDb = _context.Users.FirstOrDefault(u => u.UserName == username && u.Password == password);
            if (userInDb == null) return null;
            //JST TOKEN
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim (ClaimTypes.Name, userInDb.Id.ToString()),
                    new Claim (ClaimTypes.Role, userInDb.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(2),

                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)

            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            userInDb.Token = tokenHandler.WriteToken(token);

            //To be implemnted above

            userInDb.Password = "";
            return userInDb;
        }

        public bool IsUniqueUser(string username)
        {
            var UserInDb = _context.Users.FirstOrDefault(u => u.UserName == username);
            if (UserInDb == null) return true; return false;
        }

        public User Register(string username, string password)
        {
            User user = new()
            {
                UserName = username,
                Password = password,
                Role = "Admin"
            };
            _context.Users.Add(user);
            _context.SaveChanges();
            return user;
        }
    }
}
