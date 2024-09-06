using Microsoft.Maui.Controls;
using FoodBuddy.Pages;

namespace FoodBuddy.Pages
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
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

        private async void OnSettingsClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Settings()); // Push the Settings page onto the stack
        }

    }
}
