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

        public async Task<ReturnPayment> AuthorizePayment(Models.Payment payment)
        {
            var response = await _klingerPagService.PerformingTransaction(ForKlingerPagTransaction(payment, _paymentConfig.DefaultApiKey));

            if (response.TransactionResponse == null)
            {
                return new ReturnPayment { Error = ForErro(response) };
            }

            return new ReturnPayment { Transaction = ForTransaction(response) };
        }

        public async Task<ReturnPayment> CanceledPayment(string tid)
        {
            var response = await _klingerPagService.CanceledTransaction(tid, _paymentConfig.DefaultApiKey);

            if (response.TransactionResponse == null)
            {
                return new ReturnPayment { Error = ForErro(response) };
            }

            return new ReturnPayment { Transaction = ForTransaction(response) };
        }

        public async Task<ReturnPayment> CapturingPayment(string tid)
        {
            var response = await _klingerPagService.CapturingTransactionLater(tid, _paymentConfig.DefaultApiKey);

            if (response.TransactionResponse == null)
            {
                return new ReturnPayment { Error = ForErro(response) };
            }

            return new ReturnPayment { Transaction = ForTransaction(response) };
        }

        private static Transaction ForTransaction(PaymentResponse response)
        {
            return new Transaction
            {
                Id = Guid.NewGuid(),
                Status = Enum.Parse<StatusTransaction>(response.TransactionResponse.Status),
                TotalValue = response.TransactionResponse.Amount,
                BrandCart = response.TransactionResponse.Card_brand,
                CodeAuthorization = response.TransactionResponse.Authorization_code,
                CostTransaction = response.TransactionResponse.Cost,
                DateTransaction = response.TransactionResponse.Date_created,
                NSU = response.TransactionResponse.Nsu.ToString(),
                TID = response.TransactionResponse.Tid.ToString()
            };
        }
        private static ReturnError ForErro(PaymentResponse response)
        {
            ReturnError returnError = new ReturnError();
            returnError.method = response.TransactionError.method;
            returnError.url = response.TransactionError.method;

            foreach (var item in response.TransactionError.errors)
            {
                returnError.errors.Add(new Models.Errors
                {
                    message = item.message,
                    parameter_name = item.parameter_name,
                    type = item.type
                });

            }
            return returnError;
        }

        private static TransactionViewModel ForKlingerPagTransaction(Models.Payment payment, string key)
        {
            TransactionViewModel transactionView = new TransactionViewModel();

            transactionView.api_key = key;
            transactionView.capture = false;
            transactionView.amount = payment.Value.ToString().Remove(payment.Value.ToString().Length - 3, 1);
            transactionView.card_number = payment.CreditCart.NumberCart;
            transactionView.card_cvv = payment.CreditCart.CVV;
            transactionView.card_expiration_date = payment.CreditCart.ExpirationCart.Remove(2, 1);
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
            shipping.fee = "0";
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
