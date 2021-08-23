using KSE.Payment.Facade.Interfaces;
using KSE.Payment.KlingerPag.Models.Enum;
using KSE.Payment.KlingerPag.Service;
using KSE.Payment.Models;
using KSE.Payment.Models.Enums;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace KSE.Payment.Facade
{
    public class PaymentCardCreditFacade : IPaymentFacade
    {
        private readonly PaymentConfiguration _paymentConfig;

        public PaymentCardCreditFacade(IOptions<PaymentConfiguration> paymentConfiguration)
        {
            _paymentConfig = paymentConfiguration.Value;
        }

        public async Task<Transaction> AuthorizePayment(Models.Payment payment)
        {
            var nerdsPagSvc = new KlingerPagService(_paymentConfig.DefaultApiKey,
                _paymentConfig.DefaultEncryptionKey);

            var cardHashGen = new CardHashCode(nerdsPagSvc)
            {
                CardNumber = payment.CreditCart.NumberCart,
                CardHolderName = payment.CreditCart.NameCart,
                CardExpirationDate = payment.CreditCart.ExpirationCart,
                CardCvv = payment.CreditCart.CVV
            };
            var cardHash = await cardHashGen.GenerateAsync();

            var transacao = new KlingerPag.Models.Transaction(nerdsPagSvc)
            {
                CardHash = cardHash,
                CardNumber = payment.CreditCart.NumberCart,
                CardHolderName = payment.CreditCart.NameCart,
                CardExpirationDate = payment.CreditCart.ExpirationCart,
                CardCvv = payment.CreditCart.CVV,
                PaymentMethod = PaymentMethod.CreditCard,
                Amount = payment.Value
            };

            return ForTransaction(await transacao.AuthorizeCardTransaction());
        }

        public async Task<Transaction> CanceledPayment(Transaction transaction)
        {
            var nerdsPagSvc = new KlingerPagService(_paymentConfig.DefaultApiKey,
                _paymentConfig.DefaultEncryptionKey);

            var tran = ForKlingerPagTransaction(transaction, nerdsPagSvc);

            return ForTransaction(await tran.CancelAuthorization());
        }

        public async Task<Transaction> CapturePayment(Transaction transaction)
        {
            var nerdsPagSvc = new KlingerPagService(_paymentConfig.DefaultApiKey,
                _paymentConfig.DefaultEncryptionKey);

            var tran = ForKlingerPagTransaction(transaction, nerdsPagSvc);

            return ForTransaction(await tran.CaptureCardTransaction());
        }

        public static Transaction ForTransaction(KlingerPag.Models.Transaction transaction)
        {
            return new Transaction
            {
                Id = Guid.NewGuid(),
                Status = (StatusTransaction)transaction.Status,
                TotalValue = transaction.Amount,
                BrandCart = transaction.CardBrand,
                CodeAuthorization = transaction.AuthorizationCode,
                CostTransaction = transaction.Cost,
                DateTransaction = transaction.TransactionDate,
                NSU = transaction.Nsu,
                TID = transaction.Tid
            };
        }

        public static KlingerPag.Models.Transaction ForKlingerPagTransaction(Transaction transacao, KlingerPagService nerdsPagService)
        {
            return new KlingerPag.Models.Transaction(nerdsPagService)
            {
                Status = (TransactionStatus)transacao.Status,
                Amount = transacao.TotalValue,
                CardBrand = transacao.BrandCart,
                AuthorizationCode = transacao.CodeAuthorization,
                Cost = transacao.CostTransaction,
                Nsu = transacao.NSU,
                Tid = transacao.TID
            };
        }
    }
}
