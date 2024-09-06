using FoodBuddy.Models;
using System.Linq;

namespace FoodBuddy.Pages
{
    public partial class AddProductForm : ContentPage
    {
        // Default food groups
        private readonly List<string> defaultFoodGroups = new List<string>
        {
            "Vegetables", "Fruits", "Meat", "Dairy", "Grains", "Beverages"
        };

        private List<string> allFoodGroups = new List<string>();

        public AddProductForm()
        {
            InitializeComponent();
            LoadFoodGroupsFromDatabase();
        }

        // Load food groups from the database and add them to the allFoodGroups list
        private async void LoadFoodGroupsFromDatabase()
        {
            var products = await App.Database.GetProductsAsync();
            var foodGroupsFromDatabase = products.Select(p => p.FoodGroup).Distinct();

            allFoodGroups = defaultFoodGroups.Concat(foodGroupsFromDatabase).Distinct().ToList();
        }

        // Existing logic for package type, adding product, etc. remains unchanged
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

        private async void OnAddProductClicked(object sender, EventArgs e)
        {
            string productName = ProductNameEntry.Text;
            string foodGroup = FoodGroupEntry.Text;
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

        private async void OnCancelClicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}
