using CrudApp.Models;
using System.Data.SqlClient;
using System.Data;
using Microsoft.CodeAnalysis;

namespace CrudApp
{
    public class ProductDAL
    {
        string cs = ConnectionString.Dbcs;
        public List<Product> getAllProduct()
        {
            List<Product> prodList = new List<Product>();

            using (SqlConnection con =new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("GetAllProducts",con);
                cmd.CommandType=CommandType.StoredProcedure;
                con.Open();
                SqlDataReader Reader= cmd.ExecuteReader();

                while (Reader.Read())
                {
                    Product prod = new Product();
                    prod.ProductId = Reader["ProductId"] != DBNull.Value ? Convert.ToInt32(Reader["ProductId"]) : 0;
                    prod.ProductName = Reader["ProductName"] != DBNull.Value ? Reader["ProductName"].ToString() : "";
                    
                    prodList.Add(prod);
                }
            }
            return prodList;
        }
        public void UpdateProduct(Product prod)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("UpdateProduct", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProductId", prod.ProductId);
                cmd.Parameters.AddWithValue("@ProductName", prod.ProductName);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public Product getProdData(int? ProductId)
        {
            Product prod = null;

            using (SqlConnection con = new SqlConnection(cs))
            using (SqlCommand cmd = new SqlCommand("SELECT * FROM Product WHERE ProductId = @ProductId", con))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@ProductId", ProductId ?? (object)DBNull.Value);

                con.Open();

                using (SqlDataReader Reader = cmd.ExecuteReader())
                {
                    if (Reader.Read()) // Only fetch the first product
                    {
                        prod = new Product
                        {
                            ProductId = Reader["ProductId"] != DBNull.Value ? Convert.ToInt32(Reader["ProductId"]) : 0,
                            ProductName = Reader["ProductName"] != DBNull.Value ? Reader["ProductName"].ToString() : ""
                        };
                    }
                }
            }
            return prod;
        }


        public int AddProduct(Product prod)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("InsertProduct", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProductName", prod.ProductName);
                cmd.Parameters.AddWithValue("@CategoryId", prod.CategoryId);

                SqlParameter outputParam = new SqlParameter("@NewProductId", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(outputParam);

                con.Open();
                cmd.ExecuteNonQuery();

                return (int)outputParam.Value; 
            }
        }


        public void DeleteProduct(int productId)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("DeleteProduct", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProductId", productId);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }



    }
}
