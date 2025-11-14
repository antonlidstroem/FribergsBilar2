using System.Globalization;
using AutoMapper;
using DAL.Repositories;
using Fribergs.Core.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MarcusRent.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public OrdersController(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        // GET: api/orders
        [HttpGet]
        public async Task<ActionResult<List<OrderDto>>> GetOrders()
        {
            var orders = await _orderRepository.GetAllOrdersAsync();
            return Ok(orders);
        }

        // GET: api/orders/{id}
        //[Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDto>> GetOrderById(int id)
        {
            var order = await _orderRepository.GetOrderByIdAsync(id);
            if (order == null) return NotFound();
            return Ok(order);
        }

        // GET: api/orders/user/{userId}
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<List<OrderDto>>> GetOrdersByUser(string userId)
        {
            var orders = await _orderRepository.GetOrdersByUserIdAsync(userId);
            return Ok(orders);
        }

        // GET: api/orders/isCarBooked?carId=1&startDate=2025-11-12&endDate=2025-11-15
        [HttpGet("isCarBooked")]
        public async Task<ActionResult<bool>> IsCarBooked(int carId, string startDate, string endDate)
        {
            if (!DateTime.TryParseExact(startDate, "yyyy-MM-dd", CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out var start))
                return BadRequest("Invalid startDate format");

            if (!DateTime.TryParseExact(endDate, "yyyy-MM-dd", CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out var end))
                return BadRequest("Invalid endDate format");

            var result = await _orderRepository.IsCarBookedAsync(carId, start, end);
            return Ok(result);
        }

        // GET: api/orders/totalEarnings/{carId}
        [HttpGet("totalEarnings/{carId}")]
        public async Task<ActionResult<decimal>> GetTotalEarningsForCar(int carId)
        {
            var total = await _orderRepository.GetTotalEarningsForCarAsync(carId);
            return Ok(total);
        }

        // POST: api/orders
        [HttpPost]
        public async Task<ActionResult> AddOrder(OrderDto orderDto)
        {
            var order = _mapper.Map<DAL.Classes.Order>(orderDto);
            await _orderRepository.AddOrderAsync(order);
            return CreatedAtAction(nameof(GetOrderById), new { id = order.OrderId }, order);
        }

        // PUT: api/orders/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateOrder(int id, OrderDto orderDto)
        {
            if (id != orderDto.OrderId) return BadRequest();
            var order = _mapper.Map<DAL.Classes.Order>(orderDto);
            await _orderRepository.UpdateOrderAsync(order);
            return NoContent();
        }

        // DELETE: api/orders/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteOrder(int id)
        {
            await _orderRepository.DeleteOrderAsync(id);
            return NoContent();
        }
    }
}
