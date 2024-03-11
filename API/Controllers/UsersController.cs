using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Enitites;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {

        private readonly DataContext _context;
        public UsersController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUsers>>> GetUsers()
        {
            var users = await  _context.Users.ToListAsync();
            return Ok(users);
        }

        // api/users/1
        [HttpGet("{id}")]
        public async Task<ActionResult<AppUsers?>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            return Ok(user) ;
        }

    }
}