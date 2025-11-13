using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using FribergsApi.Models;
using DAL.Repositories;
using Fribergs.Core.Models;

namespace MarcusRent.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IApplicationUserRepository _userRepository;

        // Konstruktor för att injicera beroenden
        public UsersController(IApplicationUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // GET: api/users
        [HttpGet]
        public async Task<ActionResult<List<ApplicationUserDto>>> GetAllUsers()
        {
            var users = await _userRepository.GetAllUsersAsync();
            if (users == null || users.Count == 0)
            {
                return NotFound("Inga användare hittades.");
            }
            return Ok(users);
        }

        // GET: api/users/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ApplicationUserDto>> GetUserById(string id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound($"Användare med ID {id} hittades inte.");
            }
            return Ok(user);
        }

        // POST: api/users/{id}/approve
        [HttpPost("{id}/approve")]
        public async Task<ActionResult> ApproveUser(string id)
        {
            await _userRepository.ApproveUserAsync(id);
            return Ok();
        }

        // PUT: api/users/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateUser(string id, [FromBody] ApplicationUserDto user)
        {
            if (id != user.Id)
            {
                return BadRequest("User ID mismatch.");
            }

            var success = await _userRepository.UpdateUserAsync(user);
            if (success)
            {
                return Ok($"Användaren {id} uppdaterades.");
            }

            return NotFound("Användaren kunde inte uppdateras.");
        }

        // DELETE: api/users/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(string id)
        {
            await _userRepository.DeleteUserAsync(id);
            
                return Ok($"Användaren {id} har tagits bort.");
           
        }
    }
}
