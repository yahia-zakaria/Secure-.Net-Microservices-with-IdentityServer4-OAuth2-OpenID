using System.Net;
using System.Threading.Tasks;
using Basket.API.Entities;
using Basket.API.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Basket.API.Controllers
{
    public class BasketController : BaseController
    {
        private readonly IBasketRepository basketRepo;
        public BasketController(IBasketRepository basketRepo)
        {
            this.basketRepo = basketRepo;
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