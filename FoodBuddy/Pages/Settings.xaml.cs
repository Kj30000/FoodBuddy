using Microsoft.Maui.Controls;
using FoodBuddy.Pages;  // Ensure this is here for page navigation

namespace FoodBuddy.Pages
{
    public partial class Settings : ContentPage
    {
        public Settings()
        {
            InitializeComponent();  // Ensure InitializeComponent() is called
            LoadSettings();
        }

        private async void LoadSettings()
        {
            // Get the settings asynchronously
            var settings = await App.Database.GetAppSettingsAsync();

            // Check and set the radio buttons
            if (settings.ProductSortingLayers == 2)
            {
                TwoSortingLayersRadioButton.IsChecked = true;
            }
            else if (settings.ProductSortingLayers == 3)
            {
                ThreeSortingLayersRadioButton.IsChecked = true;
            }
        }

        private void OnHomeClicked(object sender, EventArgs e)
        {
            // Home button - No action needed if this is the home page
        }

        private void OnProductsClicked(object sender, EventArgs e)
        {
            if (Application.Current != null)
            {
                Application.Current.MainPage = new NavigationPage(new MyProductsPage()); // Navigate to Products page wrapped in a NavigationPage
            }
        }

        private void OnStockClicked(object sender, EventArgs e)
        {
            if (Application.Current != null)
            {
                Application.Current.MainPage = new NavigationPage(new MyStockPage()); // Navigate to Stock page wrapped in a NavigationPage
            }
        }

        private void OnMealsClicked(object sender, EventArgs e)
        {
            if (Application.Current != null)
            {
                Application.Current.MainPage = new NavigationPage(new MealsPage()); // Navigate to Meals page wrapped in a NavigationPage
            }
        }
    }
}
