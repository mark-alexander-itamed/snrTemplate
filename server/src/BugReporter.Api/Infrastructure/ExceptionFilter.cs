using BugReporter.Api.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BugReporter.Api.Infrastructure
{
    public class ExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var exception = context.Exception;

            var asViewModel = new ExceptionViewModel
            {
                Message = exception.Message
            };

            var result = new ObjectResult(asViewModel)
            {
                StatusCode = 500
            };

            context.Result = result;
        }
    }
}