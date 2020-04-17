using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Server.Models;
using Server.Database;

namespace Server.Services
{
    public class TrainService
    {
        private readonly TrainSystemContext _context;

        public TrainService(TrainSystemContext context)
        {
            _context = context;
        }

        public List<Train> GetAll()
        {
            return _context.Trains.Where(d => true).ToList();
        }

        public Train GetById(int id)
        {
            return _context.Trains.Find(id);
        }

        public List<Train> GetByType(int type) // train type is int
        {
            return _context.Trains.Where(d => d.Type.Equals(type)).ToList();
        }

        public void Edit(Train train)
        {
            var tr = _context.Trains.Find(train.Id);

            if (tr != null)
            {
                tr.Name = train.Name;
                tr.Seats = train.Seats;
                tr.Type = train.Type;
                tr.Wagons = train.Wagons;

                _context.Trains.Update(tr);
                _context.SaveChanges();
            }
        }
        
        public Train Create(Train train)
        {
            _context.Trains.Add(train);
            _context.SaveChanges();
            return train;
        }

        public void Delete(Train train)
        {
            _context.Trains.Remove(train);
            _context.SaveChanges();
        }
        
    }
}