using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using FoodBuddy.Services;
using System.IO;

namespace FoodBuddy
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // Setup SQLite Database Service
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "FoodBuddy.db3");
            var databaseService = new DatabaseService(dbPath);
            builder.Services.AddSingleton(databaseService);

            // Call InitializeDatabaseAsync() after the service is added
            Task.Run(async () => await databaseService.InitializeDatabaseAsync());

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }

    }
}
