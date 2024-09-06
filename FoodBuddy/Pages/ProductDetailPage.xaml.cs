using FoodBuddy.Models;
using Microsoft.Maui.Controls;

namespace FoodBuddy.Pages
{
    public partial class ProductDetailPage : ContentPage
    {

        public ProductDetailPage(Product product)
        {
            InitializeComponent();
            BindingContext = product;
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            var product = (Product)BindingContext;

            // Save the product details to the database
            await App.Database.SaveProductAsync(product);

            // Display a success message
            await DisplayAlert("Success", "Product details saved.", "OK");

            // Automatically refresh happens due to INotifyPropertyChanged
            await Navigation.PopAsync(); // Go back to the previous page
        }

        private async void OnDeleteClicked(object sender, EventArgs e)
        {
            var product = (Product)BindingContext;

            // Delete the product from the database
            await App.Database.DeleteProductAsync(product);

            // Display a success message
            await DisplayAlert("Deleted", "Product has been deleted.", "OK");

            // Refresh the product list on the MyProductsPage
            if (Application.Current?.MainPage is NavigationPage navigationPage && navigationPage.RootPage is MyProductsPage myProductsPage)
            {
                myProductsPage.RefreshProductList();
            }

            // Navigate back to the previous page
            await Navigation.PopAsync();
        }
    }
}
