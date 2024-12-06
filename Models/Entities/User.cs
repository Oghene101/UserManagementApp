using Microsoft.AspNetCore.Identity;
using UserManagementApp.Models.ViewModels;

namespace UserManagementApp.Models.Entities;

public class User : IdentityUser
{
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public string PhotoUrl { get; set; } = "";

    public static User Create(RegisterVm registerVm)
    {
        return new User
        {
            FirstName = registerVm.FirstName,
            LastName = registerVm.FirstName,
            Email = registerVm.Email,
            PhoneNumber = registerVm.PhoneNumber,
            UserName = registerVm.Email
        };
    }
}