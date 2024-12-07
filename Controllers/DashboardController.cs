using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserManagementApp.Abstractions;
using UserManagementApp.Constants;
using UserManagementApp.Models.ViewModels;

namespace UserManagementApp.Controllers;

[Authorize(Roles = Roles.Admin)]
public class DashboardController(
    IDashboardService dashboardService) : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> ManageUser(ManageUserViewModel model, string? userId, string action)
    {
        await dashboardService.AddUserToRoleAsync(userId, model.RoleName);
        await dashboardService.GetUsersAsync();
        await dashboardService.GetUserByIdAsync(userId);

        return View(model);
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
        var result = await dashboardService.GetUsersAsync();
        if (!result.IsFailure) return View("ManagerUser");

        ViewData["Error"] = result.Errors;
        return View("ManageUser");
    }
}