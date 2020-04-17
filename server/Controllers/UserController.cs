using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Server.Models;
using Server.Services;

namespace Server.Controllers
{
    [ApiController]
    [Route("/api/user")]
    public class UserController : Controller
    {
        private readonly JwtService _jwtService;

        public UserController(JwtService jwtService)
        {
            _jwtService = jwtService;
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult> Login([FromBody] LoginDto login)
        {
            var result = await _jwtService.Login(login.Email, login.Password);
            if (result == null) return StatusCode(400);

            var (user, token) = result;
            return Ok(new UserResultDto
            {
                Id = user.Id,
                Email = user.Email,
                Name = user.Name,
                Surname = user.Surname,
                Token = token
            });
        }

        [HttpPost]
        [Route("register")]
        public async Task<ActionResult> Register([FromBody] RegisterDto register)
        {
            var result = await _jwtService.Register(register.Email, register.Password, register.Name, register.Surname);
            if (result == null) return StatusCode(400);

            var (user, token) = result;
            return Ok(new UserResultDto
            {
                Id = user.Id,
                Email = user.Email,
                Name = user.Name,
                Surname = user.Surname,
                Token = token
            });
        }

        public class LoginDto
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        public class RegisterDto
        {
            public string Email { get; set; }
            public string Password { get; set; }
            public string Name { get; set; }
            public string Surname { get; set; }
        }

        public class UserResultDto
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Surname { get; set; }
            public string Email { get; set; }
            public string Token { get; set; }
        }
    }
}