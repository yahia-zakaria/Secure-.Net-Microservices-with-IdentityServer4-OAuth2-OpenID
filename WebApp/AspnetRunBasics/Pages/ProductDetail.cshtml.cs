using System;
using System.Threading.Tasks;
using AspnetRunBasics.Models;
using AspnetRunBasics.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspnetRunBasics
{
    public class ProductDetailModel : PageModel
    {
        private readonly ICatalogService catalogService;
        private readonly IBasketService basketService;

        public ProductDetailModel(ICatalogService catalogService, IBasketService basketService)
        {
            this.catalogService = catalogService ?? throw new ArgumentNullException(nameof(catalogService));
            this.basketService = basketService ?? throw new ArgumentNullException(nameof(basketService));
        }

        public CatalogModel Product { get; set; }

        [BindProperty]
        public string Color { get; set; }

        [BindProperty]
        public int Quantity { get; set; }

        public async Task<IActionResult> OnGetAsync(string productId)
        {
            if (string.IsNullOrEmpty(productId))
            {
                return NotFound();
            }

            Product = await catalogService.GetCatalog(productId);
            if (Product == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAddToCartAsync(string productId)
        {
            //if (!User.Identity.IsAuthenticated)
            //    return RedirectToPage("./Account/Login", new { area = "Identity" });

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