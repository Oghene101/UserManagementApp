using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using UserManagementApp.Abstractions;
using UserManagementApp.Constants;
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
        var identityRoles = await GetIdentityRolesAsync(user);
        // var userRoles = await userManager.GetRolesAsync(user);
        // var idenitityRoles = (await Task.WhenAll(userRoles.Select(async roleName =>
        //         await roleManager.FindByNameAsync(roleName))))
        //     .Where(identityRole => identityRole != null);

        manageUserViewModel.UserDetail = new UserVm(user.Id, user.FirstName, user.LastName, user.Email!, user.PhotoUrl,
            identityRoles.ToArray());

        return manageUserViewModel;
    }

    public async Task<PaginatorDto<IEnumerable<UserVm>>> GetUsersAsync(PaginationFilter paginationFilter)
    {
        return await userManager.Users.OrderBy(user => user.FirstName)
            .Select(user =>
                new UserVm(user.Id, user.FirstName, user.LastName, user.Email!, user.PhotoUrl, null))
            .PaginateAsync(paginationFilter);
    }

    public async Task<PaginatorDto<IEnumerable<UserVm>>> SearchUsersAsync(string searchTerm,
        PaginationFilter paginationFilter)
    {
        return await userManager.Users.Where(user => user.FirstName.ToLower().Contains(searchTerm.ToLower())
                                                     || user.LastName.ToLower().Contains(searchTerm.ToLower())
                                                     || (user.Email != null &&
                                                         user.Email.ToLower().Contains(searchTerm.ToLower()))
                                                     || (user.PhoneNumber != null &&
                                                         user.PhoneNumber.Contains(searchTerm)))
            .Select(user => new UserVm(user.Id, user.FirstName, user.LastName, user.Email!, user.PhotoUrl, null))
            .PaginateAsync(paginationFilter);
    }

    public async Task<Result> DeleteUserAsync(string id)
    {
        var user = await userManager.FindByIdAsync(id);
        if (user == null) return new Error[] { Error.NullValue };

        var identityRole = (await GetIdentityRolesAsync(user))
            .FirstOrDefault(x => x.Id == Roles.AdminId); //AdminRoleId

        if (identityRole != null)
            return new Error[] { new("Dashboard.Error", "Cannot delete admin user!") };

        await userManager.DeleteAsync(user);
        return Result.Success();
    }

    public async Task<Result> DeleteRoleAsync(string id)
    {
        if (id == Roles.UserId)
            return new Error[] { new("Dashboard.Error", "Cannot delete user role, delete user instead!") };

        var role = await roleManager.FindByIdAsync(id);
        if (role == null) return new Error[] { Error.NullValue };

        await roleManager.DeleteAsync(role);
        return Result.Success();
    }

    private async Task<IEnumerable<IdentityRole>> GetIdentityRolesAsync(User user)
    {
        var identityRoles = roleManager.Roles;
        var userRoles = await userManager.GetRolesAsync(user);

        return identityRoles.Where(identityRole => userRoles.Contains(identityRole.Name!));
    }
}