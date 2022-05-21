using System.Net;
using System.Threading.Tasks;
using Basket.API.Entities;
using Basket.API.GrpcServices;
using Basket.API.Repository;
using Microsoft.AspNetCore.Mvc;
using static Discount.Grpc.Protos.DiscountProtoService;

namespace Basket.API.Controllers
{
    public class BasketController : BaseController
    {
        private readonly IBasketRepository basketRepo;
        private readonly DiscountService discountService;
        public BasketController(IBasketRepository basketRepo, DiscountService discountService)
        {
            this.basketRepo = basketRepo;
            this.discountService = discountService;
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

        [HttpDelete, ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> DeleteBasket(string username)
        {
            await basketRepo.DeleteBasket(username);
            return Ok();
        }
    }
}