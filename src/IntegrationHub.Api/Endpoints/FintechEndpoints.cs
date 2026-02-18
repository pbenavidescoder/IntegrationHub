using IntegrationHub.Api.DTOs.FinTech;
using IntegrationHub.Application.FinTech.DTOs;
using IntegrationHub.Application.FinTech.InboundPorts;
using IntegrationHub.Application.FinTech.OutboundPorts;
//using Stripe;
//using AccountService = IntegrationHub.Application.FinTech.Services.AccountService;
using IntegrationHub.Application.FinTech.Services;

namespace IntegrationHub.Api.Endpoints
{
    public static class FintechEndpoints
    {
        public static void MapFintechEndpoints(this WebApplication app)
        {
            app.MapGet("fintech/accounts", (AccountService service) =>
            {
                var accounts = service.ListAccounts();
                if (!accounts.Any())
                    return Results.NoContent();

                var response = accounts.Select(a => new AccountResponse
                    {
                        AccountId = a.AccountId,
                        OwnerName = a.Owner,
                        Email = a.EmailAddress,
                        Balance = a.Balance,
                        Currency = a.Currency
                    });
           
                    return Results.Ok(response);
            })
                .WithTags("Fintech Hub");

            app.MapPost("fintech/accounts", async (CreateAccountRequest request, AccountService service) =>
            {
                var command = new CreateAccountCommand(request.OwnerName, request.Email ?? String.Empty, request.Currency, request.InitialAmount);

                var result = await service.CreateAccount(command);

                var response = new CreateAccountResponse
                {
                    AccountId = result.AccountId,
                    Balance = result.Balance,
                    OwnerName = result.Owner
                };
                return Results.Ok(response);
            })
                .WithTags("Fintech Hub");

            app.MapPost("fintech/deposits", async (DepositFundsRequest request, AccountService service) =>
            {
                var command = new DepositFundsCommand(request.AccountId, request.Amount);

                var result = await service.DepositFunds(command);

                var response = new DepositFundsResponse
                {
                    AccountId = result.AccountId,
                    NewBalance = result.NewBalance
                };
                return Results.Ok(response);
            })
                .WithTags("Fintech Hub");

            app.MapPost("fintech/withdraws", async (WithdrawFundsRequest request, AccountService service) =>
            {
                var command = new WithdrawFundsCommand(request.AccountId, request.Amount, request.Description ?? String.Empty);

                var result = await service.WithdrawFunds(command);
                var response = new WithdrawFundsResponse
                {
                    AccountId = result.AccountId,
                    NewBalance = result.Balance
                };
                return Results.Ok(response);
            })
                .WithTags("Fintech Hub");

            app.MapPost("fintech/payments", async (PaymentRequest request, PaymentService service) =>
            {
                var command = new ProcessPaymentCommand(request.AccountId, request.Amount, request.Description ?? String.Empty, request.Currency, request.PaymentMethod, request.MerchantId ?? String.Empty);

                var result = await service.ProcessPayment(command);
                // TODO: Validate error result

                return Results.Ok(new PaymentResponse
                {
                    PaymentId = result.paymentId,
                    ExternalTransactionId = result.externalTransactionId,
                    Success = result.success,
                    Message = result.message,
                    CheckOutUrl = result.checkOutUr
                }); 
            })
                .WithTags("Fintech Hub"); 

            app.MapGet("fintech/payments/success", async (string session_id, IPaymentQueryService queryservice) =>
            {
                var paymentDetails = await queryservice.GetPaymentBySessionIdAsync(session_id);
                return Results.Ok(paymentDetails);
            })
                .WithTags("Fintech Hub");

            app.MapGet("fintech/payments/cancel", async (string session_id, IPaymentQueryService queryservice) =>
            {
                var paymentDetails = await queryservice.GetPaymentBySessionIdAsync(session_id);
                return Results.Ok(new
                {
                    paymentDetails.SessionId,
                    Status = "cancelled",
                    paymentDetails.Currency,
                    AmountTotal = paymentDetails.Amount
                });
            })
                .WithTags("Fintech Hub");

            app.MapPost("fintech/payments/webhook", async (HttpRequest request, PaymentService service) =>
            {
                using var reader = new StreamReader(request.Body);


                var json = await reader.ReadToEndAsync();
                var signature = request.Headers["Stripe-Signature"];
                if (string.IsNullOrWhiteSpace(signature))
                    return Results.BadRequest(new { error = "signature missing"});

                await service.ProcessWebhook(json, signature!);
                return Results.Ok();
            })
                .WithTags("Fintech Hub");
        }
    }
}
