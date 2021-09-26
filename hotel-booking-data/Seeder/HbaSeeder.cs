using hotel_booking_data.Contexts;
using hotel_booking_models;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_data.Seeder
{
    public class HbaSeeder
    {
        public static async Task SeedData(HbaDbContext dbContext, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            var baseDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            await dbContext.Database.EnsureCreatedAsync();
            if (!dbContext.Users.Any())
            {
                List<string> roles = new List<string> { "Admin", "Manager", "Regular" };

                foreach (var role in roles)
                {
                    await roleManager.CreateAsync(new IdentityRole { Name = role });
                }

                var user = new AppUser
                {
                    Id = Guid.NewGuid().ToString(),
                    FirstName = "Hotel",
                    LastName = "Admin",
                    UserName = "hbapp",
                    Email = "info@hba.com",
                    PhoneNumber = "09043546576",
                    Gender = "Male",
                    Age = 34,
                    IsActive = true,
                    PublicId = null,
                    Avatar = "http://placehold.it/32x32",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                await userManager.CreateAsync(user, "Password@123");
                await userManager.AddToRoleAsync(user, "Admin");


                var path = File.ReadAllText(baseDir + @"/jsons/users.json");

                var hbaUsers = JsonConvert.DeserializeObject<List<AppUser>>(path);
                for (int i = 0; i < hbaUsers.Count; i++)
                {
                    await userManager.CreateAsync(hbaUsers[i], "Password@123");
                    if (i < 5)
                    {
                        await userManager.AddToRoleAsync(hbaUsers[i], "Manager");
                        continue;
                    }
                    await userManager.AddToRoleAsync(hbaUsers[i], "Regular");
                }
            }

            if (!dbContext.Customers.Any())
            {
                var path = File.ReadAllText(baseDir + @"/jsons/customer.json");

                var customers = JsonConvert.DeserializeObject<List<Customer>>(path);
                await dbContext.Customers.AddRangeAsync(customers);
            }

            if (!dbContext.Managers.Any())
            {
                var path = File.ReadAllText(baseDir + @"/jsons/manager.json");

                var managers = JsonConvert.DeserializeObject<List<Manager>>(path);
                await dbContext.Managers.AddRangeAsync(managers);
            }
            await dbContext.SaveChangesAsync();
        }
    }
}
