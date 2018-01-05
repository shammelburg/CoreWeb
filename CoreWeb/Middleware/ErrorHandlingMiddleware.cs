using CoreWeb.Helpers;
using CoreWeb.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWeb.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly EmailSettings _settings;
        private IHostingEnvironment _env;

        public ErrorHandlingMiddleware(RequestDelegate next, IOptions<EmailSettings> settings, IHostingEnvironment env)
        {
            _next = next;
            _settings = settings.Value;
            _env = env;
        }

        public async Task Invoke(HttpContext context)
        {
            // This will catch all exceptions if not caught anywhere else
            try
            {
                await _next(context);
            }
            catch (SqlException ex)
            {
                EmailExceptionHelper.SendSqlException(ex, context.User.Identity.Name, _settings, _env);
                await HandleSqlExceptionAsync(context, ex);
            }
            catch (Exception ex)
            {
                EmailExceptionHelper.SendException(ex, context.User.Identity.Name, _settings, _env);
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var result = JsonConvert.SerializeObject(new
            {
                Type = "Exception",
                Exception = new
                {
                    Message = ex.Message,
                    Inner = ex.InnerException
                }
            });

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 500;
            return context.Response.WriteAsync(result);
        }

        private static Task HandleSqlExceptionAsync(HttpContext context, SqlException ex)
        {
            var errorList = new List<Object>();

            for (int i = 0; i < ex.Errors.Count; i++)
            {
                errorList.Add(new
                {
                    Message = ex.Errors[i].Message,
                    Procedure = ex.Errors[i].Procedure,
                    LineNumber = ex.Errors[i].LineNumber,
                    Source = ex.Errors[i].Source,
                    Server = ex.Errors[i].Server
                });
            }

            var result = JsonConvert.SerializeObject(new
            {
                Type = "SQL Exception",
                Exceptions = errorList
            });

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 500;
            return context.Response.WriteAsync(result);
        }
    }

    public static class ErrorHandlingMiddlewareExtensions
    {
        public static IApplicationBuilder UseErrorHandlingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorHandlingMiddleware>();
        }
    }
}
