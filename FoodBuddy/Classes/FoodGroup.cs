using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodBuddy.Models
{
    public class FoodGroup
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }   // Unique identifier for the Food Group

        public string Name { get; set; } = string.Empty; // Name of the Food Group

        // Each Food Group will have a list of associated Food Types
        [Ignore]  // Ignoring for SQLite to prevent list serialization
        public List<FoodType> FoodTypes { get; set; } = [];
    }

    public class FoodType
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }            // Unique identifier for the Food Type
        public string Name { get; set; } = string.Empty; // Name of the Food Type

        public int FoodGroupId { get; set; }   // Foreign key to link Food Type to Food Group
    }
}