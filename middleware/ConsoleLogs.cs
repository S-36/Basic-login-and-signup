using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace Login_and_Signup.middleware
{
    public class ConsoleLogs
    {
        private readonly RequestDelegate _next;
        // We add Ilogger to native .net logs 
        private readonly ILogger<ConsoleLogs> _logger;

        public ConsoleLogs(RequestDelegate next, ILogger<ConsoleLogs> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Stopwatch para medir cuánto tarda cada petición
            var stopwatch = Stopwatch.StartNew();

            // Capturamos los datos de entrada ANTES de procesar
            var method = context.Request.Method;
            var path = context.Request.Path;
            var requestTime = DateTime.UtcNow; 

            _logger.LogInformation(
                "[REQUEST] {Method} {Path} | Started: {Time}",
                method, path, requestTime
            );

            try
            {
                // Aquí se ejecuta el resto del pipeline (controllers, etc)
                await _next(context);
            }
            catch (Exception ex)
            {
                // Si algo explota sin ser capturado, lo registramos aquí
                stopwatch.Stop();
                _logger.LogError(
                    ex,
                    "[UNHANDLED ERROR] {Method} {Path} | Duration: {Duration}ms",
                    method, path, stopwatch.ElapsedMilliseconds
                );
                throw; // Re-lanzamos para no ocultar el error
            }

            // Obtenemos el status code 
            stopwatch.Stop();
            var statusCode = context.Response.StatusCode;
            var duration = stopwatch.ElapsedMilliseconds;

            // Se envia el log segun el status code
            if (statusCode >= 500)
            {
                _logger.LogError(
                    "[RESPONSE] {Method} {Path} | Status: {StatusCode} | Duration: {Duration}ms",
                    method, path, statusCode, duration
                );
            }
            else if (statusCode >= 400)
            {
                _logger.LogWarning(
                    "[RESPONSE] {Method} {Path} | Status: {StatusCode} | Duration: {Duration}ms",
                    method, path, statusCode, duration
                );
            }
            else
            {
                _logger.LogInformation(
                    "[RESPONSE] {Method} {Path} | Status: {StatusCode} | Duration: {Duration}ms",
                    method, path, statusCode, duration
                );
            }
        }
    }

}