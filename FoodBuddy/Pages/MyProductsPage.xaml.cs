using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using FoodBuddy.Models;
using FoodBuddy.ViewModels;


namespace FoodBuddy.Pages
{
    public partial class MyProductsPage : ContentPage
    {
        public ObservableCollection<FoodGroupViewModel> FoodGroups { get; set; } = new ObservableCollection<FoodGroupViewModel>();
        public ICommand ProductTappedCommand { get; }

        public MyProductsPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            ProductTappedCommand = new Command<Product>(OnProductTapped);
            LoadProductsFromDatabase();
        }

        private async void LoadProductsFromDatabase()
        {
            if (App.Database == null)
            {
                // Handle null database case, e.g., show an error message or return early
                return;
            }

            var products = await App.Database.GetProductsAsync();

            if (products == null || products.Count == 0)
            {
                // Handle case where products are null or empty
                return;
            }


            if (products == null || products.Count == 0)
            {
                // Handle case where products are null or empty
                return;
            }

            // Group products by FoodGroup
            var groupedProducts = products.GroupBy(p => p.FoodGroup ?? "Unknown");

            foreach (var group in groupedProducts)
            {
                var foodGroupViewModel = new FoodGroupViewModel
                {
                    GroupName = group.Key ?? "Unknown", // Handle possible null value
                    Products = new ObservableCollection<Product>(group.ToList())
                };
                FoodGroups.Add(foodGroupViewModel);
            }

            BindingContext = this;
        }

        private void OnProductTappedHandler(object sender, EventArgs e)
        {
            if (sender is Label label && label.BindingContext is Product product)
            {
                Console.WriteLine($"{product.ProductName} pressed.");
                // You can still call the command if necessary
                ProductTappedCommand?.Execute(product);
            }
        }


        // Handle product tap to navigate to product detail page
        private async void OnProductTapped(Product product)
        {
            await Navigation.PushAsync(new ProductDetailPage(product));
        }

        // This method is called when the page becomes visible. To then run the RefreshProductList - So products listes are always updtodate. 
        protected override void OnAppearing()
        {
            base.OnAppearing();
            RefreshProductList();
        }

        //Handle product list refresh when a product is added or deleted. Is call from OnAppearing method from this code + other areas TBD (I believe when deleting)
        public void RefreshProductList()
        {
            FoodGroups.Clear();
            LoadProductsFromDatabase();
        }


        private async void OnAddProductClicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new AddProductForm());
        }



        // Functions for bottom navigation buttons


        private void OnHomeClicked(object sender, EventArgs e)
        {
            if (Application.Current != null)
            {
                Application.Current.MainPage = new NavigationPage(new MainPage()); // Navigate to Home page
            }
        }

        private void OnProductsClicked(object sender, EventArgs e)
        {
            // Already on MyProductsPage, no need to navigate
        }

        private void OnStockClicked(object sender, EventArgs e)
        {
            if (Application.Current != null)
            {
                Application.Current.MainPage = new NavigationPage(new MyStockPage()); // Navigate to Stock page
            }
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
