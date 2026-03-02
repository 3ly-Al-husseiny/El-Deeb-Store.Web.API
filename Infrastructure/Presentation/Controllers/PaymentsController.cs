using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Abstraction;
using Shared.DTOs.BasketModule;

namespace Presentation.Controllers;

[Authorize]
public class PaymentsController(IServiceManager _serviceManager) : ApiController
{
    [HttpPost("{basketId}")]
    public async Task<ActionResult<BasketDto>> CreateOrUpdatePaymentIntent(string basketId) 
        => Ok(await _serviceManager.PaymentService.CreateOrUpdatePaymentIntentAsync(basketId));
}