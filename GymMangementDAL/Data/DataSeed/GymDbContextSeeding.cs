using GymMangementDAL.Data.Contexts;
using GymMangementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GymMangementDAL.Data.DataSeed
{
    public  static class GymDbContextSeeding
    {
        public static bool SeedData(GymDbContext dbContext)
        {
            try
            {
                var HasPlan = dbContext.Plans.Any();
                var HasCategory = dbContext.Categories.Any();
                if (HasPlan && HasCategory) return false;
                if (!HasCategory)
                {
                    var Plans = LoadDataFromJsonFile<Plan>("plans.json");
                    if (Plans.Any()) dbContext.Plans.AddRange(Plans);
                }

                if (!HasCategory)
                {
                    var Categories = LoadDataFromJsonFile<Category>("categories.json");
                    if (Categories.Any()) dbContext.Categories.AddRange(Categories);
                }
                return dbContext.SaveChanges() > 0;

            }
            catch(Exception ex) 
            {
                Console.WriteLine($"Seeding Failed :{ex}");
            return false;
            }
           
        }

        private static List<T> LoadDataFromJsonFile<T>(string fileName)
        {
            var FilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files" , fileName);
            if(!File.Exists(FilePath))throw new FileNotFoundException();
            string Data = File.ReadAllText(FilePath);
            var Options = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,
            };
            return JsonSerializer.Deserialize<List<T>>(Data, Options) ?? new List<T>();
        }
    }
}
