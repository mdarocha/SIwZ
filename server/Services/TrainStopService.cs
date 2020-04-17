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

        public List<TrainStop> Get() => _context.TrainStops.Where(ts => true).ToList();

        public TrainStop Get(string id) => _context.TrainStops.Find(id);

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
    }
}