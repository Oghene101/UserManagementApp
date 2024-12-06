using Microsoft.AspNetCore.Mvc;
using UserManagementApp.Abstractions;

namespace UserManagementApp.Controllers;

public class AccountController(
    IAccountService accountService) : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}