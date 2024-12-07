using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using UserManagementApp.Abstractions;
using UserManagementApp.Data;
using UserManagementApp.Models.Entities;
using UserManagementApp.Services;

namespace UserManagementApp.Extensions;

public static class ServiceRegistration
{
    public static void AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Add services to the container.
        services.AddControllersWithViews();
        services.AddHttpContextAccessor();
        services.AddSingleton<ITempDataProvider, CookieTempDataProvider>();

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
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IUrlService, UrlService>();
        services.AddScoped<IEmailService, EmailService>();
        // services.AddScoped<IUserContactService, UserContactService>();
        // services.AddScoped<IContactService, ContactService>();
    }
}