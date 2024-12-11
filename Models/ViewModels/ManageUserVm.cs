using Microsoft.AspNetCore.Identity;

namespace UserManagementApp.Models.ViewModels;

public record ManageUserVm(
    string Action,
    string? Id,
    string? RoleName);

// public record ManageUserViewModel(
//     string RoleName,
//     List<UserVm> TableData,
//     UserVm UserDetail);

public class ManageUserViewModel
{
    public string RoleName { get; set; }
    public IEnumerable<UserVm> TableData { get; set; } = [];
    public UserVm UserDetail { get; set; }
}

public record AddUserToRoleVm(
    string RoleName,
    string UserId);

public record GetUsersVm(
    IEnumerable<UserVm> TableData);

public record UserVm(
    string Id,
    string FirstName,
    string LastName,
    string Email,
    string PhotoUrl,
    IEnumerable<IdentityRole>? Roles = null);