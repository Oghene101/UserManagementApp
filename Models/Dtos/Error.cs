namespace UserManagementApp.Models.Dtos;

public class Error(string code, string message)
{
    public static readonly IEnumerable<Error> None = [];
    public static readonly Error NullValue = new("Error.NullValue", "The specified value is null.");

    public string Code { get; set; } = code;
    public string Message { get; set; } = message;
}