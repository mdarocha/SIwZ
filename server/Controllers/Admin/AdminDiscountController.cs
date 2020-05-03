using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Server.Models;
using Server.Database;
using Server.Services;

namespace Server.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/discounts")]
    public class AdminDiscountController : ControllerBase
    {
        private readonly DiscountService _service;

        public AdminDiscountController(DiscountService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("")]
        public ActionResult<List<Discount>> GetAll()
        {
            var discounts = _service.GetAll();
            return Ok(discounts);
        }
        
        [HttpGet("{id}")]
        public ActionResult<List<Discount>> GetById([FromRoute] int id)
        {
            var discounts = _service.GetById(id);
            return Ok(discounts);
        }

        [HttpPatch]
        [Route("")]
        public ActionResult<Discount> Edit(Discount discount)
        {
            _service.Edit(discount);
            return Ok();
        }

        [HttpPost]
        public ActionResult<Discount> Add(Discount discount)
        {
            var d = _service.Create(discount);
            return d;
        }

        [HttpDelete("{id}")]
        [Route("")]
        public ActionResult Delete([FromRoute]int id)
        {
            var deleteDiscount = _service.GetById(id);
            _service.Delete(deleteDiscount);
            return Ok();
        }
        
    }
}