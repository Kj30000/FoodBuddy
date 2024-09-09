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

        private async void OnFoodGroupDatabaseClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Backend.FoodGroupDatabase()); // Navigate to the Food Group Database page
        }

        private async void OnProductDatabaseClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Backend.ProductDatabase()); // Navigate to the Product Database page
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
