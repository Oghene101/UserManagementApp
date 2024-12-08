using UserManagementApp.Dtos;
using UserManagementApp.Models.ViewModels;

namespace UserManagementApp.Abstractions;

public interface IAccountService
{
    public Task<Result<string>> RegisterAsync(RegisterVm registerVm);
    public Task<Result> ConfirmEmailAsync(ConfirmationDto confirmationDto);
    public Task<Result<string>> LoginAsync(LoginVm loginVm, string? returnUrl);
    public Task<Result> ForgotPasswordAsync(ForgotPasswordVm forgotPasswordVm);
    public Task<Result<string>> ResetPasswordAsync(ResetPasswordVm resetPasswordVm);
    public Task LogoutAsync();
}