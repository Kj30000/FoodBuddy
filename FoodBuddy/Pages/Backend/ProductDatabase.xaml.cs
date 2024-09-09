using FoodBuddy.Models;
using System.Collections.ObjectModel;

namespace FoodBuddy.Pages.Backend

//This has been built to keep a bare bones backend database (All Products) view for testing and debugging purposes
{
    public partial class ProductDatabase : ContentPage
    {
        public ObservableCollection<Product> Products { get; set; } = new ObservableCollection<Product>();

        public ProductDatabase()
        {
            InitializeComponent();
            LoadProductsFromDatabase();
            BindingContext = this;
        }

        private async void LoadProductsFromDatabase()
        {
            var products = await App.Database.GetProductsAsync();
            Products.Clear();
            foreach (var product in products)
            {
                Products.Add(product);
            }
            ProductsListView.ItemsSource = Products;  // Binding the ListView to the data source
        }

        private async void OnCloseClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}
