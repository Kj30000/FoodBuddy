using FoodBuddy.Models;
using System.Collections.ObjectModel;
using System.Linq;

namespace FoodBuddy.Pages
{
    public partial class AddProductForm : ContentPage
    {

        // Use ObservableCollection for dynamic updates
        public ObservableCollection<FoodGroup> AllFoodGroups { get; set; } = new ObservableCollection<FoodGroup>();
        public ObservableCollection<FoodType> FilteredFoodTypes { get; set; } = new ObservableCollection<FoodType>();


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


        // Picker Logic - Food Group and Food Type--------------------------------------------

        // Handle Picker selection of Food Groups & Load Food Types for FoodType Picker. 
        private async void OnFoodGroupSelected(object sender, EventArgs e)
        {
            var selectedFoodGroup = FoodGroupPicker.SelectedItem as FoodGroup;

            if (selectedFoodGroup != null)
            {
                // Load Food Types for the selected Food Group
                var foodTypes = await App.Database.GetFoodTypesByGroupAsync(selectedFoodGroup.Id);

                // Clear and update the FilteredFoodTypes collection
                FilteredFoodTypes.Clear();
                foreach (var foodType in foodTypes)
                {
                    FilteredFoodTypes.Add(foodType);
                }

                // Add the "Add New" option to the end of the list
                FilteredFoodTypes.Add(new FoodType { Name = "Add New" });

                // Enable the FoodTypePicker since a Food Group is selected
                FoodTypePicker.IsEnabled = true;

                // Hide or show the custom entry field for new food groups
                CustomFoodGroupEntry.IsVisible = selectedFoodGroup.Name == "Add New";
            }
            else
            {
                // Disable FoodTypePicker if no valid FoodGroup is selected
                FoodTypePicker.IsEnabled = false;
            }
        }


        // Handle Picker selection of Food Types
        private void OnFoodTypeSelected(object sender, EventArgs e)
        {
            var selectedFoodType = FoodTypePicker.SelectedItem as FoodType;

            if (selectedFoodType?.Name == "Add New")
            {
                CustomFoodTypeEntry.IsVisible = true;  // Show custom food type entry
            }
            else
            {
                CustomFoodTypeEntry.IsVisible = false;  // Hide custom entry
            }
        }


        // PACKAGETYPE & QUANTITY TYPE LOGIC






        // Function to handle the "Add Product" button click event (Once saved)
        private async void OnAddProductClicked(object sender, EventArgs e)
        {
            string productName = ProductNameEntry.Text;
            var selectedFoodGroup = FoodGroupPicker.SelectedItem as FoodGroup;
            var selectedFoodType = FoodTypePicker.SelectedItem as FoodType;
            string foodGroupName = selectedFoodGroup?.Name;

            // If "Add New" is selected for food group
            if (foodGroupName == "Add New")
            {
                foodGroupName = CustomFoodGroupEntry.Text;

                if (!string.IsNullOrWhiteSpace(foodGroupName))
                {
                    var newFoodGroup = new FoodGroup { Name = foodGroupName };
                    await App.Database.SaveFoodGroupAsync(newFoodGroup);  // Save new food group to the database

                    // Retrieve the saved FoodGroup's ID
                    selectedFoodGroup = newFoodGroup; // Update the selectedFoodGroup to use the newly created one

                    // Reload the food groups to include the newly added one
                    LoadFoodGroupsFromDatabase();
                }
                else
                {
                    await DisplayAlert("Error", "Please enter a valid Food Group name.", "OK");
                    return;
                }
            }

            // Check if the user is adding a new food type
            string foodTypeName = selectedFoodType?.Name;
            if (foodTypeName == "Add New")
            {
                foodTypeName = CustomFoodTypeEntry.Text;

                if (!string.IsNullOrWhiteSpace(foodTypeName))
                {
                    var newFoodType = new FoodType
                    {
                        Name = foodTypeName,
                        FoodGroupId = selectedFoodGroup.Id // Ensure the new FoodType links to the new FoodGroup
                    };
                    await App.Database.SaveFoodTypeAsync(newFoodType);  // Save new food type to the database

                    // Reload the food types to include the newly added one
                    var foodTypes = await App.Database.GetFoodTypesByGroupAsync(selectedFoodGroup.Id);
                    FilteredFoodTypes.Clear();
                    foreach (var foodType in foodTypes)
                    {
                        FilteredFoodTypes.Add(foodType);
                    }
                }
                else
                {
                    await DisplayAlert("Error", "Please enter a valid Food Type name.", "OK");
                    return;
                }
            }

            bool packageType = PrepackagedRadioButton.IsChecked;
            bool quantityType = MlRadioButton.IsChecked;

            if (string.IsNullOrWhiteSpace(productName) || string.IsNullOrWhiteSpace(foodGroupName) || string.IsNullOrWhiteSpace(foodTypeName))
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
                FoodType = foodTypeName,  // Use selected or newly added FoodType's name
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
