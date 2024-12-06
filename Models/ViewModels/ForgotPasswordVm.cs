using System.ComponentModel.DataAnnotations;

namespace UserManagementApp.Models.ViewModels;

public record ForgotPasswordVm([EmailAddress] string Email);

public record ResetPasswordVm(
    [EmailAddress] string Email,
    string Token,
    [DataType(DataType.Password)] string NewPassword);