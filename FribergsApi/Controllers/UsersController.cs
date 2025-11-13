using Microsoft.AspNetCore.Mvc;
using FribergsApi.Models;  // Eller använd din egna modeller för användare
using DAL.Repositories;    // Om du har en repository som hanterar användare

namespace FribergsApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IApplicationUserRepository _userRepository;

        // Konstruktor: Injicera din repository för att hämta användardata
        public UsersController(IApplicationUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // GET: api/users
        [HttpGet]
        public async Task<ActionResult<List<ApplicationUserDto>>> GetAllUsers()
        {
            var users = await _userRepository.GetAllUsersAsync(); // Anpassa beroende på din repo-logik
            if (users == null || !users.Any())
            {
                return NotFound();
            }

            return Ok(users);
        }

        // Eventuellt en metod för att hämta en specifik användare
        // GET: api/users/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ApplicationUserDto>> GetUserById(string id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);  // Anpassa för din repo-logik
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        // Här kan du lägga till fler metoder för att skapa, uppdatera och ta bort användare om det behövs.
    }
}
