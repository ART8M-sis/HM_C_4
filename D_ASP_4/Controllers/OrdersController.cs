using D_ASP_4.Models;
using Microsoft.AspNetCore.Mvc;

namespace D_ASP_4.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private static readonly List<Order> _orders = new()
        {
            new Order { Id = 1, Number = "ORD001", Total = 100.50m, CreatedAt = DateTime.Now },
            new Order { Id = 2, Number = "ORD002", Total = 200.75m, CreatedAt = DateTime.Now },
            new Order { Id = 3, Number = "ORD003", Total = 150.25m, CreatedAt = DateTime.Now }

        };

        [HttpGet]
        public ActionResult<IEnumerable<Order>> GetAll() => Ok(_orders);

        [HttpGet("{id:int}")]
        public ActionResult<Order> GetById(int id)
        {
            var order = _orders.FirstOrDefault(o => o.Id == id);
            if (order == null)
            {
                return NotFound("Order not found");
            }
            return Ok(order);
        }

        [HttpPost]
        public ActionResult<Order> Create([FromBody] OrderDto orderDto)
        {
            if (string.IsNullOrWhiteSpace(orderDto.Number)) return BadRequest("Number is required and cannot be empty.");

            if (orderDto.Total is null) return BadRequest("Total is required.");
            var order = new Order
            {
                Id = _orders.Max(o => o.Id) + 1,
                Number = orderDto.Number,
                Total = orderDto.Total.Value,
                CreatedAt = DateTime.Now
            };
            _orders.Add(order);
            return CreatedAtAction(nameof(GetById), new { id = order.Id }, order);
        }

        [HttpPut("{id:int}")]
        public ActionResult<Order> Update(int id, [FromBody] OrderDto orderDto)
        {
            var order = _orders.FirstOrDefault(o => o.Id == id);
            if (order == null) return NotFound("Order not found");

            if (string.IsNullOrWhiteSpace(orderDto.Number)) return BadRequest("Number is required and cannot be empty.");

            if (orderDto.Total is null) return BadRequest("Total is required.");

            order.Number = orderDto.Number;
            order.Total = orderDto.Total.Value;

            return Ok(order);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var order = _orders.FirstOrDefault(o => o.Id == id);
            if (order == null) return NotFound("Order not found");
            _orders.Remove(order);
            return NoContent();
        }

        [HttpGet("search")]
        public ActionResult<IEnumerable<Order>> Search(
        [FromQuery] string? number,
        [FromQuery] decimal? minTotal)
        {
            if (string.IsNullOrWhiteSpace(number)) return BadRequest("Search term is required.");
            

            var orders = _orders.Where(o => o.Number.Contains(number, StringComparison.OrdinalIgnoreCase)).ToList();
            
            if (minTotal.HasValue) orders = orders.Where(o => o.Total >= minTotal.Value).ToList();


            return Ok(orders);
        }
    }
}
