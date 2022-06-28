using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspnetRunBasics.Models;
using AspnetRunBasics.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspnetRunBasics
{
    public class ProductModel : PageModel
    {
        private readonly ICatalogService catalogService;
        private readonly IBasketService basketService;

        public ProductModel(ICatalogService catalogService, IBasketService basketService)
        {
            this.catalogService = catalogService ?? throw new ArgumentNullException(nameof(catalogService));
            this.basketService = basketService ?? throw new ArgumentNullException(nameof(basketService));
        }

        public IEnumerable<string> CategoryList { get; set; } = new List<string>();
        public IEnumerable<CatalogModel> ProductList { get; set; } = new List<CatalogModel>();


        [BindProperty(SupportsGet = true)]
        public string SelectedCategory { get; set; }

        public async Task<IActionResult> OnGetAsync(string categoryName)
        {
            ProductList = await catalogService.GetCatalog();
            CategoryList = ProductList.Select(s => s.Category).Distinct();
            if (!string.IsNullOrEmpty(categoryName))
            {
                ProductList = await catalogService.GetCatalogByCategory(categoryName);
                SelectedCategory = CategoryList.FirstOrDefault(category => category == categoryName);
            }
            else
            {
                ProductList = ProductList;
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