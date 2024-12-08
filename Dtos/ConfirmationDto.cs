using System.ComponentModel.DataAnnotations;

namespace UserManagementApp.Dtos;

public record ConfirmationDto(
    [EmailAddress] string Email,
    string Token);