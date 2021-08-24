using KSE.Payment.Facade.Interfaces;
using KSE.Payment.KlingerPag.Models;
using KSE.Payment.KlingerPag.Service;
using KSE.Payment.Models;
using KSE.Payment.Models.Enums;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KSE.Payment.Facade
{
    public class PaymentCardCreditFacade : IPaymentFacade
    {
        private readonly PaymentConfiguration _paymentConfig;
        private readonly IKlingerPagService _klingerPagService;

        public PaymentCardCreditFacade(IOptions<PaymentConfiguration> paymentConfiguration, IKlingerPagService klingerPagService)
        {
            _paymentConfig = paymentConfiguration.Value;
            _klingerPagService = klingerPagService;
        }

        public async Task<Transaction> AuthorizePayment(Models.Payment payment)
        {
            return ForTransaction(await _klingerPagService.PerformingTransaction(ForKlingerPagTransaction(payment, _paymentConfig.DefaultApiKey)));
        }

        public async Task<Transaction> CanceledPayment(string tid)
        {
            return ForTransaction(await _klingerPagService.CanceledTransaction(tid, _paymentConfig.DefaultApiKey));
        }

        public async Task<Transaction> CapturingPayment(string tid)
        {
            return ForTransaction(await _klingerPagService.CapturingTransactionLater(tid, _paymentConfig.DefaultApiKey));
        }

        private static Transaction ForTransaction(TransactionResponse transaction)
        {
            try
            {
                return new Transaction
                {
                    Id = Guid.NewGuid(),
                    Status = Enum.Parse<StatusTransaction>(transaction.Status),
                    TotalValue = transaction.Amount,
                    BrandCart = transaction.Card_brand,
                    CodeAuthorization = transaction.Authorization_code,
                    CostTransaction = transaction.Cost,
                    DateTransaction = transaction.Date_created,
                    NSU = transaction.Nsu.ToString(),
                    TID = transaction.Tid.ToString()
                };
            }
            catch (Exception e)
            {
                throw;
            }
            
        }

        private static TransactionViewModel ForKlingerPagTransaction(Models.Payment payment, string key)
        {
            TransactionViewModel transactionView = new TransactionViewModel();

            transactionView.api_key = key;
            transactionView.capture = false;
            transactionView.amount = payment.Value.ToString().Remove(payment.Value.ToString().Length - 3, 1); ;
            transactionView.card_number = payment.CreditCart.NumberCart;
            transactionView.card_cvv = payment.CreditCart.CVV;
            transactionView.card_expiration_date = payment.CreditCart.ExpirationCart.Remove(2,1);
            transactionView.card_holder_name = payment.CreditCart.NameCart;

            CustomerViewModel customer = new CustomerViewModel();

            customer.external_id = payment.ClientId.ToString();
            customer.name = payment.ClientName;
            customer.type = "individual";
            customer.country = "br";
            customer.email = payment.ClientEmail;
            customer.birthday = DateTime.Now.ToString("yyyy-MM-dd");

            transactionView.customer = customer;

            List<DocumentViewModel> document = new List<DocumentViewModel>();
            document.Add(new DocumentViewModel { number = payment.ClientDocument, type = "cpf" });
            transactionView.customer.documents = document;

            List<string> phone = new List<string>();
            phone.Add(payment.ClientPhone);
            transactionView.customer.phone_numbers = phone;

            BillingViewModel billing = new BillingViewModel();
            billing.name = payment.ClientName;

            AddressViewModel address = new AddressViewModel();
            address.country = "br";
            address.state = payment.State;
            address.city = payment.City;
            address.neighborhood = payment.Neighborhood;
            address.street = payment.Street;
            address.street_number = payment.Number;
            address.zipcode = payment.Zipcode;

            billing.address = address;

            transactionView.billing = billing;

            ShippingViewModel shipping = new ShippingViewModel();
            shipping.name = payment.ClientName;
            shipping.fee = payment.Value.ToString().Remove(2, 1);
            shipping.delivery_date = DateTime.Now.AddDays(10).Date.ToString("yyyy-MM-dd");
            shipping.expedited = false;

            AddressViewModel addressShipping = new AddressViewModel();
            addressShipping.country = "br";
            addressShipping.state = payment.State;
            addressShipping.city = payment.City;
            addressShipping.neighborhood = payment.Neighborhood;
            addressShipping.street = payment.Street;
            addressShipping.street_number = payment.Number;
            addressShipping.zipcode = payment.Zipcode;

            shipping.address = address;

            transactionView.shipping = shipping;

            transactionView.items = payment.Itens;

            return transactionView;
        }
    }
}
