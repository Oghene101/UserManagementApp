using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.IdentityModel.Tokens;
using UserManagementApp.Abstractions;

namespace UserManagementApp.Services;

public class UrlService(
    IHttpContextAccessor httpContextAccessor) : IUrlService
{
    public string GenerateUrl(string actionName, string controllerName, object? routeValues = null)
    {
        var context = httpContextAccessor.HttpContext;
        if (context == null)
            throw new InvalidOperationException("Cannot generate URL outside of an HTTP request context.");

        var actionContext = new ActionContext(context, context.GetRouteData(), new ActionDescriptor());
        var urlHelper = new UrlHelper(actionContext);

        return urlHelper.Action(actionName, controllerName, routeValues)!;
    }

    public string GetRedirectUrl(string redirectUrl)
    {
        return redirectUrl.IsNullOrEmpty() ? GenerateUrl("Index", "Home") : redirectUrl;
    }
}