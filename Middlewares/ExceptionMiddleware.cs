using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using UserManagementApp.Dtos;

namespace UserManagementApp.Middlewares;

public class ExceptionMiddleware(
    RequestDelegate next,
    ILogger<ExceptionMiddleware> logger,
    IHostEnvironment hostEnvironment)
{
    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await next(httpContext);
        }
        catch (ArgumentNullException ex)
        {
            HandleException(httpContext, ex, "Invalid argument", HttpStatusCode.BadRequest);
        }
        catch (UnauthorizedAccessException ex)
        {
            HandleException(httpContext, ex, "Unauthorized access", HttpStatusCode.Unauthorized);
        }
        catch (Exception ex)
        {
            HandleException(httpContext, ex, "An unexpected error occurred.", HttpStatusCode.InternalServerError);
        }
    }

    private void HandleException(HttpContext httpContext, Exception ex, string errorMessage,
        HttpStatusCode statusCode)
    {
        logger.LogError(ex, ex.Message);
        Console.WriteLine("//////////////////////////////////////////////////////////");
        Console.WriteLine("//////////////////////////////////////////////////////////");
        Console.WriteLine($"TimeStamp: {DateTime.Now} \n ErrorMessage: {ex.Message}");
        Console.WriteLine("//////////////////////////////////////////////////////////");
        Console.WriteLine("//////////////////////////////////////////////////////////");

        var responseDto = ResponseDto.Failure(new[] { new Error("Server.Error", errorMessage) }, statusCode);

        // Store the ResponseDto in TempData
        var tempDataProvider = httpContext.RequestServices.GetRequiredService<ITempDataProvider>();
        var tempDataDictionary = new TempDataDictionary(httpContext, tempDataProvider);
        tempDataDictionary["ResponseDto"] = JsonSerializer.Serialize(responseDto);

        // Use URL Helper to generate the redirect URL
        var urlHelper = httpContext.RequestServices.GetRequiredService<IUrlHelperFactory>()
            .GetUrlHelper(new ActionContext
            {
                HttpContext = httpContext,
                RouteData = httpContext.GetRouteData(),
                ActionDescriptor = new ActionDescriptor()
            });

        var errorUrl = urlHelper.Action("Error", "Home", new { statusCode = (int)statusCode });

        // Write TempData to response before redirecting
        tempDataProvider.SaveTempData(httpContext, tempDataDictionary);

        httpContext.Response.Redirect(errorUrl!);
    }
}