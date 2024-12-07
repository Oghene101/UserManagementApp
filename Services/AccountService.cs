using Microsoft.AspNetCore.Identity;
using UserManagementApp.Abstractions;
using UserManagementApp.Constants;
using UserManagementApp.Models.Dtos;
using UserManagementApp.Models.Entities;
using UserManagementApp.Models.ViewModels;

namespace UserManagementApp.Services;

public class AccountService(
    UserManager<User> userManager,
    SignInManager<User> signInManager,
    IUrlService urlService,
    IEmailService emailService) : IAccountService
{
    public async Task<Result<string>> RegisterAsync(RegisterVm registerVm)
    {
        var user = User.Create(registerVm);

        var createUserResult = await userManager.CreateAsync(user, registerVm.Password);
        if (!createUserResult.Succeeded)
            return createUserResult.Errors
                .Select(error => new Error(error.Code, error.Description)).ToArray();

        var addToRoleResult = await userManager.AddToRoleAsync(user, Roles.User);
        if (!addToRoleResult.Succeeded)
            return addToRoleResult.Errors
                .Select(error => new Error(error.Code, error.Description)).ToArray();

        var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
        var link = urlService.GenerateUrl("ConfirmEmail", "Account", new { user.Email, token });
        var body =
            $"Hi {user.FirstName}, <br/> please click the following link: <a href='{link}'>here</a> to confirm your email.";

        await emailService.SendEmailAsync(user.Email, "Confirm Email", body);

        return urlService.GenerateUrl("RegisterConfirmation", "Account", new { name = user.FirstName });
    }

    public async Task<Result<string>> ConfirmEmailAsync(ConfirmationDto confirmationDto)
    {
        var user = await userManager.FindByEmailAsync(confirmationDto.Email);
        if (user == null)
            return new Error[] { new("Account.Error", "Email confirmation failed.") };

        var confirmEmailResult = await userManager.ConfirmEmailAsync(user, confirmationDto.Token);
        if (!confirmEmailResult.Succeeded)
            return confirmEmailResult.Errors.Select(error => new Error(error.Code, error.Description)).ToArray();

        await signInManager.SignInAsync(user, false);
        return urlService.GenerateUrl("Index", "Home");
    }

    public async Task<Result<string>> LoginAsync(LoginVm loginVm, string? returnUrl)
    {
        var user = await userManager.FindByEmailAsync(loginVm.Email);
        if (user == null)
            return new Error[] { new("Account.Error", "Email or password is incorrect.") };

        var isEmailConfirmed = await userManager.IsEmailConfirmedAsync(user);
        if (!isEmailConfirmed)
            return new Error[] { new("Account.Error", "Confirm your email and try again.") };

        var loginResult = await signInManager.PasswordSignInAsync(user, loginVm.Password, loginVm.RememberMe, false);
        if (!loginResult.Succeeded)
            return new Error[] { new("Account.Error", "Email or password is incorrect.") };

        //return urlService.GetRedirectUrl(returnUrl);
        return returnUrl ?? "";
    }

    public async Task<Result> ForgotPasswordAsync(ForgotPasswordVm forgotPasswordVm)
    {
        var user = await userManager.FindByEmailAsync(forgotPasswordVm.Email);
        if (user == null)
            return new Error[] { new("Account.Error", "Email is incorrect.") };

        var token = await userManager.GeneratePasswordResetTokenAsync(user);
        var link = urlService.GenerateUrl("ResetPassword", "Account", new { user.Email, token });
        var body =
            $"Hi {user.FirstName}, you appear to have forgotten your password, click the link <a href='{link}'>here</a> to reset your password";

        await emailService.SendEmailAsync(user.Email, "Reset Password", body);
        return Result.Success();
    }

    public async Task<Result<string>> ResetPasswordAsync(ResetPasswordVm resetPasswordVm)
    {
        var user = await userManager.FindByEmailAsync(resetPasswordVm.Email);
        if (user == null)
            return new Error[] { new("Account.Error", "Email is incorrect.") };

        var resetPasswordResult =
            await userManager.ResetPasswordAsync(user, resetPasswordVm.Token, resetPasswordVm.NewPassword);

        return !resetPasswordResult.Succeeded
            ? resetPasswordResult.Errors.Select(error => new Error(error.Code, error.Description)).ToArray()
            : urlService.GenerateUrl("Login", "Account");
    }

    public async Task LogoutAsync()
    {
        await signInManager.SignOutAsync();
    }
}