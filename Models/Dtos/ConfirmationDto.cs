using System.ComponentModel.DataAnnotations;

namespace UserManagementApp.Models.Dtos;

public record ConfirmationDto(
    [EmailAddress] string Email,
    string Token);