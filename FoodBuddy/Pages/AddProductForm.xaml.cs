using FoodBuddy.Models;
using System.Collections.ObjectModel;
using System.Linq;

namespace FoodBuddy.Pages
{
    public partial class AddProductForm : ContentPage
    {

        // Use ObservableCollection for dynamic updates
        public ObservableCollection<FoodGroup> AllFoodGroups { get; set; } = new ObservableCollection<FoodGroup>();

        public AddProductForm()
        {
            InitializeComponent();
            LoadFoodGroupsFromDatabase();
            BindingContext = this;  // Set BindingContext to use data binding
        }


        // Load food groups from the FoodGroup table in the database
        private async void LoadFoodGroupsFromDatabase()
        {
            Console.WriteLine("Loading Food Groups...");

            var foodGroups = await App.Database.GetFoodGroupsAsync();
            Console.WriteLine($"Found {foodGroups.Count} food groups.");

            // Clear existing items and add new sorted ones from the database
            AllFoodGroups.Clear();
            foreach (var group in foodGroups.OrderBy(g => g.Name))
            {
                AllFoodGroups.Add(group);
                Console.WriteLine($"Added Food Group: {group.Name}");
            }

            // Add the "Add New" option to the list
            AllFoodGroups.Add(new FoodGroup { Name = "Add New" });
        }



        // Handle Picker selection
        private void OnFoodGroupSelected(object sender, EventArgs e)
        {
            var selectedFoodGroup = FoodGroupPicker.SelectedItem as FoodGroup;

            if (selectedFoodGroup?.Name == "Add New")
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
            var selectedFoodGroup = FoodGroupPicker.SelectedItem as FoodGroup;
            string foodGroupName = selectedFoodGroup?.Name;

            // If "Add New" is selected, create a new FoodGroup entry
            if (foodGroupName == "Add New")
            {
                foodGroupName = CustomFoodGroupEntry.Text;

                if (!string.IsNullOrWhiteSpace(foodGroupName))
                {
                    var newFoodGroup = new FoodGroup { Name = foodGroupName };
                    await App.Database.SaveFoodGroupAsync(newFoodGroup);  // Save new food group to the database

                    // Reload the food groups to include the newly added one
                    LoadFoodGroupsFromDatabase();
                }
                else
                {
                    await DisplayAlert("Error", "Please enter a valid Food Group name.", "OK");
                    return;
                }
            }

            string foodType = FoodTypeEntry.Text;  // For now, we'll keep this simple.
            bool packageType = PrepackagedRadioButton.IsChecked;
            bool quantityType = MlRadioButton.IsChecked;

            if (string.IsNullOrWhiteSpace(productName) || string.IsNullOrWhiteSpace(foodGroupName) || string.IsNullOrWhiteSpace(foodType))
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
                FoodGroup = foodGroupName,
                FoodType = foodType,
                PackageType = packageType,
                QuantityType = quantityType,
                Quantity = quantity,
                AverageUseByTime = averageUseByTime
            };

            var result = await App.Database.SaveProductAsync(newProduct);
            Console.WriteLine($"Product saved with result: {result}");

            // Refresh the product list if MyProductsPage is available
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
