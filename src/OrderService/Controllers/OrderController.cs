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
        [ProducesResponseType<BadRequestResponse>(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Send([FromBody] Order order)
        {
            _logger.LogInformation("Processing order for {Email} with {LineCount} lines", order.Email, order.Lines.Length);

            await DoSomethingUseless();
            
            await Task.Delay(150); // thinking...

            _logger.LogInformation("Order processed successfully for {Email}", order.Email);

            return Ok(new OrderResponse(42));
        }
        
        private async Task DoSomethingUseless()
        {
            using (var activity = AppActivitySource.Current.StartActivity("UselessActivity"))
            {
                await Task.Delay(Random.Shared.Next(1000, 2000)); // useless work
            }
        }

        public record BadRequestResponse(string Message, BadRequestCode Code);
        public enum BadRequestCode { InvalidProductId, MissingContactInformation, OutOfStock }
        public record OrderResponse(int OrderId);
        public record OrderLine(int Quantity, int ProductId);
        public record Order(OrderLine[] Lines, string Email, string FirstName, string LastName);
    }
}
