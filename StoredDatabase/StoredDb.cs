using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace StoredDatabase
{
    public class StoredDb
    {
        private readonly string connectionString = StoredDatabase.Properties.Settings.Default.Store;

        public Product GetProduct(int ID)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand("GetProductByID", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ProductID", ID);

            try
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.SingleRow);
                if (reader.Read())
                {
                    // Create a Product object that wraps current record.
                    Product product = new Product((string)reader["ModelNumber"],
                                                  (string)reader["ModelName"],
                                                  (decimal)reader["UnitCost"],
                                                  (string)reader["Description"],
                                                  (int)reader["CategoryID"],
                                                  (string)reader["CategoryName"],
                                                  (string)reader["CategoryImage"]);
                    return product;
                }
                else
                    return null;
            }
            finally
            {
                conn.Close();
            }
        }

        public ICollection<Product> GetProducts()
        {
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand("GetProducts", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            ObservableCollection<Product> products = new ObservableCollection<Product>();
            try
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    // Create a Product object that wraps the current record.
                    Product product = new Product((string)reader["ModelNumber"],
                                                  (string)reader["ModelName"],
                                                  (decimal)reader["UnitCost"],
                                                  (string)reader["Description"],
                                                  (int)reader["CategoryID"],
                                                  (string)reader["CategoryName"],
                                                  (string)reader["ProductImage"]);

                    // Add to collection.
                    products.Add(product);
                }
            }
            finally
            {
                conn.Close();
            }

            return products;
        }

        public ICollection<Product> GetProductsSlow()
        {
            System.Threading.Thread.Sleep(TimeSpan.FromSeconds(5));
            return GetProducts();
        }

        public ICollection<Product> GetProductsFilteredWithLinq(decimal minCost)
        {
            // Get the full list of products.
            ICollection<Product> products = GetProducts();

            // Create a second collection with matching products.
            IEnumerable<Product> matches =
                from product in products
                where product.UnitCost >= minCost
                select product;

            return new ObservableCollection<Product>(matches.ToList());
        }

        public ICollection<Category> GetCategoryAndProducts()
        {
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand("GetProducts", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);

            DataSet ds = new DataSet();
            adapter.Fill(ds, "Products");
            cmd.CommandText = "GetCategories";
            adapter.Fill(ds, "Categories");

            // Set up a relation between these tables (optional).
            DataRelation relCategoryProduct = new DataRelation("CategoryProduct",
                                                               ds.Tables["Categories"].Columns["CategoryID"],
                                                               ds.Tables["Products"].Columns["CategoryID"]);
            ds.Relations.Add(relCategoryProduct);

            ObservableCollection<Category> categories = new ObservableCollection<Category>();
            foreach (DataRow categoryRow in ds.Tables["Categories"].Rows)
            {
                ObservableCollection<Product> products = new ObservableCollection<Product>();
                foreach (DataRow productRow in categoryRow.GetChildRows(relCategoryProduct))
                {
                    products.Add(new Product(productRow["ModelNumber"].ToString(),
                                             productRow["ModelName"].ToString(),
                                             (decimal)(productRow["UnitCost"]),
                                             productRow["Description"].ToString()));
                }
                categories.Add(new Category(categoryRow["CategoryName"].ToString(), products));
            }

            return categories;
        }

        public ICollection<Category> GetCategories()
        {
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand("GetCategories", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            ObservableCollection<Category> categories = new ObservableCollection<Category>();
            try
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    // Create a Category object that wraps the current record.
                    Category category = new Category((string)(reader["CategoryName"]),
                                                     (int)(reader["CategoryID"]));

                    // Add to collection.
                    categories.Add(category);
                }
            }
            finally
            {
                conn.Close();
            }

            return categories;
        }
    }
}
