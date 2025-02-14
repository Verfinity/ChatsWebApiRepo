using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace ChatsWebApi.Components.Middlewares
{
    public class ValidationExceptionMiddleware
    {
        private readonly RequestDelegate _requestDelegate;
        public ValidationExceptionMiddleware(RequestDelegate requestDelegate)
        {
            _requestDelegate = requestDelegate;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _requestDelegate.Invoke(context);
            }
            catch (ValidationException ex)
            {
                var problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Incorrect data format",
                    Type = "ValidationException"
                };
                problemDetails.Extensions["Errors"] = ex.Errors;

                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsJsonAsync(problemDetails);
            }
        }
    }
}
