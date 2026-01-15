using IntegrationHub.Domain.FinTech.Exceptions;

namespace IntegrationHub.Api.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next) => _next = next;

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (DomainException ex)
            {
                context.Response.StatusCode = ex switch
                {
                    AccountNotFoundException => StatusCodes.Status404NotFound,
                    InsufficientFundsException => StatusCodes.Status422UnprocessableEntity,
                    InvalidDepositAmountException => StatusCodes.Status400BadRequest,
                    _ => StatusCodes.Status400BadRequest
                };

                await context.Response.WriteAsJsonAsync(new
                {
                    message = ex.Message,
                    errorCode = ex.ErrorCode
                });
            }
            catch (Exception ex)
            {
                await context.Response.WriteAsJsonAsync(new
                {
                    message = "Unexpected error",
                    errorCode = "INTERNAL_ERROR",
                    detail = ex.Message
                });
            }
        }
    }
}
