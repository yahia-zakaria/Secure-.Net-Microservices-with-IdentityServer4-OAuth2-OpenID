using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AspnetRunBasics.Models;
using AspnetRunBasics.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspnetRunBasics.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ICatalogService catalogService;
        private readonly IBasketService basketService;

        public IndexModel(ICatalogService catalogService, IBasketService basketService)
        {
            this.catalogService = catalogService ?? throw new ArgumentNullException(nameof(catalogService));
            this.basketService = basketService ?? throw new ArgumentNullException(nameof(basketService));
        }

        public IEnumerable<CatalogModel> ProductList { get; set; } = new List<CatalogModel>();

        public async Task<IActionResult> OnGetAsync()
        {
            ProductList = await catalogService.GetCatalog();
            return Page();
        }

        public async Task<IActionResult> OnPostAddToCartAsync(string productId)
        {
            var product = await catalogService.GetCatalog(productId);

            var username = "yahia";
            var basket = await basketService.GetBasket(username);
            BasketItemModel item = new()
            {
                ProductId = product.Id,
                ProductName = product.Name,
                Price = product.Price,
                Quantity = 1,
                Color = "Black"
            };
            basket.shoppingCartItems.Add(item);

            var basketUpdated = await basketService.UpdateBasket(basket);
            return RedirectToPage("Cart");
        }
    }
}
