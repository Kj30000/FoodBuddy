namespace FoodBuddy.Pages;

public partial class MealsPage : ContentPage
{
	public MealsPage()
	{
		InitializeComponent();
	}

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
        if (Application.Current != null)
        {
            Application.Current.MainPage = new NavigationPage(new MyStockPage()); // Navigate to Stock page
        }
    }

    private void OnMealsClicked(object sender, EventArgs e)
    {
        // Already on MealsPage, no need to navigate
    }

}