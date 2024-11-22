using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace Garage_Payment_API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        [HttpPost("create-intent")]
        public async Task<IActionResult> CreatePaymentIntent([FromBody] PaymentRequest paymentRequest)
        {
            try
            {
                StripeConfiguration.ApiKey = "sk_test_51QFCeaPPa7Ug3bKuqdrWZlAcf2jbcG1fs9kea62j6YcAgSR1M10oVMvKIV8FXgRcinO9PvBHsi27LblJporyBFsz00hVVwxPap"; // Replace with your secret key

                // Convert dollars to cents for Stripe
                long amountInCents = (long)(paymentRequest.Amount * 100);

                var options = new PaymentIntentCreateOptions
                {
                    Amount = amountInCents,
                    Currency = "usd",
                    PaymentMethodTypes = new List<string> { "card" }
                };

                var service = new PaymentIntentService();
                var paymentIntent = await service.CreateAsync(options);

                return Ok(paymentIntent);
            }

            catch (StripeException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}

public class PaymentRequest
{
    public long Amount { get; set; } // Amount in cents
}
