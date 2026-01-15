using IntegrationHub.Api.DTOs.FinTech;
using IntegrationHub.Application.FinTech.DTOs;
using IntegrationHub.Application.FinTech.Services;
using Microsoft.AspNetCore.Builder;

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
            });

            app.MapPost("fintech/accounts", (CreateAccountRequest request, AccountService service) =>
            {
                var command = new CreateAccountCommand(request.OwnerName, request.Email ?? String.Empty, request.Currency, request.InitialAmount);

                var result = service.CreateAccount(command);

                var response = new CreateAccountResponse
                {
                    AccountId = result.AccountId,
                    Balance = result.Balance,
                    OwnerName = result.Owner
                };
                return Results.Ok(response);
            });

            app.MapPost("fintech/deposits", (DepositFundsRequest request, AccountService service) =>
            {
                var command = new DepositFundsCommand(request.AccountId, request.Amount);

                var result = service.DepositFunds(command);

                var response = new DepositFundsResponse
                {
                    AccountId = result.AccountId,
                    NewBalance = result.NewBalance
                };
                return Results.Ok(response);
            });

            app.MapPost("fintech/withdraws", (WithdrawFundsRequest request, AccountService service) =>
            {
                var command = new WithdrawFundsCommand(request.AccountId, request.Amount, request.Description ?? String.Empty);

                var result = service.WithdrawFunds(command);
                var response = new WithdrawFundsResponse
                {
                    AccountId = result.AccountId,
                    NewBalance = result.Balance
                };

            });
        }
    }
}
