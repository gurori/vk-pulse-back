// Server/Extensions/ApiExceptionFilter.cs
using Core.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace Server.Extensions
{
    public class ApiExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
            string message = "An unexpected error occurred.";

            switch (context.Exception)
            {
                case UnauthorizedException unauthorizedException:
                    statusCode = HttpStatusCode.Unauthorized;
                    message = unauthorizedException.Message;
                    break;
                case ConflictException conflictException:
                    statusCode = HttpStatusCode.Conflict;
                    message = conflictException.Message;
                    break;
                case NotFoundException notFoundException:
                    statusCode = HttpStatusCode.NotFound;
                    message = notFoundException.Message;
                    break;
                case System.UnauthorizedAccessException unauthorizedAccessException: // Для специфических ошибок доступа
                    statusCode = HttpStatusCode.Forbidden; // Или Unauthorized, в зависимости от контекста
                    message = unauthorizedAccessException.Message;
                    break;
                case System.InvalidOperationException invalidOperationException: // Для уже завершенных задач
                    statusCode = HttpStatusCode.BadRequest;
                    message = invalidOperationException.Message;
                    break;
                    // Добавьте другие кастомные исключения
            }

            context.Result = new ObjectResult(new { error = message })
            {
                StatusCode = (int)statusCode
            };
            context.ExceptionHandled = true;
        }
    }
}
