using System.Net;

namespace AltenChancellery.Middleware
{
    internal record ExceptionResponse(HttpStatusCode StatusCode, string Description);

    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleException(context, ex);
            }
        }

        private async Task HandleException(HttpContext context, Exception ex)
        {
            // ADD ALL NEW EXCEPTION NEEDED HERE
            ExceptionResponse response = ex switch
            {
                // NOT FOUND EXCEPTIONS
                KeyNotFoundException _ => new ExceptionResponse(HttpStatusCode.NotFound, ex.Message),

                // BAD REQUEST EXCEPTIONS
                ArgumentNullException or ArgumentException _ => new ExceptionResponse(HttpStatusCode.BadRequest, ex.Message),

                // DEFAULT
                _ => new ExceptionResponse(HttpStatusCode.InternalServerError, ex.Message)
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)response.StatusCode;

            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
