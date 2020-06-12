using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Server.Models;
using Server.Database;

namespace Server.Services
{
    public class UserService
    {
        private readonly TrainSystemContext _context;


        public UserService(TrainSystemContext context)
        {
            _context = context;
        }

        public List<User> GetAll()
        {
            return _context.Users.Where(x => true).ToList();
        }

        public User GetUser(string id)
        {
            return _context.Users.Find(id);
        }
    }
}