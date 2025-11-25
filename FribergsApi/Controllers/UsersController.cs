using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DAL.Classes;
using DAL.Interfaces;
using Fribergs.Core.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MarcusRent.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IApplicationUserRepository _userRepository;
        private readonly IMapper _mapper;
        public UsersController(IApplicationUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        // GET: api/users
        [HttpGet]
        public async Task<ActionResult<List<UserDto>>> GetAllUsers()
        {
            var users = await _userRepository.GetAllUsersAsync();
            if (users == null || users.Count == 0)
                return NotFound("Inga användare hittades.");

            var usersDto = _mapper.Map<List<UserDto>>(users);
            return Ok(usersDto);
        }

        // GET: api/users/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUserById(string id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
                return NotFound($"Användare med ID {id} hittades inte.");

            var dto = _mapper.Map<UserDto>(user);
            return Ok(dto);

        }

        // POST: api/users/{id}/approve
        [Authorize]
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
        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateUser(string id, [FromBody] UserDto userDto)
        {
            var user = await _userRepository.GetUserByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }
            _mapper.Map(userDto, user);
            var success = await _userRepository.UpdateUserAsync(user);
            if (!success)
            {
                return NotFound();
            }
            return Ok("Användaren har uppdateras.");
        }

        // DELETE: api/users/{id}
        [Authorize]
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
