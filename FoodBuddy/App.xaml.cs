using FoodBuddy.Services;
using System;
using System.IO;
using Microsoft.Maui.Controls;

namespace FoodBuddy
{
    public partial class App : Application
    {
        private static DatabaseService? _database;

        public static DatabaseService Database
        {
            get
            {
                _database ??= new DatabaseService(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "FoodBuddy.db3"));
                return _database;
            }
        }

        public App()
        {
            InitializeComponent();

            // Ensure the database is initialized properly
            var databaseService = Database;  // This ensures the service is instantiated.
            Task.Run(async () => await databaseService.InitializeDatabaseAsync());  // Async call to initialize the database.

            MainPage = new AppShell();
        }
    }
}
