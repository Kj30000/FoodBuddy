using SQLite; // Import SQLite package for database handling
using System.ComponentModel; // Required for data binding and property change notifications
using System.Runtime.CompilerServices; // Required for CallerMemberName, which helps track property changes

namespace FoodBuddy.Models // Namespace for organizing related classes
{
    // AppSettings class to store application settings like 'ProductSortingLayers'
    public class AppSettings : INotifyPropertyChanged // Implements INotifyPropertyChanged to enable data binding in the UI
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; } // Primary key, auto-incrementing. Every record will have a unique ID

        private int productSortingLayers; // Field to store the value for Product Sorting Layers (2 or 3)

        public int ProductSortingLayers
        {
            get => productSortingLayers; // Getter to return the current value of productSortingLayers
            set
            {
                if (productSortingLayers != value) // Only update if the value has changed
                {
                    productSortingLayers = value; // Update the value
                    OnPropertyChanged(); // Notify the UI that the property has changed
                }
            }
        }

        // Event that is triggered when a property changes, to notify the UI
        public event PropertyChangedEventHandler? PropertyChanged;

        // Method to raise the PropertyChanged event. CallerMemberName automatically uses the name of the property calling this method.
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); // Notify that a specific property has changed
        }
    }
}
