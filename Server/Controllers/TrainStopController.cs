using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server.Models;
using Server.Services;


namespace Server.Controllers
{

    public class dlskjldsfkj
    {
        public string City { get; set; }
    }
    
    [Route("/api/[controller]")]
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

        [HttpPost]
        [Route("Chuj")]
        public ActionResult<TrainStop> Create([FromBody] dlskjldsfkj city)
        {
//            var ts = new TrainStop(city);
//            return new CreatedAtRouteResult("TrainStop", new {id = ts.Id}, ts);
            Debug.Print("dupa");
            return new TrainStop($"A man has fallen in a river in {city.City}");
        }
        
    }
}

