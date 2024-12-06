using System.ComponentModel.DataAnnotations;

namespace UserManagementApp.Models.ViewModels;

public record LoginVm(
    [EmailAddress] string Email,
    [DataType(DataType.Password)] string Password,
    bool RememberMe);

public record RegisterVm(
    [MaxLength(20)] string FirstName,
    [MaxLength(20)] string LastName,
    [EmailAddress] string Email,
    [Phone]
    [DataType(DataType.PhoneNumber)]
    string PhoneNumber,
    [DataType(DataType.Password)] string Password,
    [DataType(DataType.Password)] string ConfirmPassword);