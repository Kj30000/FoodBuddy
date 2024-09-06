using SQLite;
using FoodBuddy.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FoodBuddy.Services
{
    public class DatabaseService
    {
        // SQLite connection to manage async database operations
        private readonly SQLiteAsyncConnection _database;



        // Constructor initializes the database connection and creates necessary tables
        public DatabaseService(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
        }



        public async Task InitializeDatabaseAsync()
        {
            await _database.CreateTableAsync<Product>();
            await _database.CreateTableAsync<Stock>();
            await _database.CreateTableAsync<AppSettings>();
        }



        // AppSettings Methods

        // Retrieves the AppSettings from the database. If no record exists, initializes default settings
        public async Task<AppSettings> GetAppSettingsAsync()
        {
            // Try to get the first (and only) AppSettings record
            var appSettings = await _database.Table<AppSettings>().FirstOrDefaultAsync();

            // If no AppSettings exist in the database, create default settings
            if (appSettings == null)
            {
                appSettings = new AppSettings
                {
                    // Set default ProductSortingLayers to 2
                    ProductSortingLayers = 2
                };

                // Save the default settings to the database
                await SaveAppSettingsAsync(appSettings);
            }

            // Return the AppSettings (either retrieved or newly created)
            return appSettings;
        }

        // Saves AppSettings to the database. If the settings exist, update them; otherwise, insert new ones
        public Task<int> SaveAppSettingsAsync(AppSettings appSettings)
        {
            // If the settings already have an ID, update the existing record
            if (appSettings.Id != 0)
            {
                return _database.UpdateAsync(appSettings); // Update existing AppSettings
            }
            else
            {
                // If no ID exists, insert new settings into the database
                return _database.InsertAsync(appSettings); // Insert new AppSettings
            }
        }




        // Product Methods

        // Retrieve a list of all products from the Product table
        public Task<List<Product>> GetProductsAsync()
        {
            return _database.Table<Product>().ToListAsync(); // Return all products
        }

        // Retrieve a specific product by its ID
        public Task<Product> GetProductAsync(int id)
        {
            return _database.Table<Product>().Where(i => i.Id == id).FirstOrDefaultAsync(); // Return the product with matching ID
        }

        // Save a Product to the database. If it has an ID, update it; otherwise, insert a new product
        public Task<int> SaveProductAsync(Product product)
        {
            if (product.Id != 0)
            {
                return _database.UpdateAsync(product); // Update existing Product
            }
            else
            {
                return _database.InsertAsync(product); // Insert new Product
            }
        }

        // Delete a product from the database
        public Task<int> DeleteProductAsync(Product product)
        {
            return _database.DeleteAsync(product); // Delete Product from the table
        }





        // Stock Methods

        // Retrieve a list of all stock items and link each stock to its corresponding Product
        public async Task<List<Stock>> GetStockAsync()
        {
            // Load all stock items
            var stockItems = await _database.Table<Stock>().ToListAsync();

            // Collect all ProductIds in one list
            var productIds = stockItems.Select(stock => stock.ProductId).Distinct().ToList();

            // Fetch all products in a single query
            var products = await _database.Table<Product>().Where(p => productIds.Contains(p.Id)).ToListAsync();

            // Create a dictionary for quick lookup of products by ID
            var productDict = products.ToDictionary(p => p.Id);

            // Link each stock item with its corresponding product
            foreach (var stock in stockItems)
            {
                if (productDict.TryGetValue(stock.ProductId, out Product? value))
                {
                    stock.Product = value;
                }
            }

            return stockItems;
        }

        // Save a Stock item to the database. If it has an ID, update it; otherwise, insert a new stock item
        public Task<int> SaveStockAsync(Stock stock)
        {
            if (stock.Id != 0)
            {
                return _database.UpdateAsync(stock); // Update existing Stock item
            }
            else
            {
                return _database.InsertAsync(stock); // Insert new Stock item
            }
        }

        // Delete a stock item from the database
        public Task<int> DeleteStockAsync(Stock stock)
        {
            return _database.DeleteAsync(stock); // Delete Stock from the table
        }
    }
}
