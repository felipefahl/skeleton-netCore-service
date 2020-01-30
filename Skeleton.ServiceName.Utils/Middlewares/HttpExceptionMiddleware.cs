using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Skeleton.ServiceName.Utils.Exceptions;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Skeleton.ServiceName.Utils.Middlewares
{
    public class HttpExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<HttpExceptionMiddleware> _logger;

        public HttpExceptionMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _logger = loggerFactory?.CreateLogger<HttpExceptionMiddleware>() ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (HttpStatusCodeException ex)
            {
                if (context.Response.HasStarted)
                {
                    _logger.LogWarning("The response has already started, the http status code middleware will not be executed.");
                    throw;
                }

                context.Response.Clear();
                context.Response.StatusCode = ex.StatusCode;
                context.Response.ContentType = ex.ContentType;

                await context.Response.WriteAsync(ex.Message);

                return;
            }
            catch (ValidationException ex)
            {
                if (context.Response.HasStarted)
                {
                    _logger.LogWarning("The response has already started, the http status code middleware will not be executed.");
                    throw;
                }

                context.Response.Clear();
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                context.Response.ContentType = @"application/json";

                await context.Response.WriteAsync(ex.Message);

                return;
            }
            catch (Exception ex)
            {
                if (context.Response.HasStarted)
                {
                    _logger.LogWarning("The response has already started, the http status code middleware will not be executed.");
                    throw;
                }

                context.Response.Clear();
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = @"application/json";

                await context.Response.WriteAsync(ex.Message);

                return;
            }
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class HttpExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseHttpExceptionMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<HttpExceptionMiddleware>();
        }
    }
}
