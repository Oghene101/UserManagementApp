using System.ComponentModel.DataAnnotations;

namespace UserManagementApp.Models.ViewModels;

public class LoginVm
{
    [EmailAddress] public string Email { get; set; }
    [DataType(DataType.Password)] public string Password { get; set; }
    public bool RememberMe { get; set; }
}

public class RegisterVm
{
    [MaxLength(20)] public string FirstName { get; set; }

    [MaxLength(20)] public string LastName { get; set; }

    [EmailAddress] public string Email { get; set; }

    [Phone]
    [DataType(DataType.PhoneNumber)]
    public string PhoneNumber { get; set; }

    [DataType(DataType.Password)] public string Password { get; set; }

    [DataType(DataType.Password)]
    [Compare("Password")]
    public string ConfirmPassword { get; set; }
}