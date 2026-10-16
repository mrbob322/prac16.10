using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ShopApi.Filters;

public class GlobalExceptionFilter : IExceptionFilter
{
    private readonly ILogger<GlobalExceptionFilter> _logger;

    public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
        => _logger = logger;

    public void OnException(ExceptionContext context)
    {
        _logger.LogError(context.Exception,
            "Ошибка в {Action}",
            context.ActionDescriptor.DisplayName);

        context.Result = new ObjectResult(new
        {
            message = "Внутренняя ошибка сервера",
            code = "INTERNAL_ERROR"
        })
        {
            StatusCode = 500
        };

        context.ExceptionHandled = true;
    }
}