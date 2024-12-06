using Microsoft.AspNetCore.Identity;

namespace UserManagementApp.Models.Entities;

public class User(
    string firstName,
    string lastName,
    string photoUrl) : IdentityUser
{
    public string FirstName { get; set; } = firstName;
    public string LastName { get; set; } = lastName;
    public string PhotoUrl { get; set; } = photoUrl;
}