using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Repositories;
using DAL.Classes;
using AutoMapper;
using Fribergs.Core.DTO;

namespace MarcusRent.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IApplicationUserRepository _userRepository;
        private readonly IMapper _mapper;

        // Konstruktor för att injicera beroenden
        public UsersController(IApplicationUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
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
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound($"Användare med ID {id} hittades inte.");
            }

            await _userRepository.ApproveUserAsync(id);
            return Ok($"Användaren med ID {id} har blivit godkänd.");
        }


        // PUT: api/users/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateUser(string id, [FromBody] ApplicationUserDto applicationUserDto)
        {
            var user = await _userRepository.GetUserByIdAsync (id);
            
            if (user == null)
            {
                return NotFound();
            }

            
            _mapper.Map(applicationUserDto, user);

            // Uppdatera användaren
            var success = await _userRepository.UpdateUserAsync(user);
            if (!success)
            {
                return NotFound();
            }

            
            return NotFound("Användaren har uppdateras.");
        }


        // DELETE: api/users/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(string id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound($"Användare med ID {id} hittades inte.");
            }

            await _userRepository.DeleteUserAsync(id);
            return Ok($"Användaren med ID {id} har tagits bort.");
        }

    }
}
