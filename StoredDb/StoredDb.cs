using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace StoreDatabase
{
    public class StoreDb
    {
        public Product GetProduct(int ID)
        {
            DataSet ds = StoreDbDataSet.ReadDataSet();
            DataRow productRow = ds.Tables["Products"].Select("ProductID = " + ID.ToString())[0];
            Product product = new Product((string)productRow["ModelNumber"],
                                          (string)productRow["ModelName"],
                                          (decimal)productRow["UnitCost"],
                                          (string)productRow["Description"],
                                          (int)productRow["CategoryID"],
                                          (string)productRow["CategoryName"],
                                          (string)productRow["ProductImage"]);
            return product;
        }

        public ICollection<Product> GetProducts()
        {
            DataSet ds = StoreDbDataSet.ReadDataSet();

            ObservableCollection<Product> products = new ObservableCollection<Product>();
            foreach (DataRow productRow in ds.Tables["Products"].Rows)
            {
                products.Add(new Product((string)productRow["ModelNumber"],
                                         (string)productRow["ModelName"],
                                         (decimal)productRow["UnitCost"],
                                         (string)productRow["Description"],
                                         (int)productRow["CategoryID"],
                                         (string)productRow["CategoryName"],
                                         (string)productRow["ProductImage"]));
            }
            return products;
        }

        public ICollection<Category> GetCategoriesAndProducts()
        {
            DataSet ds = StoreDbDataSet.ReadDataSet();
            DataRelation relCategoryProduct = ds.Relations[0];

            ObservableCollection<Category> categories = new ObservableCollection<Category>();
            foreach (DataRow categoryRow in ds.Tables["Categories"].Rows)
            {
                ObservableCollection<Product> products = new ObservableCollection<Product>();
                foreach (DataRow productRow in categoryRow.GetChildRows(relCategoryProduct))
                {
                    products.Add(new Product((string)productRow["ModelNumber"],
                                             (string)productRow["ModelName"],
                                             (decimal)productRow["UnitCost"],
                                             (string)productRow["Description"]));
                }
                categories.Add(new Category(categoryRow["CategoryName"].ToString(), products));
            }
            return categories;
        }

        public ICollection<Product> GetProductsFillteredWithLinq(decimal minCost)
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

        public ICollection<Category> GetCategories()
        {
            DataSet ds = StoreDbDataSet.ReadDataSet();

            ObservableCollection<Category> categories = new ObservableCollection<Category>();
            foreach (DataRow categoryRow in ds.Tables["Categories"].Rows)
                categories.Add(new Category((string)categoryRow["CategoryName"], (int)categoryRow["CategoryID"]));
            return categories;
        }
    }
}