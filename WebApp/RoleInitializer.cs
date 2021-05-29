using WebApp.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace WebApp
{
    public class RoleInitializer
    {
        public static async Task InitilizeAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            string adminEmail = "admin@gmail.com";
            string password = "123456aA";
            if (await roleManager.FindByNameAsync("admin") == null) await roleManager.CreateAsync(new IdentityRole("admin"));
            if (await roleManager.FindByNameAsync("user") == null) await roleManager.CreateAsync(new IdentityRole("user"));
            if (await userManager.FindByNameAsync("adminEmail") == null)
            {
                User admin = new User
                {
                    Email = adminEmail,
                    UserName = adminEmail
                };
                IdentityResult result = await userManager.CreateAsync(admin, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "admin");
                }
            }
            User adm = userManager.FindByEmailAsync(adminEmail).Result;
            if (!adm.EmailConfirmed)
            {
                adm.EmailConfirmed = true;
               await userManager.UpdateAsync(adm);
            }
        }
    }
}
