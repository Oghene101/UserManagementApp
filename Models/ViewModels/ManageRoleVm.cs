namespace UserManagementApp.Models.ViewModels;

public record ManageRoleVm(
    string RoleName,
    IEnumerable<RoleVm> RolesToReturn);

public class RoleVm(
    string Id,
    string Name);