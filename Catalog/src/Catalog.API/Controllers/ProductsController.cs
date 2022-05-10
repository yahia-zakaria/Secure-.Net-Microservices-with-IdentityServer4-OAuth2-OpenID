using Catalog.API.Entities;
using Catalog.API.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
    public class ProductsController : BaseController
    {
        private readonly IProductRepository repository;
        private readonly ILogger<ProductsController> logger;
        public ProductsController(IProductRepository repository, ILogger<ProductsController> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Product>))]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return Ok(await repository.GetProducts());
        }
        [HttpGet("{id:length(24)}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Product))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Product>> GetProductById(string id)
        {
            var product = await repository.GetProductById(id);
            if (product == null)
            {
                logger.LogWarning($"The product with id: {id}, not found");
                return NotFound();
            }
            return Ok(product);
        }
        [HttpGet("GetProductByCategory/{category}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Product>))]
        public async Task<ActionResult<Product>> GetProductByCategory(string category)
        {
            var product = await repository.GetProductsByCategory(category);
            if (product == null)
            {
                logger.LogWarning($"The product with category: {category}, not found");
                return NotFound();
            }
            return Ok(product);
        }
        [HttpGet("{name}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Product))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Product>> GetProductByName(string name)
        {
            var product = await repository.GetProductByName(name);
            if (product == null)
            {
                logger.LogWarning($"The product with name: {name}, not found");
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPost()]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Product))]
        public async Task<ActionResult> Create(Product product)
        {
            await repository.CreateProduct(product);
            return CreatedAtAction("GetProductById", new { id = product.Id }, product);
        }

        [HttpPut()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Update(Product product)
        {
            var existingProduct = await repository.GetProductById(product.Id);
            if (existingProduct == null)
            {
                logger.LogWarning($"The product with id: {product.Id}, not found");
                return BadRequest();
            }
            return Ok(await repository.UpdateProduct(product));
        }
        [HttpDelete("{id:length(24)}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Delete(string id)
        {
            var existingProduct = await repository.GetProductById(id);
            if (existingProduct == null)
            {
                logger.LogWarning($"The product with id: {id}, not found");
                return BadRequest();
            }
            return Ok(await repository.DeleteProduct(id));
        }

    }
}