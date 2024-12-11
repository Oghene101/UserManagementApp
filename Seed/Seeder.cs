using Bogus;
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

            var defaultAdmin = new User
            {
                FirstName = "Ogheneruemu",
                LastName = "Karieren",
                UserName = "ogheneruemu.engineer",
                Email = "ogheneruemu.engineer@gmail.com",
                EmailConfirmed = true,
                CreatedAt = DateTimeOffset.UtcNow,
                UpdateAt = DateTimeOffset.UtcNow
            };

            await userManager.CreateAsync(defaultAdmin, "Admin@123");
            await userManager.AddToRoleAsync(defaultAdmin, Roles.Admin);

            // Create a Faker for your entity
            var userFaker = new Faker<User>()
                .RuleFor(u => u.FirstName, f => f.Name.FirstName())
                .RuleFor(u => u.LastName, f => f.Name.LastName())
                .RuleFor(u => u.Email, f => f.Internet.Email())
                .RuleFor(u => u.UserName, (f, u) => u.Email)
                .RuleFor(u => u.EmailConfirmed, (f, u) => true)
                .RuleFor(u => u.CreatedAt, () => DateTimeOffset.UtcNow)
                .RuleFor(u => u.UpdateAt, () => DateTimeOffset.UtcNow);

            // Generate dummy data
            var fakeUsers = userFaker.Generate(100);

            var tasks = fakeUsers.Select(async user =>
            {
                await userManager.CreateAsync(user, "User@123");
                await userManager.AddToRoleAsync(user, Roles.User);
            });
            await Task.WhenAll(tasks);
        }
    }
}