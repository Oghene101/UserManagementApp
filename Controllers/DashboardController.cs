using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserManagementApp.Abstractions;
using UserManagementApp.Constants;
using UserManagementApp.Dtos;

namespace UserManagementApp.Controllers;

[Authorize(Roles = Roles.Admin)]
public class DashboardController(
    IDashboardService dashboardService) : Controller
{
    [HttpGet("Dashboard/Users")]
    public async Task<IActionResult> GetUsers(int pageNumber = 1, int pageSize = 10)
    {
        var paginationFilter = new PaginationFilter(pageNumber, pageSize);

        //await dashboardService.AddUserToRoleAsync(userId, model.RoleName);
        var result = await dashboardService.GetUsersAsync(paginationFilter);
        //manageUserViewModel = await dashboardService.GetUserByIdAsync(userId);

        // Determine if this is an API call or a view rendering
        if (Request.Headers.Accept.Contains("application/json", StringComparer.OrdinalIgnoreCase))
            return Ok(result.Data.TableData); // Return JSON for API calls

        return View(result.Data);
    }

    [HttpGet("Dashboard/User/{userId}")]
    public async Task<IActionResult> GetUserById([FromRoute] string userId)
    {
        var result = await dashboardService.GetUserByIdAsync(userId);

        return View(result.Data);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser([FromRoute] string id)
    {
        var result = await dashboardService.DeleteUserAsync(id);
        if (!result.IsFailure) return View("ManageUser");

        ViewData["Error"] = result.Errors;
        return View("ManageUser");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRole([FromRoute] string id)
    {
        var result = await dashboardService.DeleteRoleAsync(id);
        if (!result.IsFailure) return View("ManagerUser");

        ViewData["Error"] = result.Errors;
        return View("ManageUser");
    }
}