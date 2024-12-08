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
        manageUserViewModel.UserDetail = new UserVm(user.Id, user.FirstName, user.LastName, user.Email!, user.PhotoUrl,
            userRoles.ToList());

        return manageUserViewModel;
    }

    public async Task<Result<ManageUserViewModel>> GetUsersAsync(PaginationFilter paginationFilter)
    {
        var paginatorDto = await userManager.Users.PaginateAsync(paginationFilter);
        if (!paginatorDto.PageItems!.Any()) return new[] { Error.NullValue };


        var manageUserVm = new ManageUserViewModel
        {
            TableData = paginatorDto.PageItems!.Select(user =>
                    new UserVm(user.Id, user.FirstName, user.LastName, user.Email!, user.PhotoUrl, new List<string>()))
                .ToArray()
        };

        return manageUserVm;
    }


    public async Task<Result> DeleteUserAsync(string id)
    {
        var user = await userManager.FindByIdAsync(id);
        if (user == null) return new Error[] { Error.NullValue };

        await userManager.DeleteAsync(user);
        return Result.Success();
    }

    public async Task<Result> DeleteRoleAsync(string id)
    {
        var role = await roleManager.FindByIdAsync(id);
        if (role == null) return new Error[] { Error.NullValue };

        await roleManager.DeleteAsync(role);
        return Result.Success();
    }
}