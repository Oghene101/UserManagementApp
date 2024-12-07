using UserManagementApp.Models.Dtos;
using UserManagementApp.Models.ViewModels;

namespace UserManagementApp.Abstractions;

public interface IDashboardService
{
    public Task<Result> AddUserToRoleAsync(string id, string roleName);
    public Task<Result<ManageUserViewModel>> GetUserByIdAsync(string id);
    public Task<Result<ManageUserViewModel>> GetUsersAsync();
    public Task<Result> DeleteUserAsync(string id);
    public Task<Result> DeleteRoleAsync(string id);
}