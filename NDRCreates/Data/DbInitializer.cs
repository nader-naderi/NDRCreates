using Microsoft.AspNetCore.Identity;
using NDRCreates.Models.Entities;

namespace NDRCreates.Data
{
    public static class DbInitializer
    {
        public async static Task InitializeAsync(IServiceCollection services)
        {

            var userManager = services.BuildServiceProvider().GetRequiredService<UserManager<BasicUser>>();
            var roleManager = services.BuildServiceProvider().GetRequiredService<RoleManager<IdentityRole>>();

            // Create admin role
            bool roleExists = await roleManager.RoleExistsAsync("Admin");
            if (!roleExists)
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            // Create admin user
            var userExists = await userManager.FindByEmailAsync("123@Admin.com");
            if (userExists == null)
            {
                BasicUser adminUser = new BasicUser
                {
                    UserName = "123@Admin.com",
                    Email = "123@Admin.com",
                };

                await userManager.CreateAsync(adminUser, "123@Admin.com");
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
    }
}
