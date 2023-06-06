using ASPNetCoreApp.DAL.Models;
using DAL.Models;
using Microsoft.AspNetCore.Identity;

namespace ASPNetCoreApp.Data
{
    public class IdentitySeed
    {
        public static async Task CreateUserRoles(IServiceProvider serviceProvider)
        {
            try
            {
                var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = serviceProvider.GetRequiredService<UserManager<User>>();

                //Создание ролей администратора и пользователя
                if (await roleManager.FindByNameAsync("admin") == null)
                {
                    await roleManager.CreateAsync(new IdentityRole("admin"));
                }
                if (await roleManager.FindByNameAsync("user") == null)
                {
                    await roleManager.CreateAsync(new IdentityRole("user"));
                }

                //Создание Администратора
                string adminEmail = "administrator@mail.com";
                string adminPassword = "Aa123456!";
                if (await userManager.FindByNameAsync(adminEmail) == null)
                {
                    User admin = new User { Email = adminEmail, UserName = adminEmail };
                    IdentityResult result = await userManager.CreateAsync(admin, adminPassword);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(admin, "admin");
                    }
                }

                //Создание Оператора
                string userEmail = "operator@mail.com";
                string userPassword = "Aa123456!";
                if (await userManager.FindByNameAsync(userEmail) == null)
                {
                    User user = new User { Email = userEmail, UserName = userEmail };
                    IdentityResult result = await userManager.CreateAsync(user, userPassword);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, "user");
                    }
                }
            }
            catch (Exception ex)
            {
                //Обработка исключения
                Console.WriteLine($"Произошла ошибка: {ex.Message}");
            }
        }
    }
}
