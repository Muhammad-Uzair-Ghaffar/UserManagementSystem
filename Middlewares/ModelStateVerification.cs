using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace UserManagementSystem.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class ModelStateVerification
    {
        private readonly RequestDelegate _next;

        public ModelStateVerification(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext)
        {

            return _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class ModelStateVerificationExtensions
    {
        public static IApplicationBuilder UseModelStateVerification(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ModelStateVerification>();
        }
    }
}
