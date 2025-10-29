using DAL.Classes;
using DAL.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FribergsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarsController : ControllerBase
    {
        private readonly ICarRepository _carRepository;
        private readonly ILogger<CarsController> _logger;

        public CarsController(ICarRepository carRepository, ILogger<CarsController> logger)
        {
            _carRepository = carRepository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Car>>> GetCars()
        {
            try
            {
                var cars = await _carRepository.GetAllAsync();  // Förutsätter att GetAllCarsAsync finns i ditt repository
                if (cars == null)
                {
                    return NotFound("No cars found.");
                }
                return Ok(cars);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving cars.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Car>> GetCar(int id)
        {
            try
            {
                var car = await _carRepository.GetByIdAsync(id);  // Förutsätter att GetCarByIdAsync finns i ditt repository
                if (car == null)
                {
                    return NotFound($"Car with ID {id} not found.");
                }
                return Ok(car);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the car.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Car>> CreateCar([FromBody] Car car)
        {
            try
            {
                if (car == null)
                {
                    return BadRequest("Car data is required.");
                }

                await _carRepository.AddAsync(car);  // Förutsätter att AddCarAsync finns i ditt repository
                return CreatedAtAction(nameof(GetCar), new { id = car.CarId }, car);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the car.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCar(int id, [FromBody] Car car)
        {
            try
            {
                if (id != car.CarId)
                {
                    return BadRequest("Car ID mismatch.");
                }

                var existingCar = await _carRepository.GetByIdAsync(id);
                if (existingCar == null)
                {
                    return NotFound($"Car with ID {id} not found.");
                }

                await _carRepository.UpdateAsync(car);  // Förutsätter att UpdateCarAsync finns i ditt repository
                return NoContent();
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the car.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCar(int id)
        {
            try
            {
                var car = await _carRepository.GetByIdAsync(id);
                if (car == null)
                {
                    return NotFound($"Car with ID {id} not found.");
                }

                await _carRepository.DeleteAsync(id);  // Förutsätter att DeleteCarAsync finns i ditt repository
                return NoContent();
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the car.");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
