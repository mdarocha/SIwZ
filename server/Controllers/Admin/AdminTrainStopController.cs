using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server.Models;
using Server.Services;

namespace Server.Controllers.Admin
{
    [Route("/api/admin/stops")]
    [ApiController]
    public class AdminTrainStopController : ControllerBase
    {
        private readonly TrainStopService _trainStopService;

        public AdminTrainStopController(TrainStopService service)
        {
            _trainStopService = service;
        }
        
        [HttpGet]
        [Route("Get")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<List<TrainStop>> Get() =>
            _trainStopService.Get();
        
        [HttpPost]
        [Route("Create")]
        public ActionResult<TrainStop> Create([FromBody] TrainStop trainStop)
        {
            var ts = _trainStopService.Create(trainStop);

            return Ok(ts);
        }
        
        
    }
}