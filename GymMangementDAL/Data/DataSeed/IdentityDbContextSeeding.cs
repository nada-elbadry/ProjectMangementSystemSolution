using GymMangementDAL.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangementDAL.Data.DataSeed
{
    public class IdentityDbContextSeeding
    {
         public static bool SeedData(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManger)
        {
            try
            {
                var HasUsers = userManger.Users.Any();
                var HasRoles = roleManager.Roles.Any();
                if (HasUsers && HasRoles) return false;
                if (!HasRoles)
                {
                    var roles = new List<IdentityRole>()
                    {
                        new (){ Name="SuperAdmin"},
                        new (){ Name="Admin"},
                    };
                    foreach (var role in roles)
                    {

                        if (!roleManager.RoleExistsAsync(role.Name!).Result)
                        {
                            roleManager.CreateAsync(role).Wait();
                        }
                    }
                }

                if (!HasUsers)
                {
                    var MainAdmin = new ApplicationUser()
                    {
                        FirstName = "Nada",
                        LastName = "Elbadry",
                        UserName = "NadaElbadry",
                        Email = "nadaelbadry1012@gmail.com",
                        PhoneNumber = "01146309804"

                    };
                    userManger.CreateAsync(MainAdmin, "P@ssOrd").Wait();
                    userManger.AddToRoleAsync(MainAdmin, "SuperAdmin").Wait();

                    var Admin = new ApplicationUser()
                    {
                        FirstName = "Mohamed",
                        LastName = "Omar",
                        UserName = "MohamedOmar",
                        Email = "MohammedOmar37@gmail.com",
                        PhoneNumber = "01097767673"
                    };
                    userManger.CreateAsync(Admin, "P@ssOrd").Wait();
                    userManger.AddToRoleAsync(Admin, "Admin").Wait();
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Seeding Failed :{ex}");
                return false;
            }
        }
    }
}
