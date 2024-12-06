using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using UserManagementApp.Models;
using UserManagementApp.Models.ViewModels;

namespace UserManagementApp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        var dummyListOfUsers = new List<UserVm>
        {
            new(
                "1", "Kenedy", "Taria", "kenedyt@sample.com",
                "https://image-placeholder.com/images/actual-size/120x150.png"
            ),
            new(
                "2", "Murphy", "Ogbeide", "murphyo@sample.com",
                "https://image-placeholder.com/images/actual-size/120x150.png"
            ),
            new(
                "3", "Agberowo", "Kayode", "agberowok@sample.com",
                "https://image-placeholder.com/images/actual-size/120x150.png"
            ),
            new(
                "4", "Babatunde", "Mustapha", "babatundem@sample.com",
                "https://image-placeholder.com/images/actual-size/120x150.png"
            ),
            new(
                "5", "Godwin", "Ozioko", "godwino@sample.com",
                "https://image-placeholder.com/images/actual-size/120x150.png"
            ),
            new(
                "6", "Ozoeze", "Boniface", "ozoezob@sample.com",
                "https://image-placeholder.com/images/actual-size/120x150.png"
            )
        };

        return View(dummyListOfUsers);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}