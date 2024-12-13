using UserManagementApp.Dtos;
using UserManagementApp.Models.ViewModels;

namespace UserManagementApp.Abstractions;

public interface IDashboardService
{
    public Task<Result> AddUserToRoleAsync(string? id, string roleName);
    public Task<Result<ManageUserViewModel>> GetUserByIdAsync(string id);
    public Task<PaginatorDto<IEnumerable<UserVm>>> GetUsersAsync(PaginationFilter paginationFilter);
    public Task<Result> DeleteUserAsync(string id);
    public Task<Result> DeleteRoleAsync(string id);

    public Task<PaginatorDto<IEnumerable<UserVm>>> SearchUsersAsync(string searchTerm,
        PaginationFilter paginationFilter);
}