using CrudApp.Models;
using System.Data.SqlClient;
using System.Data;

namespace CrudApp
{
    public class CategoryDAL
    {
        string cs = ConnectionString.Dbcs;
        public IEnumerable<Category> GetAllCategories()
        {
            var categories = new List<Category>();

            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("GetAllCategories", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        categories.Add(new Category
                        {
                            CategoryId = (int)reader["CategoryId"],
                            CategoryName = reader["CategoryName"].ToString()
                        });
                    }
                }
            }

            return categories;
        }

        public Category GetCategoryById(int categoryId)
        {
            Category category = null;

            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("GetCategoryById", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@CategoryId", categoryId);
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        category = new Category
                        {
                            CategoryId = (int)reader["CategoryId"],
                            CategoryName = reader["CategoryName"].ToString()
                        };
                    }
                }
            }

            return category;
        }

        public void AddCategory(Category category)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("InsertCategory", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@CategoryName", category.CategoryName);

                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 2627) // SQL Server duplicate entry error code
                    {
                        throw new Exception("Category name already exists.");
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }


        public void UpdateCategory(Category category)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("UpdateCategory", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@CategoryId", category.CategoryId);
                cmd.Parameters.AddWithValue("@CategoryName", category.CategoryName);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteCategory(int categoryId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    using (SqlCommand cmd = new SqlCommand("DeleteCategory", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CategoryId", categoryId);

                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting category: " + ex.Message);
            }
        }


    }
}

