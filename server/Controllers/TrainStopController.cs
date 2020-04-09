using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server.Models;
using Server.Services;


namespace Server.Controllers
{
    [Route("/api/stops")]
    [ApiController]
    public class TrainStopController : ControllerBase
    {
        private readonly TrainStopService _trainStopService;

        public TrainStopController(TrainStopService service)
        {
            _trainStopService = service;
        }

        [HttpGet]
        [Route("Get")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<List<TrainStop>> Get() =>
            _trainStopService.Get();

        [HttpGet]
        [Route("GetStop")]
        public ActionResult<TrainStop> GetStop(string id)
        {
            var ts = _trainStopService.Get(id);

            if (ts == null)
                return new NotFoundResult();
            
            return ts;
        }

        [HttpGet]
        [Route("GetStops")]
        public ActionResult<List<TrainStop>> GetStops(string city, string name)
        {
            var ts = _trainStopService.Get(city, name);
            
            if (ts == null)
                return new NotFoundResult();
            
            return ts;
        }
        
    }
}

