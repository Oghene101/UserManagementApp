namespace UserManagementApp.Models.ViewModels;

public record ManageRoleVm(
    string RoleName,
    IEnumerable<RoleVm> RolesToReturn);

public record RoleVm(
    string Id,
    string Name);