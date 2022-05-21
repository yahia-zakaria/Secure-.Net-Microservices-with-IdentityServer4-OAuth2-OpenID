using Discount.API.Entities;
using Discount.API.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Discount.API.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class DiscountController : Controller
    {
        private readonly IDiscountRepository repository;

        public DiscountController(IDiscountRepository repository)
        {
            this.repository = repository;
        }
        [HttpGet("{productName}")]
        [ProducesResponseType(200, Type = typeof(Coupon))]
        public async Task<ActionResult<Coupon>> GetDiscount(string productName)
        {
            return Ok(await repository.GetDiscount(productName));
        }
        [HttpPost()]
        [ProducesResponseType(200, Type = typeof(bool))]
        public async Task<ActionResult<bool>> CreateDiscount(Coupon coupon)
        {
            return Ok(await repository.CreateDiscount(coupon));
        }
        [HttpPut()]
        [ProducesResponseType(200, Type = typeof(bool))]
        public async Task<ActionResult<bool>> UpdateDiscount(Coupon coupon)
        {
            return Ok(await repository.UpdateDiscount(coupon));
        }
        [HttpDelete("{productName}")]
        [ProducesResponseType(200, Type = typeof(bool))]
        public async Task<ActionResult<bool>> DeleteDiscount(string productName)
        {
            return Ok(await repository.DeleteDiscount(productName));
        }
    }
}
