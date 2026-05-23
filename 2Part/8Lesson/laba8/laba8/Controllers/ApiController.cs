using laba8.Data;
using laba8.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace laba8.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly PizzaDBContext _context;

        public ApiController(PizzaDBContext context)
        {
            _context = context;
        }

        [HttpGet("getAddress")]
        public async Task<IActionResult> GetAddress(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone) || !Regex.IsMatch(phone, @"^[\d\-+]{10,}$"))
                return BadRequest("Неверный формат телефона");

            if (string.IsNullOrWhiteSpace(phone))
                return Ok("");

            var customer = await _context.Customers
                .FirstOrDefaultAsync(c => c.Phone == phone);

            return Ok(customer?.Address ?? "");
        }

        [HttpPost("placeOrder")]
        public async Task<IActionResult> PlaceOrder([FromBody] OrderDto orderDto)
        {
            var customer = await _context.Customers
                .FirstOrDefaultAsync(c => c.Phone == orderDto.Phone);

            if (customer == null)
            {
                customer = new Customer
                {
                    Phone = orderDto.Phone,
                    Address = orderDto.DeliveryAddress
                };
                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();
            }

            var order = new Order
            {
                CustomerId = customer.Id,
                OrderDetails = orderDto.OrderDetails,
                DeliveryAddress = orderDto.DeliveryAddress,
                OrderDate = System.DateTime.Now
            };
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return Ok("Заказ принят");
        }
    }

    public class OrderDto
    {
        public string Phone { get; set; }
        public string OrderDetails { get; set; }
        public string DeliveryAddress { get; set; }
    }
}