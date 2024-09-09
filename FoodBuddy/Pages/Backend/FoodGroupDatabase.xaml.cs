using FoodBuddy.Models;
using System.Collections.ObjectModel;

namespace FoodBuddy.Pages.Backend

    //This has bene built to keep a bare bones backend database (Food Groups) view for testing and debugging purposes
{
    public partial class FoodGroupDatabase : ContentPage
    {
        public ObservableCollection<FoodGroupDisplay> FoodGroups { get; set; } = new ObservableCollection<FoodGroupDisplay>();

        public FoodGroupDatabase()
        {
            InitializeComponent();
            LoadFoodGroupsFromDatabase();
            BindingContext = this;
        }

        private async void LoadFoodGroupsFromDatabase()
        {
            var foodGroups = await App.Database.GetFoodGroupsAsync();
            var foodTypes = await App.Database.GetFoodTypesAsync();

            FoodGroups.Clear();

            // Group food types by their FoodGroupId and create a display model
            foreach (var foodGroup in foodGroups)
            {
                var attachedFoodTypes = foodTypes.Where(ft => ft.FoodGroupId == foodGroup.Id).ToList();
                var displayModel = new FoodGroupDisplay
                {
                    Name = foodGroup.Name,
                    FoodTypesDisplay = string.Join(", ", attachedFoodTypes.Select(ft => ft.Name))  // List all food types as a string
                };

                FoodGroups.Add(displayModel);
            }

            // Handle food types without a FoodGroupId
            var unattachedFoodTypes = foodTypes.Where(ft => ft.FoodGroupId == 0).ToList();
            if (unattachedFoodTypes.Any())
            {
                var noGroupDisplayModel = new FoodGroupDisplay
                {
                    Name = "No Food Group",
                    FoodTypesDisplay = string.Join(", ", unattachedFoodTypes.Select(ft => ft.Name))
                };
                FoodGroups.Add(noGroupDisplayModel);
            }

            FoodGroupsListView.ItemsSource = FoodGroups;  // Bind the ListView to the data source
        }

        private async void OnCloseClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }

    // Display model to simplify ListView display
    public class FoodGroupDisplay
    {
        public string Name { get; set; } // Food group name
        public string FoodTypesDisplay { get; set; } // Concatenated food types for display
    }
}
