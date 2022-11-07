using Microsoft.AspNetCore.Identity;
using SMS.Models;

namespace SMS.Enum
{
    public class Seed
    {
        public static async Task SeedRolesAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            await roleManager.CreateAsync(new IdentityRole(Enums.Roles.Admin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Enums.Roles.User.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Enums.Roles.Student.ToString()));
        }

        public static async Task SeedAdminAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            ApplicationUser admin = new ApplicationUser();
            admin.Id = "70a0f12a-43bb-4cda-b2c7-700195b945c5";
            admin.Email = "admin@sms.com";
            admin.EmailConfirmed = true;
            admin.FirstName = "Admin";
            admin.MiddleName = "";
            admin.LastName = "User";
            admin.PhoneNumber = "0123456789";
            admin.PhoneNumberConfirmed = true;
            admin.Address = String.Empty;
            admin.NormalizedUserName = "admin@sms.com";
            admin.UserName = "admin@sms.com";          
            admin.LockoutEnabled = false;
            admin.IsActive = true;
            try
            {
                await userManager.CreateAsync(admin, "@dm1nPassword");
                await userManager.AddToRoleAsync(admin, "Admin");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
