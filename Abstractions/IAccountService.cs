using UserManagementApp.Models.Dtos;
using UserManagementApp.Models.ViewModels;

namespace UserManagementApp.Abstractions;

public interface IAccountService
{
    public Task<Result<string>> RegisterAsync(RegisterVm registerVm);
    public Task<Result<string>> ConfirmEmailAsync(ConfirmationDto confirmationDto);
    public Task<Result<string>> LoginAsync(LoginVm loginVm, string? returnUrl);
    public Task<Result> ForgotPasswordAsync(ForgotPasswordVm forgotPasswordVm);
    public Task<Result<string>> ResetPasswordAsync(ResetPasswordVm resetPasswordVm);
    public Task<Result<string>> LogoutAsync();
}