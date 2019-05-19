using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

using AuthApi.Models;

namespace AuthApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserContext _context;

        public UserController(UserContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<User> GetUsers()
        {
            var users = _context.Users.ToList();
            var _users = users.Select(user =>
            {
                user.Password = null;
                return user;
            });
            return _users;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(long id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            user.Password = null;
            return user;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(User user)
        {
            var _user = await _context.Users.SingleOrDefaultAsync(
                u => u.Username == user.Username
            );

            if (_user != null)
            {
                return BadRequest(new { error = "Username already taken" });
            }

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<object>> Login(User user)
        {
            var _user = await _context.Users.SingleOrDefaultAsync(
                u => u.Username == user.Username && u.Password == user.Password
            );

            if (_user == null)
                return NotFound();

            var tokenHandler = new JwtSecurityTokenHandler();
            var secret = Encoding.ASCII.GetBytes("70e396f94d84b205077262ba81143762");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] {
                    new Claim(ClaimTypes.Name, _user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(5),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(secret),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };
            var _token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenKey = tokenHandler.WriteToken(_token);

            return Ok(new { token = tokenKey });
        }
    }
}
