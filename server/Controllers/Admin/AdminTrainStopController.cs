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
    //[Authorize]  if uncommented it's always failing fetch
    public class AdminTrainStopController : ControllerBase
    {
        private readonly TrainStopService _trainStopService;

        public AdminTrainStopController(TrainStopService service)
        {
            _trainStopService = service;
        }

        [HttpGet]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<List<TrainStop>> GetAll()
        {
            var stops = _trainStopService.GetAll();
            return Ok(stops);
        }
        
        [HttpGet("{id}")]
        public ActionResult<List<TrainStop>> GetById([FromRoute] int id) 
        {
            var trains = _trainStopService.GetById(id);
            return Ok(trains);
        }
            
        
        [HttpPost]
        [Route("")]
        public ActionResult<TrainStop> Create(TrainStop trainStop)
        {
            var ts = _trainStopService.Create(trainStop);

            return Ok(ts);
        }
        
        [HttpPatch]
        [Route("")] 
        public ActionResult<Train> Edit(TrainStop trainStop)
        {
            _trainStopService.Edit(trainStop);
            return Ok(); 
        }
        
        [HttpDelete("{id}")]
        [Route("")]
        public ActionResult Delete([FromRoute]int id)
        {
            var deleteTrainStop = _trainStopService.GetById(id);
            _trainStopService.Delete(deleteTrainStop);
            
            return Ok();
        }
        
        
    }
}