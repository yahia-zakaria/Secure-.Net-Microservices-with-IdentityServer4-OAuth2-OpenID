using System;
using System.Linq;
using System.Threading.Tasks;
using AspnetRunBasics.Models;
using AspnetRunBasics.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspnetRunBasics
{
    public class CartModel : PageModel
    {
        private readonly IBasketService basketService;

        public CartModel(IBasketService basketService)
        {
            this.basketService = basketService ?? throw new ArgumentNullException(nameof(basketService));
        }

        public BasketModel Cart { get; set; } = new();        

        public async Task<IActionResult> OnGetAsync()
        {
            Cart = await basketService.GetBasket("yahia");            

            return Page();
        }

        public async Task<IActionResult> OnPostRemoveToCartAsync(string productId)
        {
            var basket = await basketService.GetBasket("yahia");
            basket.shoppingCartItems.Remove(
                basket.shoppingCartItems.Single(s=>s.ProductId == productId)
                );

            var basketUpdated = await basketService.UpdateBasket(basket);
            return RedirectToPage();
        }
    }
}