using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Abstraction;
using Shared.DTOs.BasketModule;

namespace Presentation.Controllers;

[Authorize]
public class PaymentsController(IServiceManager _serviceManager) : ApiController
{
    [HttpPost("{basketId}")]
    [AllowAnonymous]
    public async Task<ActionResult<BasketDto>> CreateOrUpdatePaymentIntent(string basketId) 
        => Ok(await _serviceManager.PaymentService.CreateOrUpdatePaymentIntentAsync(basketId));
    
    [HttpPost("webhook")]
    [AllowAnonymous]
    public async Task<ActionResult> WebHook()
    {
        var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
        var signatureHeader = Request.Headers["Stripe-Signature"];
        await _serviceManager.PaymentService.UpdatePaymentStatusAsync(json, signatureHeader);
        return new EmptyResult();
    }
}
