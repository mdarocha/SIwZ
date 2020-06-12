using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Server.Models;
using Server.Services;

namespace Server.Controllers.Admin
{
    [Route("/api/admin/users")]
    [ApiController]
    public class AdminUserController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly TicketsService _ticketService;

        public AdminUserController (UserService userService, TicketsService ticketService)
        {
            _userService = userService;
            _ticketService = ticketService;
        }
        
        [HttpGet]
        [Route("")]
        public ActionResult<List<User>> GetAll()
        {
            var users = _userService.GetAll();
            return Ok(users);
        }
        
        [HttpGet]
        [Route("{id}")]
        public ActionResult<List<User>> GetUser([FromRoute] string id)
        {
            var user = _userService.GetUser(id);
            return Ok(user);
        }
        
        [HttpGet]
        [Route("{id}/tickets")]
        public ActionResult<List<User>> GetUserTickets([FromRoute] string id)
        {
            var user = _userService.GetUser(id);
            var tickets = _ticketService.GetUserTickets(user.Email);
            return Ok(tickets);
        }
        
        [HttpGet]
        [Route("{id}/tickets/{ticketId}")]
        public ActionResult<List<User>> GetUserTicket([FromRoute] string id, [FromRoute] int ticketId)
        {
            var user = _userService.GetUser(id);
            var tickets = _ticketService.GetUserTickets(user.Email);
            var ticket = _ticketService.GetUserTicket(ticketId, tickets);
            
            return Ok(ticket);
        }
        
        [HttpDelete]
        [Route("{id}/tickets/{ticketId}")]
        public ActionResult<List<User>> DeleteUserTicket([FromRoute] string id, [FromRoute] int ticketId)
        {
            var user = _userService.GetUser(id);
            var tickets = _ticketService.GetUserTickets(user.Email);
            var ticket = _ticketService.GetUserTicket(ticketId, tickets);
            _ticketService.DeleteTicket(ticket.Id);
            
            return Ok(ticket);
        }
    }
}