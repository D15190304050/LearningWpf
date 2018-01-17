using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace StoredDatabase
{
    public class StoredDbDataSet
    {
        private readonly string connectionString = StoredDatabase.Properties.Settings.Default.Store;
        //private const string GetProducts = "GetProducts";
        private const string Products = "Products";
        private const string Categories = "Categories";
        private const string GetCategories = "GetCategories";
        private const string CategoryID = "CategoryID";
        private const string CategoryProduct = "CategoryProduct";

        public DataTable GetProducts()
        {
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand("GetProducts", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);

            DataSet ds = new DataSet();
            adapter.Fill(ds, Products);
            return ds.Tables[0];
        }

        public DataSet GetCategoriesAndProducts()
        {
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand("GetProducts", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);

            DataSet ds = new DataSet();
            adapter.Fill(ds, Products);
            cmd.CommandText = GetCategories;
            adapter.Fill(ds, Categories);

            // Set up a relation between these tables (optional).
            DataRelation relCategoryProduct = new DataRelation(CategoryProduct,
                                                               ds.Tables[Categories].Columns[CategoryID],
                                                               ds.Tables[Products].Columns[CategoryID]);
            ds.Relations.Add(relCategoryProduct);

            return ds;
        }
    }
}
