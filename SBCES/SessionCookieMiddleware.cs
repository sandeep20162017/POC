using Microsoft.AspNetCore.Http;
using System.Text.Json;
using System.Threading.Tasks;

public class SessionCookieMiddleware
{
    private readonly RequestDelegate _next;

    public SessionCookieMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Check if the session cookie exists
        var sessionCookie = context.Request.Cookies["SessionCookie"];
        if (!string.IsNullOrEmpty(sessionCookie))
        {
            // Deserialize the session data from the cookie
            var sessionData = JsonSerializer.Deserialize<Dictionary<string, string>>(sessionCookie);
            if (sessionData != null)
            {
                foreach (var item in sessionData)
                {
                    context.Session.SetString(item.Key, item.Value);
                }
            }
        }

        // Call the next middleware in the pipeline
        await _next(context);

        // Serialize the session data to a cookie
        var sessionDataToSerialize = new Dictionary<string, string>();
        foreach (var key in context.Session.Keys)
        {
            sessionDataToSerialize[key] = context.Session.GetString(key);
        }

        var serializedSessionData = JsonSerializer.Serialize(sessionDataToSerialize);
        context.Response.Cookies.Append("SessionCookie", serializedSessionData);
    }
}

public static class SessionCookieMiddlewareExtensions
{
    public static IApplicationBuilder UseSessionCookieMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<SessionCookieMiddleware>();
    }
}