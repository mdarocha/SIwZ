using System;
using Server.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace Server.Services
{
    public class TrainStopService
    {
        private readonly IMongoCollection<TrainStop> _trainStops;

        public TrainStopService(IMongoClient client)
        {
            var dataBase = client.GetDatabase("TrainSystem");
            _trainStops = dataBase.GetCollection<TrainStop>("TrainStops");
        }

        public List<TrainStop> Get() =>
            _trainStops.Find(trainStop => true).ToList();

        public TrainStop Get(string id) =>
            _trainStops.Find<TrainStop>(trainStop => trainStop.Id == id).FirstOrDefault();

        //TODO
        // void?
        public TrainStop Create(TrainStop trainStop)
        {
            _trainStops.InsertOne(trainStop);
            return trainStop;
        }
    }
}