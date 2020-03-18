using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server.Models;
using Server.Services;


namespace Server.Controllers
{
    
    [Route("/api/[controller]")]
    [ApiController]
    public class TrainStopController : ControllerBase
    {
        private readonly TrainStopService _trainStopService;

        public TrainStopController(TrainStopService service)
        {
            _trainStopService = service;
        }

        [HttpGet(nameof(Get))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<List<TrainStop>> Get() =>
            _trainStopService.Get();

        [HttpGet(nameof(GetStop))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<TrainStop> GetStop(string id)
        {
            var ts = _trainStopService.Get(id);

            if (ts == null)
                return new NotFoundResult();
            
            return ts;
        }

        [HttpPost(nameof(Create))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<TrainStop> Create([FromBody]String city)
        {
            var ts = new TrainStop(city);
            return new CreatedAtRouteResult("TrainStop", new {id = ts.Id}, ts);
        }
    }
}

