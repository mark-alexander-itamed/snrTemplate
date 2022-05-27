using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BugReporter.Api.Infrastructure
{
    public class RequestLatencyFilter : IAsyncActionFilter
    {
        private static readonly Random Random = new();
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            /*
             * When developing you don't get a sense of how loading really works.
             * This should give you a better idea, by adding some slowness into the request.
             */

            var delay = Random.Next(50, 500);

            await Task.Delay(delay);
            await next();
        }
    }
}