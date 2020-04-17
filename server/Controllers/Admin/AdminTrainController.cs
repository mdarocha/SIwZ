using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Server.Models;
using Server.Services;

namespace Server.Controllers.Admin

{    
    [ApiController]
    [Route("api/admin/trains")]
    public class AdminTrainController : ControllerBase
    {
        private readonly TrainService _service;

        public AdminTrainController(TrainService service)
        {
            _service = service;
        }
        
        [HttpGet]
        [Route("")]
        public ActionResult<List<Train>> GetAll() 
        {
            var trains = _service.GetAll();
            return Ok(trains);
        }
        
        [HttpGet]
        [Route("/{id}")]
        public ActionResult<List<Train>> GetById(int id) 
        {
            var trains = _service.GetById(id);
            return Ok(trains);
        }
        
        [HttpPost]
        [Route("/add")]
        public ActionResult<Train> Add(Train train) // 
        {
            //Validation pls
            var d = _service.Create(train); 
            return d;
        }

        [HttpPatch]
        [Route("/edit")] 
        public ActionResult<Train> Edit(Train train)
        {
            _service.Edit(train);
            return Ok(); // to implement
        }
        
        [HttpDelete]
        [Route("/{id}")]
        public ActionResult Delete([FromRoute]int id)
        {
            var deleteTrain = _service.GetById(id);
            _service.Delete(deleteTrain);
            
            return Ok();
        }
        
    }
}