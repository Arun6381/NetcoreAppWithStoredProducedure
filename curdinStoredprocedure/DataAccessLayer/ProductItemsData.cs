using Microsoft.Data.SqlClient;
using curdinStoredprocedure.Models;

using System.Data;
using Microsoft.Build.Evaluation;

namespace curdinStoredprocedure.DataAccessLayer
{
    public class ProductItemsData
    {
        private readonly string? _connectionString;

        public ProductItemsData(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

       

        public int InsertProductItem(ProductItems item)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("InsertProductItem", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@ProductName", item.ProductName);
                    command.Parameters.AddWithValue("@Price", item.Price);
                    command.Parameters.AddWithValue("@Description", item.Description);
                    command.Parameters.AddWithValue("@CategoryId", item.CategoryId);

                    connection.Open();
                    var result = command.ExecuteScalar();
                    return Convert.ToInt32(result);
                }
            }
        }

        public void UpdateProductItem(ProductItems item)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("UpdateProductItem", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@Product_Id", item.Product_Id);
                    command.Parameters.AddWithValue("@ProductName", item.ProductName);
                    command.Parameters.AddWithValue("@Price", item.Price);
                    command.Parameters.AddWithValue("@Description", item.Description);
                    command.Parameters.AddWithValue("@CategoryId", item.CategoryId);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteProductItem(int productId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("DeleteProductItem", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@Product_Id", productId);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }
        public List<ProductItems> GetAllProductItems()
        {
            var items = new List<ProductItems>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("GetAllProductItems", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        items.Add(new ProductItems
                        {
                            Product_Id = (int)reader["Product_Id"],
                            ProductName = reader["ProductName"].ToString(),
                            Price = (decimal)reader["Price"],
                            Description = reader["Description"].ToString(),
                            CategoryId = (int)reader["CategoryId"]
                        });
                    }
                }
            }
            return items;
        }
        public ProductItems GetProductItemById(int productId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("GetProductItemById", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@Product_Id", productId);

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();
                        return new ProductItems
                        {
                            Product_Id = (int)reader["Product_Id"],
                            ProductName = reader["ProductName"].ToString(),
                            Price = (decimal)reader["Price"],
                            Description = reader["Description"].ToString(),
                            CategoryId = (int)reader["CategoryId"]
                        };
                    }
                    return null;
                }
            }
        }
        public List<ProductItems> GetProductItemsByCategory(int categoryId)
        {
            var items = new List<ProductItems>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("GetProductItemsByCategory", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@CategoryId", categoryId);

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        items.Add(new ProductItems
                        {
                            Product_Id = (int)reader["Product_Id"],
                            ProductName = reader["ProductName"].ToString(),
                            Price = (decimal)reader["Price"],
                            Description = reader["Description"].ToString(),
                            CategoryId = (int)reader["CategoryId"]
                        });
                    }
                }
            }
            return items;
        }
        public IEnumerable<Productcategory> GetAllProductCategories()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("GetAllProductCategorys", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        var categories = new List<Productcategory>();
                        while (reader.Read())
                        {
                            categories.Add(new Productcategory
                            {
                                CategoryId = (int)reader["CategoryId"],
                                CategoryName = reader["CategoryName"].ToString()
                            });
                        }
                        return categories;
                    }
                }
            }
        }

    }
}
