using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using UserManagementApp.Abstractions;
using UserManagementApp.Dtos;
using UserManagementApp.Models.Entities;
using UserManagementApp.Models.ViewModels;
using UserManagementApp.Utilities;

namespace UserManagementApp.Services;

public class DashboardService(
    UserManager<User> userManager,
    RoleManager<IdentityRole> roleManager) : IDashboardService
{
    public async Task<Result> AddUserToRoleAsync(string? id, string roleName)
    {
        if (id.IsNullOrEmpty() || roleName.IsNullOrEmpty()) return new Error[] { Error.NullValue };

        var user = await userManager.FindByIdAsync(id);
        if (user == null) return new Error[] { Error.NullValue };

        var userRoles = await userManager.GetRolesAsync(user);
        if (!userRoles.Contains(roleName, StringComparer.OrdinalIgnoreCase))
        {
            await userManager.AddToRoleAsync(user, roleName);
        }

        return Result.Success();
    }

    public async Task<Result<ManageUserViewModel>> GetUserByIdAsync(string id)
    {
        if (id.IsNullOrEmpty()) return new Error[] { Error.NullValue };

        var user = await userManager.FindByIdAsync(id);
        if (user == null) return new Error[] { Error.NullValue };

        var manageUserViewModel = new ManageUserViewModel();
        var userRoles = await userManager.GetRolesAsync(user);
        var idenitityRoles = (await Task.WhenAll(userRoles.Select(async roleName =>
                await roleManager.FindByNameAsync(roleName))))
            .Where(identityRole => identityRole != null);

        manageUserViewModel.UserDetail = new UserVm(user.Id, user.FirstName, user.LastName, user.Email!, user.PhotoUrl,
            idenitityRoles.ToList());

        return manageUserViewModel;
    }

    public async Task<PaginatorDto<IEnumerable<UserVm>>> GetUsersAsync(PaginationFilter paginationFilter)
    {
        var paginatorDto = await userManager.Users.Select(user =>
                new UserVm(user.Id, user.FirstName, user.LastName, user.Email!, user.PhotoUrl, null))
            .PaginateAsync(paginationFilter);

        return paginatorDto;
    }


    public async Task<Result> DeleteUserAsync(string id)
    {
        if (id == "61c47527-a795-471a-bf1b-b824310815f5")
            return new Error[] { new("Dashboard.Error", "Cannot delete admin user!") };

        var user = await userManager.FindByIdAsync(id);
        if (user == null) return new Error[] { Error.NullValue };

        await userManager.DeleteAsync(user);
        return Result.Success();
    }

    public async Task<Result> DeleteRoleAsync(string id)
    {
        if (id == "96e1efbb-54c0-4268-8985-b6d7c6a31db8")
            return new Error[] { new("Dashboard.Error", "Cannot delete user role, delete user instead!") };

        var role = await roleManager.FindByIdAsync(id);
        if (role == null) return new Error[] { Error.NullValue };

        await roleManager.DeleteAsync(role);
        return Result.Success();
    }
}