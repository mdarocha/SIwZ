using Microsoft.AspNetCore.Mvc;
using Server.Models;
using Server.Services;

namespace Server.Controllers.Admin
{
    [Route("/api/admin/[controller]")]
    [ApiController]
    public class AdminTrainStopController : ControllerBase
    {
        private readonly TrainStopService _trainStopService;

        public AdminTrainStopController(TrainStopService service)
        {
            _trainStopService = service;
        }
        
        [HttpPost]
        [Route("Create")]
        public ActionResult<TrainStop> Create([FromBody] TrainStop trainStop)
        {
            var ts = _trainStopService.Create(trainStop);

            return Ok(ts);
        }
        
        
    }
}