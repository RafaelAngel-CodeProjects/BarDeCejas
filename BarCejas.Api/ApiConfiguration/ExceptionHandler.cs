using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;

using Newtonsoft.Json;

using System;
using System.Net;
using System.Threading.Tasks;

namespace BarCejas.Api.ApiConfiguration
{
    public class ExceptionHandler
    {
        private readonly RequestDelegate _next;
        private readonly IStringLocalizer<Program> _localizer;

        public ExceptionHandler(
            RequestDelegate next,
            IStringLocalizer<Program> localizer)
        {
            this._localizer = localizer;
            this._next = next;
        }

        // ReSharper disable once UnusedMember.Global
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await this._next(context);
            }
            catch (Exception ex)
            {
                await this.InternalErrorException(context, ex, HttpStatusCode.InternalServerError);
            }
        }

        private Task InternalErrorException(HttpContext context, Exception ex, HttpStatusCode statusCode)
        {
            Exception baseException = ex.GetBaseException();
            string result = JsonConvert.SerializeObject(new
            {
                message = this.GetMessage(baseException.Message),
                detail = ex.GetBaseException().StackTrace
            });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;
            return context.Response.WriteAsync(result);
        }

        private string GetMessage(string key)
        {
            LocalizedString message = this._localizer.GetString(key);
            return !message.ResourceNotFound ? message.Value : $"Error message missing: {key}";
        }
    }
}
