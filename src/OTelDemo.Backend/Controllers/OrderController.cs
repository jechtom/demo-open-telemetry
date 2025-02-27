using Microsoft.AspNetCore.Mvc;

namespace OTelDemo.Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly ILogger<OrderController> _logger;

        public OrderController(ILogger<OrderController> logger)
        {
            _logger = logger;
        }

        [HttpPost(Name = "SendOrder")]
        [ProducesResponseType<OrderResponse>(StatusCodes.Status200OK)]
        [ProducesResponseType<BadRequest>(StatusCodes.Status400BadRequest)]
        public IActionResult Send([FromBody]Order order)
        {
            return BadRequest(new BadRequest("I don't like you.", BadRequestCode.OutOfStock));
        }

        public record BadRequest(string Message, BadRequestCode Code);
        public enum BadRequestCode { InvalidProductId, MissingContactInformation, OutOfStock }
        public record OrderResponse(int OrderId);
        public record OrderLine(int Quantity, int ProductId);
        public record Order(OrderLine[] Lines, string Email, string FirstName, string LastName);
    }
}
