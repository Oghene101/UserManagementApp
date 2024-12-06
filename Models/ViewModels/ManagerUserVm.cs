namespace UserManagementApp.Models.ViewModels;

public record ManagerUserVm(
    string RoleName,
    IEnumerable<UserVm> TableData,
    UserVm UserVm);

public record UserVm(
    string Id,
    string FirstName,
    string LastName,
    string Email,
    string PhotoUrl,
    IEnumerable<string>? Roles = null);