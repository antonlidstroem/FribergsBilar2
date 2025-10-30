using AutoMapper;
using DAL.Classes;
using DAL.Repositories;
using FribergsApi.Models;
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
        private readonly IMapper _mapper;

        public CarsController(ICarRepository carRepository, ILogger<CarsController> logger, IMapper mapper)
        {
            _carRepository = carRepository;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CarDto>>> GetCars()
        {
            try
            {
                var cars = await _carRepository.GetAllAsync();
                if (cars == null)
                {
                    return NotFound("No cars found.");
                }

                //Mapping
                var carDtos = _mapper.Map<IEnumerable<CarDto>>(cars);
                return Ok(carDtos);

            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving cars.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CarDto>> GetCar(int id)
        {
            try
            {
                var car = await _carRepository.GetByIdAsync(id);
                if (car == null)
                {
                    return NotFound($"Car with ID {id} not found.");
                }

                var carDto = _mapper.Map<CarDto>(car);
                return Ok(carDto);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the car.");
                return StatusCode(500, "Internal server error");
            }
        }

        

        [HttpPost]
        public async Task<ActionResult<CarDto>> CreateCar([FromBody] CarDto carDto)
        {
            try
            {
                if (carDto == null)
                {
                    return BadRequest("Car data is required.");
                }


                var car = _mapper.Map<Car>(carDto);
                await _carRepository.AddAsync(car);

                var createdCarDto = _mapper.Map<CarDto>(car);
                return CreatedAtAction(nameof(GetCar), new { id = car.CarId }, createdCarDto);

            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the car.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCar(int id, [FromBody] CarDto carDto)
        {
            try
            {
                if (id != carDto.CarId)
                {
                    return BadRequest("Car ID mismatch.");
                }

                var existingCar = await _carRepository.GetByIdAsync(id);
                if (existingCar == null)
                {
                    return NotFound($"Car with ID {id} not found.");
                }

                var car = _mapper.Map<Car>(carDto);
                await _carRepository.UpdateAsync(car);

                return NoContent();
            }
            catch (Exception ex)
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

                await _carRepository.DeleteAsync(id);  
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
