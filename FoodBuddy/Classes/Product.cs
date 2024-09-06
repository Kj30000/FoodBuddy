using SQLite;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FoodBuddy.Models
{
    public class Product : INotifyPropertyChanged
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        private string? productName = string.Empty;
        public string? ProductName
        {
            get => productName;
            set
            {
                if (productName != value)
                {
                    productName = value;
                    OnPropertyChanged();
                }
            }
        }

        public string FoodGroup { get; set; } = string.Empty;
        public string FoodType { get; set; } = string.Empty;

        // New Fields
        public bool PackageType { get; set; } // true for Prepackaged, false for Custom
        public bool QuantityType { get; set; } // true for Ml, false for Grams

        public int Quantity { get; set; } // Integer value for quantity
        public int AverageUseByTime { get; set; } // Days

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
