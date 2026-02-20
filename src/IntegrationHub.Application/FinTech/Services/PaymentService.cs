using IntegrationHub.Application.FinTech.DTOs;
using IntegrationHub.Application.FinTech.InboundPorts;
using IntegrationHub.Domain.FinTech.Enums;
using IntegrationHub.Domain.FinTech.Events;
using IntegrationHub.Domain.FinTech.OutboundPorts;

namespace IntegrationHub.Application.FinTech.Services
{
    public class PaymentService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IPaymentGateway _paymentGateway;
        private readonly IEventBus _eventBus;
        private readonly IPaymentWebHookHandler _paymentWebhookHandler;

        public PaymentService(IAccountRepository accountRepository, IPaymentGateway paymentGatway, IEventBus eventBus, IPaymentWebHookHandler paymentWebhookHandler)
        {
            _accountRepository = accountRepository;
            _paymentGateway = paymentGatway;
            _eventBus = eventBus;
            _paymentWebhookHandler = paymentWebhookHandler;
        }

        public async Task<ProcessPaymentResult> ProcessPayment(ProcessPaymentCommand command)
        {
            var account = await _accountRepository.GetByIdAsync(command.accountID);
            var payment = account.MakePayment(command.amount, command.description, command.currency, command.merchantId, command.merchantId);

            var externalResult = _paymentGateway.Charge(payment);

            if (externalResult.Success)
            {
                account.AttachPaymentExternalId(payment.PaymentId.ToString(), externalResult.ExternalTransactionId!);
            }
            else
            {
                account.FailPayment(payment, externalResult.ErrorMessage);
            }

            await _accountRepository.SaveAsync(account);

            return new ProcessPaymentResult
            (
                payment.AccountId,
                externalResult.PaymentId,
                externalResult.ExternalTransactionId,
                externalResult.Success,
                externalResult.Status,
                payment.Amount,
                payment.Currency,
                externalResult.Success ? string.Format($"Payment {externalResult.Status.ToString()}") : externalResult.ErrorMessage,
                externalResult.CheckOutUrl
            );
        }

        public async Task ProcessWebhook(string payload, string signature)
        {
            var result = await _paymentWebhookHandler.HandleAsync(payload, signature);
            if ((result != null) && (result.Status == PaymentStatus.Completed))
            {
                Guid.TryParse(result.AccountId, out Guid id);
                var account = await _accountRepository.GetByIdAsync(id);
                account.CompletePayment(result.PaymentId);

                await _accountRepository.SaveAsync(account);
            }
            
        }
    }
}
