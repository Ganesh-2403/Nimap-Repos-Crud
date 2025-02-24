using CrudApp;
using System.Data.SqlClient;
using System.Data;
using CrudApp.Models;

public class ListingDAL
{
    string cs = ConnectionString.Dbcs;

    public IEnumerable<Listing> GetAllProducts()
    {
        var products = new List<Listing>();

        using (SqlConnection con = new SqlConnection(cs))
        {
            SqlCommand cmd = new SqlCommand("GetAllProducts", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            con.Open();
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    products.Add(new Listing
                    {
                        ProductId = Convert.ToInt32(reader["ProductId"]),
                        ProductName = reader["ProductName"].ToString(),
                        CategoryId = Convert.ToInt32(reader["CategoryId"]),
                        CategoryName = reader["CategoryName"].ToString()
                    });
                }
            }
        }
        return products;
    }
    public int GetTotalProductCount()
    {
        using (SqlConnection con = new SqlConnection(cs))
        {
            SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Product", con);
            con.Open();
            return (int)cmd.ExecuteScalar();
        }
    }

    public List<Listing> GetPagedProducts(int page, int pageSize)
    {
        List<Listing> products = new List<Listing>();

        using (SqlConnection con = new SqlConnection(cs))
        {
            SqlCommand cmd = new SqlCommand(@"
            SELECT p.ProductId, p.ProductName, p.CategoryId, c.CategoryName 
            FROM Product p 
            JOIN Category c ON p.CategoryId = c.CategoryId 
            ORDER BY p.ProductId 
            OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY", con);

            cmd.Parameters.AddWithValue("@Offset", (page - 1) * pageSize);
            cmd.Parameters.AddWithValue("@PageSize", pageSize);

            con.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                products.Add(new Listing
                {
                    ProductId = (int)rdr["ProductId"],
                    ProductName = rdr["ProductName"].ToString(),
                    CategoryId = (int)rdr["CategoryId"],
                    CategoryName = rdr["CategoryName"].ToString()
                });
            }
        }
        return products;
    }

}
