﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using UserManagementApp.Abstractions;
using UserManagementApp.Dtos;
using UserManagementApp.Models.ViewModels;

namespace UserManagementApp.Controllers;

public class AccountController(
    IAccountService accountService) : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterVm registerVm)
    {
        if (!ModelState.IsValid) return View(registerVm);

        var result = await accountService.RegisterAsync(registerVm);
        if (!result.IsFailure) return LocalRedirect(result.Data);

        foreach (var error in result.Errors) ModelState.AddModelError(error.Code, error.Message);

        return View(registerVm);
    }

    [HttpGet]
    public IActionResult RegisterConfirmation(string name)
    {
        ViewBag.Name = name;
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> ConfirmEmail(ConfirmationDto confirmationDto)
    {
        var result = await accountService.ConfirmEmailAsync(confirmationDto);
        if (!result.IsFailure) return RedirectToAction("Index", "Home");

        foreach (var error in result.Errors) ModelState.AddModelError(error.Code, error.Message);

        return View(ModelState);
    }

    [HttpGet]
    public IActionResult Login(string? returnUrl)
    {
        ViewBag.ReturnUrl = returnUrl;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginVm loginVm, string? returnUrl)
    {
        if (!ModelState.IsValid) return View(returnUrl);

        var result = await accountService.LoginAsync(loginVm, returnUrl);
        if (!result.IsFailure)
        {
            if (result.Data.IsNullOrEmpty())
                return RedirectToAction("Index", "Home");
            ;
            return Redirect(result.Data);
        }

        foreach (var error in result.Errors) ModelState.AddModelError(error.Code, error.Message);

        return View(loginVm);
    }

    [HttpGet]
    public IActionResult ForgotPassword()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordVm forgotPasswordVm)
    {
        var result = await accountService.ForgotPasswordAsync(forgotPasswordVm);
        if (!result.IsFailure)
        {
            ViewBag.Message =
                "Reset password link has been sent to the email provided. If correct you should already get it by now.";
            return View();
        }

        foreach (var error in result.Errors) ModelState.AddModelError(error.Code, error.Message);

        return View(forgotPasswordVm);
    }

    [HttpGet]
    public IActionResult ResetPassword(ConfirmationDto confirmationDto)
    {
        var resetPasswordModel = new ResetPasswordVm(confirmationDto.Email, confirmationDto.Token, "");
        return View(resetPasswordModel);
    }

    [HttpPost]
    public async Task<IActionResult> ResetPassword(ResetPasswordVm resetPasswordVm)
    {
        var result = await accountService.ResetPasswordAsync(resetPasswordVm);
        if (!result.IsFailure) return LocalRedirect(result.Data);

        foreach (var error in result.Errors) ModelState.AddModelError(error.Code, error.Message);

        return View(resetPasswordVm);
    }

    public async Task<IActionResult> Logout()
    {
        await accountService.LogoutAsync();
        return RedirectToAction("Index", "Home");
    }

    public IActionResult AccessDenied()
    {
        return View();
    }
}