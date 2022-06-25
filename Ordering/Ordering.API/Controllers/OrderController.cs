using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Features.Orders.Commands.CheckoutOrder;
using Ordering.Application.Features.Orders.Commands.DeleteOrder;
using Ordering.Application.Features.Orders.Commands.UpdateOrder;
using Ordering.Application.Features.Orders.Queries.GetOrdersList;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Ordering.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IMediator mediatR;

        public OrderController(IMediator mediatR)
        {
            this.mediatR = mediatR ?? throw new ArgumentNullException(nameof(mediatR));
        }

        [HttpGet("{username}")]
        [ProducesResponseType(type: typeof(IEnumerable<OrderDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetOrderByUserName(string username)
        {
            GetOrdersListRequest query = new(username);
            var orders = await mediatR.Send(query);
            return Ok(orders);
        }
        [HttpPost()]
        [ProducesResponseType(type: typeof(int), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> checkoutOrder(CheckoutOrderRequest request)
        {
            var orderId = await mediatR.Send(request);
            return Ok(orderId);
        }
        [HttpPut()]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> Update(UpdateOrderRequest request)
        {
            await mediatR.Send(request);
            return NoContent();
        }

        [HttpDelete()]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> Delete(int id)
        {
            DeleteOrderRequest request = new() { Id = id };
            await mediatR.Send(request);
            return NoContent();
        }
    }
}
