using MediatR;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Queries.Requests;
using Questao5.Application.Queries.Responses;
using Questao5.Domain.Entities;
using Questao5.Domain.Interfaces;

namespace Questao5.Application.Handlers
{
    public class ConsultaMovimentoHandler : IRequestHandler<MovimentoQueryRequest, MovimentoQueryResponse>
    {
        private readonly IConsultaMovimentoQueryStore _movimentoRepository;
        private readonly IConsultaContaCorrenteQueryStore _contaRepository;

        public ConsultaMovimentoHandler(IConsultaMovimentoQueryStore movimentoRepository, IConsultaContaCorrenteQueryStore contaRepository)
        {
            _movimentoRepository = movimentoRepository;
            _contaRepository = contaRepository;
        }

        public async Task<MovimentoQueryResponse> Handle(MovimentoQueryRequest request, CancellationToken cancellationToken)
        {
            await ValidateRequest(request);

            var response = await _movimentoRepository.GetAllByCurrentAccount(request.contaCorrenteId);
            
            decimal saldo = CalculateBalance(response);

            var movimento = response.movimentos.FirstOrDefault();

            return new MovimentoQueryResponse(movimento?.ContaCorrente?.Numero ?? 0, movimento?.ContaCorrente?.NomeTitular ?? "", DateTime.Now, saldo);
        }

        private decimal CalculateBalance(Infrastructure.Database.QueryStore.Responses.ConsultaMovimentoQueryStoreResponse response)
        {
            return response.movimentos.Where(x => string.Equals(x.TipoMovimento, "C", StringComparison.CurrentCultureIgnoreCase)).Sum(x => x.Valor) - response.movimentos.Where(x => string.Equals(x.TipoMovimento, "D", StringComparison.CurrentCultureIgnoreCase)).Sum(x => x.Valor);
        }

        private async Task ValidateRequest(MovimentoQueryRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.contaCorrenteId)) throw new BusinessValidationException("INVALID_ACCOUNT", "O Id da conta corrente não pode ser nulo ou vazio");

            var response = await _contaRepository.GetAsync(request.contaCorrenteId);
            if (response?.contaCorrente == null) throw new BusinessValidationException("INVALID_ACCOUNT", "Conta Corrente não encontrada");
            if (!response.contaCorrente.ativo) throw new BusinessValidationException("INACTIVE_ACCOUNT", "Conta Corrente está inativa");
        }
    }
}
