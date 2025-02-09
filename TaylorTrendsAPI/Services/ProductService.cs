using System.Data.SqlClient;
using Microsoft.AspNetCore.Http.HttpResults;
using TaylorTrendsAPI.Common;
using TaylorTrendsAPI.Models;

namespace TaylorTrendsAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly DatabaseConfig _databaseConfig;

        public ProductService(DatabaseConfig databaseConfig)
        {
            _databaseConfig = databaseConfig;
        }

        public async Task<int> DeleteProductFromCart(int cardProductId)
        {
            if (cardProductId <= 0)
            {
                return -1;
            }

            using (var conn = _databaseConfig.GetConnection())
            {
                var cmd = new SqlCommand("Delete from CartDetails where Id = @id", conn);
                cmd.Parameters.AddWithValue("@id", cardProductId);
                await conn.OpenAsync();
                return await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task<int> AddToCart(AddToCart cart)
        {
            if(CheckProductExistInCart(cart.ProductId))
            {
                return await UpdateCart(cart);
            }

            using (var conn = _databaseConfig.GetConnection())
            {
                var cmd = new SqlCommand("Insert Into CartDetails (UserId, ProductId, Quantity) Values (@userId, @productId, @quantity)", conn);
                cmd.Parameters.AddWithValue("@userId", cart.UserId);
                cmd.Parameters.AddWithValue("@productId", cart.ProductId);
                cmd.Parameters.AddWithValue("@quantity", cart.Quantity);
                await conn.OpenAsync();
                return await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task<int> UpdateCart(AddToCart cart)
        {
            if (cart.Id <= 0 && !CheckProductExistInCart(cart.ProductId))
            {
                return -1;
            }

            using (var conn = _databaseConfig.GetConnection())
            {
                var cmd = new SqlCommand("Update CartDetails SET UserId = @userId, ProductId = @productId, Quantity =@quantity WHERE Id=@id", conn);
                cmd.Parameters.AddWithValue("@userId", cart.UserId);
                cmd.Parameters.AddWithValue("@productId", cart.ProductId);
                cmd.Parameters.AddWithValue("@quantity", cart.Quantity);
                cmd.Parameters.AddWithValue("@id", cart.Id);
                await conn.OpenAsync();
                return await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task<IEnumerable<CartDetails>> getAllCartDetails()
        {
            var products = new List<CartDetails>();
            using (var conn = _databaseConfig.GetConnection())
            {
                var query = $"SELECT  c.Id As CartId, c.ProductId, c.Quantity, c.Size, p.Name, p.Description, p.Price, p.Discount,p.StockStatus, p.Rating, p.Brand, p.Category,c.CreatedAt," +
                            $"(select Top(1) i.Url from Images i where i.ProductId = c.ProductId) as ImageUrl " +
                            $" FROM CartDetails c INNER JOIN Products p ON p.Id = c.ProductId";
                var cmd = new SqlCommand(query, conn);
                await conn.OpenAsync();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        products.Add(new CartDetails() 
                        { 
                            CartId = Convert.ToInt32(reader["CartId"]),
                            ProductId = Convert.ToInt32(reader["ProductId"]),
                            Quantity = Convert.ToInt32(reader["Quantity"]),
                            Size = reader["Size"].ToString() ?? string.Empty,
                            Name = reader["Name"].ToString() ?? string.Empty,
                            Description = reader["Description"].ToString() ?? string.Empty,
                            Price = Convert.ToDecimal(reader["Price"]),
                            Discount = Convert.ToDecimal(reader["Discount"]),
                            StockStatus = reader["StockStatus"].ToString() ?? string.Empty,
                            Rating = Convert.ToInt32(reader["Rating"]),
                            Category = reader["Category"].ToString() ?? string.Empty,
                            Brand = reader["Brand"].ToString() ?? string.Empty,
                            ImageUrl = reader["ImageUrl"].ToString() ?? string.Empty,
                            CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
                            UserId = 1
                        });
                    }
                }
            }
            return products;
        }

        private bool CheckProductExistInCart(int productId)
        {
            using (var conn = _databaseConfig.GetConnection())
            {
                var cmd = new SqlCommand("Select Count(1) from CartDetails Where ProductId = @productId ", conn);
                cmd.Parameters.AddWithValue("@productId", productId);
                conn.Open();
                int count = (int)cmd.ExecuteScalar();
                return count > 0;
            }
        }

        public async Task<IEnumerable<Product>> getAllProducts()
        {
            var products = new List<Product>();
            using (var conn = _databaseConfig.GetConnection())
            {
                var cmd = new SqlCommand("Select * from Products Order by ID asc", conn);
                await conn.OpenAsync();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while(await reader.ReadAsync())
                    {
                        products.Add(MapToProducts(reader));
                    }
                }
            }
            return products;
        }

        public async Task<ProductDetail> getProductById(int productId)
        {
            var productDetails = new List<ProductDetail>(); //{ImageId=1, Url="" ,Brand ="", Category="", Id =1, Images = [], Name="", StockStatus=""};
            var products = new List<Product>();
            using (var conn = _databaseConfig.GetConnection())
            {
                var cmd = new SqlCommand($"select i.Id as ImageId, i.Url as UrlData, p.Id as PId, * from products p inner join Images i on i.ProductId = p.ID where p.ID = {productId} Order by p.ID asc", conn);
                await conn.OpenAsync();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        productDetails.Add(MapToProductDetails(reader));
                    }
                }
            }

            if(productDetails.Count == 0)
            {
                return null;
            }

            var product = productDetails
            .GroupBy(p => p.Id)
            .Select(group => new ProductDetail
            {
                Id = group.First().Id,
                Name = group.First().Name,
                Description = group.First().Description,
                Price = group.First().Price,
                Discount = group.First().Discount,
                StockStatus = group.First().StockStatus,
                Rating = group.First().Rating,
                ReviewsCount = group.First().ReviewsCount,
                Category = group.First().Category,
                Brand = group.First().Brand,
                Images = group
                    .Select(p => new Image { Id = p.ImageId, Url = p.Url ?? string.Empty })
                    .ToList(),
                Tags = group.First().Tags,
                CreatedAt = group.First().CreatedAt,
                UpdatedAt = group.First().UpdatedAt
            }).ToList()[0];

            return product;
        }

        private ProductDetail MapToProductDetails(SqlDataReader reader)
        {
            int productId = Convert.ToInt32(reader["PId"]);
            return new ProductDetail
            {
                Id = productId,
                Name = reader["Name"].ToString() ?? string.Empty,
                Description = reader["Description"].ToString() ?? string.Empty,
                Price = Convert.ToDecimal(reader["Price"]),
                Discount = Convert.ToDecimal(reader["Discount"]),
                StockStatus = reader["StockStatus"].ToString() ?? string.Empty,
                Rating = Convert.ToInt32(reader["Rating"]),
                ReviewsCount = Convert.ToInt32(reader["ReviewsCount"]),
                Category = reader["Category"].ToString() ?? string.Empty,
                Brand = reader["Brand"].ToString() ?? string.Empty,
                Tags = reader["Tags"].ToString()?.Split(',').Select(t => t.Trim()).ToList() ?? new List<string>(),
                ImageId = Convert.ToInt32(reader["ImageId"]),
                Images = [],
                Url = reader["UrlData"].ToString() ?? string.Empty,
                CreatedAt = Convert.ToDateTime(reader["CreatedAt"])
            };
        }


        private Product MapToProducts(SqlDataReader reader)
        {
            int productId = Convert.ToInt32(reader["ID"]);
            return new Product
            {
                Id = productId,
                Name = reader["Name"].ToString() ?? string.Empty,
                Description = reader["Description"].ToString() ?? string.Empty,
                Price = Convert.ToDecimal(reader["Price"]),
                Discount = Convert.ToDecimal(reader["Discount"]),
                StockStatus = reader["StockStatus"].ToString() ?? string.Empty,
                Rating = Convert.ToInt32(reader["Rating"]),
                ReviewsCount = Convert.ToInt32(reader["ReviewsCount"]),
                Category = reader["Category"].ToString() ?? string.Empty,
                Brand = reader["Brand"].ToString() ?? string.Empty,
                Tags = reader["Tags"].ToString()?.Split(',').Select(t => t.Trim()).ToList() ?? new List<string>(),
                Images = getAllImagesById(productId),
                CreatedAt = Convert.ToDateTime(reader["CreatedAt"])
            };
        }

        public List<string> getAllImagesById(int productId)
        {
            var images = new List<string>();
            using (var conn = _databaseConfig.GetConnection())
            {
                var cmd = new SqlCommand($"Select Url from Images where ProductId= {productId}", conn);
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var value = reader["Url"].ToString();
                        if (value != null)
                        {
                            images.Add(value);
                        }
                    }
                }
            }
            return images;
        }

        public async Task<IEnumerable<Category>> getAllCategories()
        {
            var categories = new List<Category>();
            using (var conn = _databaseConfig.GetConnection())
            {
                var cmd = new SqlCommand("Select * from Categories Order by Name asc", conn);
                await conn.OpenAsync();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        categories.Add(MapToCategories(reader));
                    }
                }
            }
            return categories;
        }

        private Category MapToCategories(SqlDataReader reader)
        {
            return new Category
            {
                Id = Convert.ToInt32(reader["ID"]),
                Name = reader["Name"].ToString() ?? string.Empty,
                Code = reader["Code"].ToString() ?? string.Empty,
                ImageUrl = reader["ImageUrl"].ToString() ?? string.Empty,
                CreatedAt = Convert.ToDateTime(reader["CreatedAt"])
            };
        }


    }
}
