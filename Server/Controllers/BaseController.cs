using Core.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Server.Controllers
{
    public abstract class BaseController : ControllerBase
    {
        protected string GetTokenFromHeaders()
        {
            if (!Request.Headers.TryGetValue("Authorization", out var header))
                throw new UnauthorizedException();

            var parts = header.ToString().Split(' ', StringSplitOptions.RemoveEmptyEntries);

            return (
                parts.Length == 2 && parts[0].Equals("Bearer", StringComparison.OrdinalIgnoreCase)
            )
                ? parts[1]
                : throw new UnauthorizedException();
        }
    }

    public sealed class ApiExceptionFilter : IAsyncExceptionFilter
    {
        public Task OnExceptionAsync(ExceptionContext context)
        {
            if (context.Exception is ApiException apiEx)
            {
                context.Result = new ObjectResult(new { detail = apiEx.Message })
                {
                    StatusCode = apiEx.StatusCode,
                };

                context.ExceptionHandled = true;
            }

            return Task.CompletedTask;
        }
    }
}
