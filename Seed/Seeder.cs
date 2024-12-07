using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UserManagementApp.Constants;
using UserManagementApp.Data;
using UserManagementApp.Models.Entities;

namespace UserManagementApp.Seed;

public class Seeder
{
    public static async Task Run(IApplicationBuilder app)
    {
        var context = app.ApplicationServices.CreateScope()
            .ServiceProvider.GetRequiredService<AppDbContext>();

        if ((await context.Database.GetPendingMigrationsAsync()).Any())
            await context.Database.MigrateAsync();

        if (!context.Roles.Any())
        {
            var roles = new List<IdentityRole>
            {
                new() { Name = Roles.Admin, NormalizedName = Roles.Admin.ToUpper() },
                new() { Name = Roles.User, NormalizedName = Roles.User.ToUpper() }
            };
            await context.Roles.AddRangeAsync(roles);
            await context.SaveChangesAsync();
        }

        if (!context.Users.Any())
        {
            var userManager = app.ApplicationServices.CreateScope()
                .ServiceProvider.GetRequiredService<UserManager<User>>();

            var user = new User
            {
                FirstName = "Ogheneruemu",
                LastName = "Karieren",
                UserName = "ogheneruemu.engineer",
                Email = "ogheneruemu.engineer@gmail.com",
            };

            await userManager.CreateAsync(user, "Admin@123");
            await userManager.AddToRoleAsync(user, Roles.Admin);
        }
    }
}