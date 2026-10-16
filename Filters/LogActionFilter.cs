using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ShopApi.Filters;

public class LogActionFilter : IAsyncActionFilter
{
    private readonly ILogger<LogActionFilter> _logger;

    public LogActionFilter(ILogger<LogActionFilter> logger) => _logger = logger;

    public async Task OnActionExecutionAsync(
        ActionExecutingContext context,
        ActionExecutionDelegate next)
    {
        var action = context.ActionDescriptor.DisplayName;
        var args = string.Join(", ",
            context.ActionArguments.Select(a => $"{a.Key}={a.Value}"));

        _logger.LogInformation("→ {Action} ({Args})", action, args);

        var sw = Stopwatch.StartNew();
        await next();
        sw.Stop();

        _logger.LogInformation(
            "← {Action} ({Elapsed}ms)", action, sw.ElapsedMilliseconds);
    }
}