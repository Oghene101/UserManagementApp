using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using UserManagementApp.Models.Dtos;
using UserManagementApp.Models.ViewModels;

namespace UserManagementApp.Controllers;

public class HomeController(ILogger<HomeController> logger) : Controller
{
    private readonly ILogger<HomeController> _logger = logger;

    public IActionResult Index()
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

    public IActionResult Privacy()
    {
        return View();
    }

    [HttpGet("/Error/{statusCode}")]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error(int statusCode)
    {
        if (TempData["ResponseDto"] is not string responseDtoJson) return View();

        var responseDto = JsonSerializer.Deserialize<ResponseDto>(responseDtoJson);
        return View(responseDto);
    }
}