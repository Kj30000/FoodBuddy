using System.Collections.ObjectModel;
using FoodBuddy.Models;

namespace FoodBuddy.ViewModels
{
    public class FoodGroupViewModel
    {
        public string GroupName { get; set; } = string.Empty; // Initialize to avoid null
        public ObservableCollection<Product> Products { get; set; } = new ObservableCollection<Product>();


        // Change default to true
        private bool _isExpanded = true;
        public bool IsExpanded
        {
            get => _isExpanded;
            set
            {
                if (_isExpanded != value)
                {
                    _isExpanded = value;
                    Console.WriteLine($"IsExpanded changed for {GroupName}: {_isExpanded}");
                }
            }
        }
    }
}