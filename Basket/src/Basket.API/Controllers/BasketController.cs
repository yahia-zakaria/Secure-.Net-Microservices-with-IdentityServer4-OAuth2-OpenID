using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Basket.API.Entities;
using Basket.API.GrpcServices;
using Basket.API.Repository;
using EventBus.Messages.Events;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using static Discount.Grpc.Protos.DiscountProtoService;

namespace Basket.API.Controllers
{
    public class BasketController : BaseController
    {
        private readonly IBasketRepository basketRepo;
        private readonly DiscountService discountService;
        private readonly IMapper mapper;
        private readonly IPublishEndpoint publishEndpoin;
        public BasketController(IBasketRepository basketRepo, DiscountService discountService, IMapper mapper, IPublishEndpoint publishEndpoint)
        {
            this.basketRepo = basketRepo;
            this.discountService = discountService;
            this.mapper = mapper;
            this.publishEndpoin = publishEndpoint;
        }

        [HttpGet("{username}"), ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> GetBasket(string username)
        {
            var basket = await basketRepo.GetBasket(username);
            return Ok(basket ?? new ShoppingCart(username));
        }

        [HttpPost, ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> UpdateBasket(ShoppingCart basket)
        {
            foreach (var item in basket.ShoppingCartItems)
            {
                var coupon = await discountService.GetDiscount(item.ProductName);
                item.Price -= coupon.Amount;
            }
            return Ok(await basketRepo.UpdateBasket(basket));

        }

        [HttpPost("Checkout")]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> Checkout([FromBody] BasketCheckout basketCheckout)
        {
            var basket = await basketRepo.GetBasket(basketCheckout.UserName);
            if(basket is null)
            {
                return BadRequest();
            }

            var basketCheckoutEvent = mapper.Map<BasketCheckoutEvent>(basketCheckout);
            basketCheckoutEvent.TotalPrice = basket.TotalPrice;
            await publishEndpoin.Publish(basketCheckoutEvent);

            await basketRepo.DeleteBasket(basketCheckout.UserName);

            return Accepted();

        }

        [HttpDelete, ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> DeleteBasket(string username)
        {
            await basketRepo.DeleteBasket(username);
            return Ok();
        }
    }
}