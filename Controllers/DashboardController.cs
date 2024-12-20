﻿using Microsoft.AspNetCore.Authorization;
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
            return Ok(result); // Return JSON for API calls

        return View(result);
    }

    [HttpGet]
    public async Task<IActionResult> SearchUsers(string searchTerm, int pageNumber = 1, int pageSize = 10)
    {
        var paginationFilter = new PaginationFilter(pageNumber, pageSize);

        var result = await dashboardService.SearchUsersAsync(searchTerm, paginationFilter);

        // Determine if this is an API call or a view rendering
        if (Request.Headers.Accept.Contains("application/json", StringComparer.OrdinalIgnoreCase))
            return Ok(result); // Return JSON for API calls

        return View("GetUsers");
    }

    [HttpGet("Dashboard/User/{userId}")]
    public async Task<IActionResult> GetUserById([FromRoute] string userId)
    {
        var result = await dashboardService.GetUserByIdAsync(userId);

        return View(result.Data);
    }

    [HttpGet("Dashboard/Roles")]
    public async Task<IActionResult> GetRoles(int pageNumber = 1, int pageSize = 10)
    {
        var paginationFilter = new PaginationFilter(pageNumber, pageSize);

        var result = await dashboardService.GetRolesAsync(paginationFilter);

        // Determine if this is an API call or a view rendering
        if (Request.Headers.Accept.Contains("application/json", StringComparer.OrdinalIgnoreCase))
            return Ok(result); // Return JSON for API calls

        return View(result);
    }

    //TODO
    [HttpGet]
    public async Task<IActionResult> SearchRoles(string searchTerm, int pageNumber = 1, int pageSize = 10)
    {
        var paginationFilter = new PaginationFilter(pageNumber, pageSize);

        var result = await dashboardService.SearchUsersAsync(searchTerm, paginationFilter);

        // Determine if this is an API call or a view rendering
        if (Request.Headers.Accept.Contains("application/json", StringComparer.OrdinalIgnoreCase))
            return Ok(result); // Return JSON for API calls

        return View("GetUsers");
    }

    //TODO
    [HttpGet("Dashboard/Role/{roleId}")]
    public async Task<IActionResult> GetRoleById([FromRoute] string roleId)
    {
        var result = await dashboardService.GetUserByIdAsync(roleId);

        return View(result.Data);
    }

    [HttpDelete("DeleteUser/{id}")]
    public async Task<IActionResult> DeleteUser([FromRoute] string id)
    {
        var result = await dashboardService.DeleteUserAsync(id);

        // Determine if this is an API call or a view rendering
        if (!Request.Headers.Accept.Contains("application/json",
                StringComparer.OrdinalIgnoreCase)) return View("GetUsers"); // Return JSON for API calls
        if (!result.IsFailure)
            return Ok(result);
        return BadRequest(result);
    }

    [HttpDelete("DeleteRole/{id}")]
    public async Task<IActionResult> DeleteRole([FromRoute] string id)
    {
        var result = await dashboardService.DeleteRoleAsync(id);

        if (Request.Headers.Accept.Contains("application/json",
                StringComparer.OrdinalIgnoreCase)) // Return JSON for API calls
        {
            if (!result.IsFailure)
                return Ok(result);
            return BadRequest(result);
        }

        ViewData["Error"] = result.Errors;
        return View("GetUsers");
    }
}