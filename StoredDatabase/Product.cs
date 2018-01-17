using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace StoredDatabase
{
    public class Product : INotifyPropertyChanged
    {
        private string modelNumber;
        public string ModelNumber
        {
            get { return modelNumber; }
            set
            {
                modelNumber = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ModelNumber"));
            }
        }

        private string modelName;
        public string ModelName
        {
            get { return modelName; }
            set
            {
                modelName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ModelName"));
            }
        }

        private decimal unitCost;
        public decimal UnitCost
        {
            get { return unitCost; }
            set
            {
                unitCost = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UnitCost"));
            }
        }

        private string description;
        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Description"));
            }
        }

        private string categoryName;
        public string CategoryName
        {
            get { return categoryName; }
            set
            {
                categoryName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CategoryName"));
            }
        }

        // For DataGridComboBoxColumn example.
        private int categoryID;
        public int CategoryID
        {
            get { return categoryID; }
            set
            {
                categoryID = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CategoryID"));
            }
        }

        private string productImagePath;
        public string ProductImagePath
        {
            get { return productImagePath; }
            set { productImagePath = value; }
        }

        public Product(string modelNumber, string modelName, decimal unitCost, string description)
        {
            this.ModelNumber = modelNumber;
            this.ModelName = modelName;
            this.UnitCost = unitCost;
            this.Description = description;
            dateAdded = DateTime.Today;
        }

        public Product(string modelNumber, string modelName, decimal unitCost, string description, string productImagePath)
            : this(modelNumber, modelName, unitCost, description)
        {
            this.ProductImagePath = productImagePath;
        }

        public Product(string modelNumber, string modelName, decimal unitCost, string description, int categoryID, string categoryName, string productImagePath)
            : this(modelNumber, modelName, unitCost, description)
        {
            this.CategoryName = categoryName;
            this.CategoryID = categoryID;
            this.ProductImagePath = productImagePath;
        }

        public override string ToString()
        {
            return this.ModelName + " (" + this.ModelNumber + ")";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }

        private DateTime dateAdded;
        public DateTime DateAdded
        {
            get { return dateAdded; }
            set { dateAdded = value; }
        }
    }
}
