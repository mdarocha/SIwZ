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
        [Route("list")]
        public ActionResult<List<Discount>> GetAll()
        {
            var discounts = _service.GetAll();
            return Ok(discounts);
        }

        [HttpPatch]
        [Route("edit")]
        public ActionResult<Discount> Edit(Discount discount)
        {
            return Ok();
        }

        [HttpPost]
        [Route("add")]
        public ActionResult<Discount> Add(Discount discount)
        {
            var d = _service.Create(discount);
            return d;
        }

        [HttpDelete]
        [Route("delete")]
        public ActionResult Delete(int id)
        {
            return Ok();
        }
        
    }
}