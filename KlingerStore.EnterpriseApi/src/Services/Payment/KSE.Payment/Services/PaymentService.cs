using FluentValidation.Results;
using KSE.Core.DomainObjets;
using KSE.Core.Messages.IntegrationEvents;
using KSE.Payment.Facade.Interfaces;
using KSE.Payment.Interfaces;
using KSE.Payment.Models.Enums;
using KSE.Payment.Services.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace KSE.Payment.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentFacade _pagamentoFacade;
        private readonly IPaymentRepository _pagamentoRepository;

        public PaymentService(IPaymentFacade pagamentoFacade,
                                IPaymentRepository pagamentoRepository)
        {
            _pagamentoFacade = pagamentoFacade;
            _pagamentoRepository = pagamentoRepository;
        }

        public async Task<ResponseMessage> AutorizarPagamento(Models.Payment payment)
        {
            var transacao = await _pagamentoFacade.AuthorizePayment(payment);
            var validationResult = new ValidationResult();

            if (transacao.Status != StatusTransaction.authorized)
            {
                validationResult.Errors.Add(new ValidationFailure("Pagamento",
                        "Pagamento recusado, entre em contato com a sua operadora de cartão"));

                return new ResponseMessage(validationResult);
            }

            payment.AdicionarTransacao(transacao);
            _pagamentoRepository.InsertPayment(payment);


            if (!await _pagamentoRepository.UnitOfWork.Commit())
            {
                validationResult.Errors.Add(new ValidationFailure("Pagamento",
                    "Houve um erro ao realizar o pagamento."));

                // Cancelar pagamento no gateway
                await CancelarPagamento(payment.OrderId);

                return new ResponseMessage(validationResult);
            }

            return new ResponseMessage(validationResult);

        }

        public async Task<ResponseMessage> CancelarPagamento(Guid pedidoId)
        {
            var transacoes = await _pagamentoRepository.FindTransactionPerOrderId(pedidoId);
            var transacaoAutorizada = transacoes?.FirstOrDefault(t => t.Status == StatusTransaction.authorized);
            var validationResult = new ValidationResult();

            if (transacaoAutorizada == null) throw new DomainException($"Transação não encontrada para o pedido {pedidoId}");

            var transacao = await _pagamentoFacade.CanceledPayment(transacaoAutorizada.TID);

            if (transacao.Status != StatusTransaction.cancelled)
            {
                validationResult.Errors.Add(new ValidationFailure("Pagamento",
                    $"Não foi possível cancelar o pagamento do pedido {pedidoId}"));

                return new ResponseMessage(validationResult);
            }

            transacao.PaymentId = transacaoAutorizada.PaymentId;
            _pagamentoRepository.InsertTransaction(transacao);

            if (!await _pagamentoRepository.UnitOfWork.Commit())
            {
                validationResult.Errors.Add(new ValidationFailure("Pagamento",
                    $"Não foi possível persistir o cancelamento do pagamento do pedido {pedidoId}"));

                return new ResponseMessage(validationResult);
            }

            return new ResponseMessage(validationResult);
        }

        public async Task<ResponseMessage> CapturarPagamento(Guid pedidoId)
        {
            var payment = await _pagamentoRepository.FindPaymentPerOrderId(pedidoId);
            var transacaoAutorizada = payment.Transaction?.FirstOrDefault(t => t.Status == StatusTransaction.authorized);
            var validationResult = new ValidationResult();

            if (transacaoAutorizada == null) throw new DomainException($"Transação não encontrada para o pedido {pedidoId}");

            var transacao = await _pagamentoFacade.CapturingPayment(transacaoAutorizada.TID);

            if (transacao.Status != StatusTransaction.paid)
            {
                validationResult.Errors.Add(new ValidationFailure("Pagamento",
                    $"Não foi possível capturar o pagamento do pedido {pedidoId}"));

                return new ResponseMessage(validationResult);
            }

            transacao.PaymentId = transacaoAutorizada.PaymentId;
            _pagamentoRepository.InsertTransaction(transacao);

            if (!await _pagamentoRepository.UnitOfWork.Commit())
            {
                validationResult.Errors.Add(new ValidationFailure("Pagamento",
                    $"Não foi possível persistir a captura do pagamento do pedido {pedidoId}"));

                return new ResponseMessage(validationResult);
            }

            return new ResponseMessage(validationResult);
        }
    }
}
