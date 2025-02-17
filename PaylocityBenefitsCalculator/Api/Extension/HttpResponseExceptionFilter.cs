﻿using Api.Models;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;

namespace Api.Extension
{
    /// <summary>
    /// Create Exception Filter to catch all exception using built in exception provider
    /// </summary>
    public static class HttpResponseExceptionFilter
    {
        public static void ConfigureCustomExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        await context.Response.WriteAsync(new ErrorDetails()
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = contextFeature.Error.Message.ToString(),
                            Error = contextFeature.Error.ToString(),
                        }.ToString());
                    }
                });
            });
        }
    }
}
