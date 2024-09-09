using FoodBuddy.Models;
using System.Collections.ObjectModel;
using System.Linq;

namespace FoodBuddy.Pages
{
    public partial class AddProductForm : ContentPage
    {
        // Default food groups
        private readonly List<string> defaultFoodGroups = new List<string>
        {
            "Vegetables", "Fruits", "Meat", "Dairy", "Grains", "Beverages", "Add New"  // "Add New" added here
        };

        // Use ObservableCollection for dynamic updates
        public ObservableCollection<string> AllFoodGroups { get; set; } = new ObservableCollection<string>();

        public AddProductForm()
        {
            InitializeComponent();
            LoadFoodGroupsFromDatabase();
            BindingContext = this;  // Set BindingContext to use data binding
        }

        // Load food groups from the database and combine with default ones
        private async void LoadFoodGroupsFromDatabase()
        {
            var products = await App.Database.GetProductsAsync();
            var foodGroupsFromDatabase = products.Select(p => p.FoodGroup).Distinct();

            // Combine default and database food groups, excluding "Add New" for now
            var combinedGroups = defaultFoodGroups
                .Where(fg => fg != "Add New") // Exclude "Add New" from initial sort
                .Concat(foodGroupsFromDatabase)
                .Distinct()
                .OrderBy(fg => fg) // Sort alphabetically
                .ToList();

            // Add "Add New" to the end of the list
            combinedGroups.Add("Add New");

            // Clear existing items and add new sorted ones
            AllFoodGroups.Clear();
            foreach (var group in combinedGroups)
            {
                AllFoodGroups.Add(group);
            }
        }

        // Handle Picker selection
        private void OnFoodGroupSelected(object sender, EventArgs e)
        {
            var selectedFoodGroup = FoodGroupPicker.SelectedItem as string;

            if (selectedFoodGroup == "Add New")
            {
                CustomFoodGroupEntry.IsVisible = true;  // Show custom entry
            }
            else
            {
                CustomFoodGroupEntry.IsVisible = false;  // Hide custom entry
            }
        }

        private async void OnAddProductClicked(object sender, EventArgs e)
        {
            string productName = ProductNameEntry.Text;
            string foodGroup = FoodGroupPicker.SelectedItem as string;

            // If "Add New" is selected, use the value from the custom entry field
            if (foodGroup == "Add New")
            {
                foodGroup = CustomFoodGroupEntry.Text;  // Use custom food group
            }

            string foodType = FoodTypeEntry.Text;
            bool packageType = PrepackagedRadioButton.IsChecked;
            bool quantityType = MlRadioButton.IsChecked;

            if (string.IsNullOrWhiteSpace(productName) || string.IsNullOrWhiteSpace(foodGroup) || string.IsNullOrWhiteSpace(foodType))
            {
                await DisplayAlert("Missing Information", "Please fill out all fields.", "OK");
                return;
            }

            bool isQuantityParsed = int.TryParse(QuantityEntry.Text, out int quantity);
            if (!isQuantityParsed)
            {
                await DisplayAlert("Invalid Input", "Please enter a valid quantity.", "OK");
                return;
            }

            bool isAverageUseByTimeParsed = int.TryParse(AverageUseByTimeEntry.Text, out int averageUseByTime);
            if (!isAverageUseByTimeParsed)
            {
                await DisplayAlert("Invalid Input", "Please enter a valid average use by time.", "OK");
                return;
            }

            var newProduct = new Product
            {
                ProductName = productName,
                FoodGroup = foodGroup,
                FoodType = foodType,
                PackageType = packageType,
                QuantityType = quantityType,
                Quantity = quantity,
                AverageUseByTime = averageUseByTime
            };

            var result = await App.Database.SaveProductAsync(newProduct);
            Console.WriteLine($"Product saved with result: {result}");

            if (Application.Current?.MainPage is NavigationPage navigationPage && navigationPage.RootPage is MyProductsPage myProductsPage)
            {
                myProductsPage?.RefreshProductList();
            }

            await Navigation.PopModalAsync();
        }

        // Handle package type changes
        private void OnPackageTypeChanged(object sender, CheckedChangedEventArgs e)
        {
            bool isPrepackaged = PrepackagedRadioButton.IsChecked;
            QuantityEntry.IsEnabled = isPrepackaged;

            if (!isPrepackaged)
            {
                QuantityEntry.Placeholder = "Only available for Prepackaged Products";
                QuantityEntry.Text = string.Empty;
                QuantityEntry.TextColor = Colors.Gray;
            }
            else
            {
                QuantityEntry.Placeholder = "Enter quantity";
                QuantityEntry.TextColor = Colors.Black;
            }
        }

        private async void OnCancelClicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}
