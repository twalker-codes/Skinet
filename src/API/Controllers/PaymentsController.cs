using System;
using API.Extensions;
using API.SignalR;
using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Stripe;

namespace API.Controllers;

public class PaymentsController(IPaymentService paymentService, 
    IUnitOfWork unitOfWork, ILogger<PaymentsController> logger, 
    IConfiguration config, IHubContext<NotificationHub> hubContext): BaseApiController
{
    private readonly string _whSecret = config["StripeSettings:WhSecret"]!;

    [Authorize]
    [HttpPost("{cartId}")]
    public async Task<ActionResult<ShoppingCart>> CreateOrUpdatePaymentIntent(string cartId)
    {
        var cart = await paymentService.CreateOrUpdatePaymentIntent(cartId);
        if (cart == null) return BadRequest("Problem with your cart");
        return Ok(cart);
    }
    
    [HttpGet("delivery-methods")]
    public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethods()
    {
        return Ok(await unitOfWork.Repository<DeliveryMethod>().ListAllAsync());
    }

    [HttpPost("webhook")]
    public async Task<IActionResult> StripeWebHook()
    {
        var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
        try
        {
            var stripeEvent =  ConstructStripEvent(json);
            if (stripeEvent.Data.Object is not PaymentIntent paymentIntent) return BadRequest("Invalid payment intent");
            if (stripeEvent.Type == "payment_intent.succeeded")
            await HandlePaymentIntentSucceeded(paymentIntent);
            return Ok();
        }
        catch (StripeException ex)
        {
            logger.LogError(ex, "Error processing stripe webhook");
            return StatusCode(StatusCodes.Status500InternalServerError,"Error processing stripe webhook");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unexpected error occurred");
            return StatusCode(StatusCodes.Status500InternalServerError,"An unexpected error occurred");
        }
    }

    private async Task HandlePaymentIntentSucceeded(PaymentIntent paymentIntent)
    {
        if (paymentIntent.Status == "succeeded")
        {
            var spec = new OrderSpecification(paymentIntent.Id, true);
            var order = await unitOfWork.Repository<Order>().GetEntityWithSpec(spec)
                ?? throw new Exception("Order not found");

            if ((long)order.GetTotal() * 100 != paymentIntent.Amount)
            {
               order.Status = OrderStatus.PaymentMismatch;
            }
            else
            {
                order.Status = OrderStatus.PaymentReceived;
            }
            await unitOfWork.Complete();
            var connectionId = NotificationHub.GetConnectionIdByEmail(order.BuyerEmail);
            if (!string.IsNullOrEmpty(connectionId))
            {
                await hubContext.Clients.Client(connectionId)
                .SendAsync("OrderCompleteNotification", order.ToDto());
            }
        } 
    }

    private Event ConstructStripEvent(string json)
    {
        try
        {
            return EventUtility.ConstructEvent(json, Request.Headers["Stripe-Signature"], _whSecret);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to construct stripe event");
            throw new StripeException("Invalid signature");
        }
    }
}
