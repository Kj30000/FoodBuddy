using System.Collections.ObjectModel;
using Microsoft.Maui.Controls;
using FoodBuddy.Models;
using FoodBuddy.Services;

namespace FoodBuddy.Pages
{
    public partial class MyStockPage : ContentPage
    {
        public ObservableCollection<Stock> StockItems { get; set; } = new ObservableCollection<Stock>();

        public MyStockPage()
        {
            InitializeComponent();
            LoadStockFromDatabase();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        private async void LoadStockFromDatabase()
        {
            if (App.Database == null)
            {
                // Handle case where the database is not initialized properly
                return;
            }

            var stockItems = await App.Database.GetStockAsync();

            if (stockItems == null || stockItems.Count == 0)
            {
                // Handle case where stock items are null or empty
                return;
            }

            foreach (var stock in stockItems)
            {
                // Ensure that ProductName is available through the Stock.Product relationship
                stock.Product = await App.Database.GetProductAsync(stock.ProductId); // Load Product for StockItem
                StockItems.Add(stock);
            }

            BindingContext = this;
        }


        // Refresh stock list
        public void RefreshStockList()
        {
            StockItems.Clear();
            LoadStockFromDatabase();
        }


        private async void OnAddStockClicked(object sender, EventArgs e)
        {
            // Handle the addition of new stock (similar to how products are added)
            await Navigation.PushModalAsync(new AddStockForm());
        }

        // Handle navigation for bottom nav buttons

        private void OnHomeClicked(object sender, EventArgs e)
        {
            if (Application.Current != null)
            {
                Application.Current.MainPage = new NavigationPage(new MainPage()); // Navigate to Home page
            }
        }

        private void OnProductsClicked(object sender, EventArgs e)
        {
            if (Application.Current != null)
            {
                Application.Current.MainPage = new NavigationPage(new MyProductsPage()); // Navigate to Products page
            }
        }

        private void OnStockClicked(object sender, EventArgs e)
        {
            // Already on MyStockPage, no need to navigate
        }

        private void OnMealsClicked(object sender, EventArgs e)
        {
            if (Application.Current != null)
            {
                Application.Current.MainPage = new NavigationPage(new MealsPage()); // Navigate to Meals page
            }
        }


        
    }
}
