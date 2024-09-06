using FoodBuddy.Models;

namespace FoodBuddy.Pages
{
    public partial class AddProductForm : ContentPage
    {
        public AddProductForm()
        {
            InitializeComponent();
        }

        private void OnPackageTypeChanged(object sender, CheckedChangedEventArgs e)
        {
            // Enable or disable the Quantity Entry based on Prepackaged selection
            bool isPrepackaged = PrepackagedRadioButton.IsChecked;
            QuantityEntry.IsEnabled = isPrepackaged;

            // Change text color and placeholder accordingly
            if (!isPrepackaged)
            {
                QuantityEntry.Placeholder = "Only available for Prepackaged Products";
                QuantityEntry.Text = string.Empty; // Clear the text if disabled
                QuantityEntry.TextColor = Colors.Gray;
            }
            else
            {
                QuantityEntry.Placeholder = "Enter quantity";
                QuantityEntry.TextColor = Colors.Black;
            }
        }

        private async void OnAddProductClicked(object sender, EventArgs e)
        {
            // Get input values
            string productName = ProductNameEntry.Text;
            string foodGroup = FoodGroupEntry.Text;
            string foodType = FoodTypeEntry.Text;

            // Determine Package Type and Quantity Type as booleans
            bool packageType = PrepackagedRadioButton.IsChecked;
            bool quantityType = MlRadioButton.IsChecked;

            // Validate that fields are not empty
            if (string.IsNullOrWhiteSpace(productName) || string.IsNullOrWhiteSpace(foodGroup) || string.IsNullOrWhiteSpace(foodType))
            {
                await DisplayAlert("Missing Information", "Please fill out all fields.", "OK");
                return;
            }

            // Try parsing Quantity and check if successful
            bool isQuantityParsed = int.TryParse(QuantityEntry.Text, out int quantity);
            if (!isQuantityParsed)
            {
                await DisplayAlert("Invalid Input", "Please enter a valid quantity.", "OK");
                return;
            }

            // Try parsing AverageUseByTime and check if successful
            bool isAverageUseByTimeParsed = int.TryParse(AverageUseByTimeEntry.Text, out int averageUseByTime);
            if (!isAverageUseByTimeParsed)
            {
                await DisplayAlert("Invalid Input", "Please enter a valid average use by time.", "OK");
                return;
            }


            // Create a new Product object
            var newProduct = new Product
            {
                ProductName = productName,
                FoodGroup = foodGroup,
                FoodType = foodType,
                PackageType = packageType, // Assign the boolean value
                QuantityType = quantityType, // Assign the boolean value
                Quantity = quantity,
                AverageUseByTime = averageUseByTime
            };

            // Save the product to the database
            var result = await App.Database.SaveProductAsync(newProduct);
            Console.WriteLine($"Product saved with result: {result}");

            // Refresh the product list on the MyProductsPage
            if (Application.Current?.MainPage is NavigationPage navigationPage && navigationPage.RootPage is MyProductsPage myProductsPage)
            {
                myProductsPage?.RefreshProductList();
            }

            // Navigate back to the previous page
            await Navigation.PopModalAsync();
        }

        private async void OnCancelClicked(object sender, EventArgs e)
        {
            // Navigate back to the previous page without saving
            await Navigation.PopModalAsync();
        }
    }
}
