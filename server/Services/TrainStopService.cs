using Server.Models;
using System.Collections.Generic;
using System.Linq;
using Server.Database;

namespace Server.Services
{
    public class TrainStopService
    {
        private readonly TrainSystemContext _context;

        public TrainStopService(TrainSystemContext context)
        {
            _context = context;
        }

        public List<TrainStop> GetAll()
        {
            return _context.TrainStops.Where(d => true).ToList();
        }

        public TrainStop GetById(int id)
        {
            return _context.TrainStops.Find(id);
        }

        public List<TrainStop> Get(string city, string name)
        {
            if (city != null && name != null)
                return _context.TrainStops.Where(ts => ts.Name == name && ts.City == city).ToList();
            return _context.TrainStops.Where(ts => ts.Name == name || ts.City == city).ToList();
        }

        public TrainStop Create(TrainStop trainStop)
        {
            _context.TrainStops.Add(trainStop);
            _context.SaveChanges();
            return trainStop;
        }
        
        public void Edit(TrainStop trainStop)
        {
            var ts = _context.TrainStops.Find(trainStop.Id);

            if (ts != null)
            {
                ts.City = trainStop.City;
                ts.Name = trainStop.Name;

                _context.TrainStops.Update(ts);
                _context.SaveChanges();
            }
        }
        
        public void Delete(TrainStop trainStop)
        {
            _context.TrainStops.Remove(trainStop);
            _context.SaveChanges();
        }
    }
}