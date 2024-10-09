using System.Net;

namespace NZWalks.API.Middleares
{
    public class ExceptionHandlerMiddleWare
    {
        private readonly ILogger<ExceptionHandlerMiddleWare> logger;
        private readonly RequestDelegate next;

        public ExceptionHandlerMiddleWare(ILogger<ExceptionHandlerMiddleWare> logger,RequestDelegate next)
        {
            this.logger = logger;
            this.next = next;
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await next(httpContext);
            }
            catch (Exception ex)
            {
                var errorId=Guid.NewGuid();
                //Log this exception

                logger.LogError(ex,$"{errorId} : {ex.Message}");

                //Return a custom response
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                httpContext.Response.ContentType = "application/json";

                var error = new
                {
                    Id=errorId,
                    ErrorMessage="something went wrong!We are looking into it."
                };
                await httpContext.Response.WriteAsJsonAsync(error);
            }
        }
    }
}
