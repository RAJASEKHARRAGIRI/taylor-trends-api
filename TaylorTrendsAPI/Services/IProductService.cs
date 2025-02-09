using TaylorTrendsAPI.Models;

namespace TaylorTrendsAPI.Services
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> getAllProducts();
        Task<ProductDetail> getProductById(int productId);
        Task<IEnumerable<Category>> getAllCategories();
        Task<int> AddToCart(AddToCart cart);
        Task<int> UpdateCart(AddToCart cart);
        Task<IEnumerable<CartDetails>> getAllCartDetails();
        Task<int> DeleteProductFromCart(int cardProductId);
    }
}
