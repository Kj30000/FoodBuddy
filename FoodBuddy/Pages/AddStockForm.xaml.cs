using FoodBuddy.Models;

namespace FoodBuddy.Pages
{
    public partial class AddStockForm : ContentPage
    {
        public AddStockForm()
        {
            InitializeComponent();
            LoadProducts();
        }

        private async void LoadProducts()
        {
            var products = await App.Database.GetProductsAsync();
            ProductPicker.ItemsSource = products;
        }

        private async void OnAddStockClicked(object sender, EventArgs e)
        {
            // Get input values
            if (ProductPicker.SelectedItem is not Product selectedProduct)
            {
                await DisplayAlert("Missing Information", "Please select a product.", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(QuantityEntry.Text) || !int.TryParse(QuantityEntry.Text, out int quantity))
            {
                await DisplayAlert("Missing Information", "Please enter a valid quantity.", "OK");
                return;
            }

            var purchaseDate = PurchaseDatePicker.Date;

            // Create a new Stock object
            var newStock = new Stock
            {
                ProductId = selectedProduct.Id, // Reference to the Product
                Quantity = quantity,
                PurchaseDate = purchaseDate
            };

            // Save the stock to the database
            var result = await App.Database.SaveStockAsync(newStock);
            Console.WriteLine($"Stock saved with result: {result}");

            // Refresh the stock list on the MyStockPage
            if (Application.Current?.MainPage is NavigationPage navigationPage && navigationPage.RootPage is MyStockPage myStockPage)
            {
                myStockPage.RefreshStockList();
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
