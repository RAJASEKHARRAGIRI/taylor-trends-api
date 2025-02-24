using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaylorTrendsAPI.Models;
using TaylorTrendsAPI.Services;

namespace TaylorTrendsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost]
        public async Task<IActionResult> GetProducts(ProductFilterRequest productFilterRequest)
        {
            var products = await _productService.getAllProducts(productFilterRequest);
            return Ok(products);
        }

        [HttpGet("productDetails/{productId:int}")]
        public async Task<IActionResult> GetProductById(int productId)
        {
            var products = await _productService.getProductById(productId);
            if(products == null)
            {
                return NotFound();
            }
            return Ok(products);
        }

        
        [HttpGet("categories")]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _productService.getAllCategories();
            return Ok(categories);
        }

        [HttpGet("cartDetails")]
        public async Task<IActionResult> GetCartDetails()
        {
            var cart = await _productService.getAllCartDetails();
            return Ok(cart);
        }

        [HttpPost("AddCart")]
        public async Task<IActionResult> AddCart(AddToCart cart)
        {
            if (cart.ProductId <= 0)
            {
                return BadRequest();
            }

            var response = await _productService.AddToCart(cart);
            return Ok( new
            {
                message = "Successfully Product Added to Cart.",
                id = response
            });
        }

        [HttpPut("UpdateCart")]
        public async Task<IActionResult> UpdateCart(AddToCart cart)
        {
            if (cart.Id <= 0 )
            {
                return BadRequest();
            } 

            var response = await _productService.UpdateCart(cart);
            return Ok(new
            {
                message = "Successfully Product Updated to Cart.",
                id = response
            });
        }

        [HttpDelete("DeleteProductFromCart/{cardProductId:int}")]
        public async Task<IActionResult> DeleteProductFromCart(int cardProductId)
        {
            if (cardProductId <= 0)
            {
                return BadRequest();
            }

            var response = await _productService.DeleteProductFromCart(cardProductId);
            return Ok(new
            {
                message = "Successfully Product Deleted from Cart.",
                id = response
            });
        }
    }
}
