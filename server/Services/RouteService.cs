using System.Collections.Generic;
using MongoDB.Driver;
using Server.Models;

namespace Server.Services
{
    public class RouteService
    {
        private readonly IMongoCollection<Route> _routes;

        public RouteService(IMongoClient client)
        {
            var database = client.GetDatabase("TrainSystem");
            _routes = database.GetCollection<Route>("Routes");
        }

        public List<Route> Get() =>
            _routes.Find(route => true).ToList();

        public Route Create(Route route)
        {
            _routes.InsertOne(route);
            return route;
        }
    }
}