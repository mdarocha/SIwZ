using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Server.Database;
using Server.Models;

namespace Server.Services
{
    public class JwtService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly TrainSystemContext _context;
        private readonly IConfiguration _configuration;
        
        public JwtService(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            TrainSystemContext context,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _configuration = configuration;
        }

        public async Task<Tuple<User, string>> Login(string email, string password)
        {
            var result = await _signInManager
                .PasswordSignInAsync(email, password, false, false);

            if (!result.Succeeded) return null;
            
            var user = _context.Users.Single(u => u.Email == email);
            var token = GenerateJwtToken(user);
            return Tuple.Create(user, token); 
        }

        public async Task<Tuple<User, string>> Register(string email, string password, string name, string surname)
        {
            var user = new User
            {
                UserName = email,
                Email = email,
                Name = name,
                Surname = surname
            };

            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded) return null;

            await _signInManager.SignInAsync(user, false);
            var token = GenerateJwtToken(user);

            return Tuple.Create(user, token);
        }
        
        private string GenerateJwtToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim("IsAdmin", user.IsAdmin.ToString())
            };

            var key = new SymmetricSecurityKey(_configuration.GetSecretKey());
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(7);

            var token = new JwtSecurityToken(
                "",
                "",
                claims,
                expires: expires,
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}