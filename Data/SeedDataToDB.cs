using System.Text.Json;
using adoptera_hund.api.Models;
using Microsoft.AspNetCore.Identity;

namespace adoptera_hund.api.Data;

public class SeedDataToDB
{
    public static async Task LoadRolesAndUsers(UserManager<UserModel> userManager, RoleManager<IdentityRole> roleManager)
    {
        if (!roleManager.Roles.Any())
        {
            var admin = new IdentityRole { Name = "Admin", NormalizedName = "ADMIN" };
            var customer = new IdentityRole { Name = "Customer", NormalizedName = "CUSTOMER" };
            var user = new IdentityRole { Name = "User", NormalizedName = "USER" };

            await roleManager.CreateAsync(admin);
            await roleManager.CreateAsync(customer);
            await roleManager.CreateAsync(user);
        }

        if (!userManager.Users.Any())
        {
            var admin = new UserModel
            {
                UserName = "anton",
                Email = "anton",
                FirstName = "Anton",
                LastName = "Kozak"
            };

            await userManager.CreateAsync(admin, "Pa$$w0rd");
            await userManager.AddToRolesAsync(admin, new[]{"Admin","Customer","User"});

            var user = new UserModel
            {
                UserName = "lora@gmail.com",
                Email = "lora@gmail.com",
                FirstName = "Lora",
                LastName = "Lora"
            };

            await userManager.CreateAsync(user, "Pa$$w0rd");
            await userManager.AddToRoleAsync(user, "User");

            var customer = new UserModel
            {
                UserName = "Customer@gmail.com",
                Email = "Customer@gmail.com",
                FirstName = "Customer",
                LastName = "Customer"
            };

            await userManager.CreateAsync(customer, "Pa$$w0rd");
            await userManager.AddToRoleAsync(customer, "Customer");

        };
    }

    ///  Dogs loads data
    public static async Task LoadDogsToDB(AdopteraHundContext context)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        if (context.DogsDataBase.Any()) return;

        var json = System.IO.File.ReadAllText("Data/json/dogs.json");
        var listOfDogs = JsonSerializer.Deserialize<List<DogModel>>(json, options);

        if (listOfDogs is not null && listOfDogs.Count > 0)
        {
            await context.DogsDataBase.AddRangeAsync(listOfDogs);
            await context.SaveChangesAsync();
        }
    }
}
