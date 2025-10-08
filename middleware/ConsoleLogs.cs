using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
namespace Login_and_Signup.middleware
{
    public class ConsoleLogs
    {
        private readonly RequestDelegate _next;

        public ConsoleLogs(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            Console.WriteLine($"{DateTime.Now} - Request: {context.Request.Method} {context.Request.Path} - Status: {context.Response.StatusCode}");
            await _next(context);
            if (context.Response.StatusCode >= 400)
            {
                Console.WriteLine($"{DateTime.Now} - Error Response: {context.Response.StatusCode} for {context.Request.Method} {context.Request.Path}");
            }
        }
    }

}