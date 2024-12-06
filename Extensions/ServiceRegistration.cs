using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UserManagementApp.Data;
using UserManagementApp.Models.Entities;

namespace UserManagementApp.Extensions;

public static class ServiceRegistration
{
    public static void AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Add services to the container.
        services.AddControllersWithViews();

        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                optionsBuilder => optionsBuilder.MigrationsAssembly(typeof(AppDbContext).Assembly.GetName().Name));
        });

        services.AddIdentity<User, IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

        // services.AddScoped<IUnitOfWork, UnitOfWork>();
        // services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        // services.AddScoped<IAuthService, AuthService>();
        // services.AddScoped<IUserContactService, UserContactService>();
        // services.AddScoped<IContactService, ContactService>();
    }
}