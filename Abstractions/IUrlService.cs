namespace UserManagementApp.Abstractions;

public interface IUrlService
{
    public string GenerateUrl(string actionName, string controllerName, object? routeValues = null);
    public string GetRedirectUrl(string redirectUrl);
}