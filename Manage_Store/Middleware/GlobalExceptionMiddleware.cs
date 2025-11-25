using Microsoft.AspNetCore.Http;
using Manage_Store.Responses;
using Manage_Store.Exceptions;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace Manage_Store.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context); // Chạy request tiếp theo
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            int statusCode;
            string message = exception.Message;

            // Phân loại lỗi
            if (exception is NotFoundException)
            {
                statusCode = (int)HttpStatusCode.NotFound; // 404
            }
            else if (exception is BadRequestException)
            {
                statusCode = (int)HttpStatusCode.BadRequest; // 400
            }
            else
            {
                statusCode = (int)HttpStatusCode.InternalServerError; // 500
            }

            context.Response.StatusCode = statusCode;

            var response = ApiResponse<string>.Builder()
                .WithSuccess(false)
                .WithStatus(statusCode)
                .WithMessage(message)
                .Build();

            var json = JsonSerializer.Serialize(response);

            return context.Response.WriteAsync(json);
        }
    }
}
