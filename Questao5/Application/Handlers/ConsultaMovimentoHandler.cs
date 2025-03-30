using MediatR;
using Questao5.Application.Queries.Requests;
using Questao5.Application.Queries.Responses;
using Questao5.Domain.Interfaces;

namespace Questao5.Application.Handlers
{
    public class ConsultaMovimentoHandler : IRequestHandler<MovimentoQueryRequest, MovimentoQueryResponse>
    {
        private readonly IConsultaMovimentoQueryStore _movimentoRepository;

        public ConsultaMovimentoHandler(IConsultaMovimentoQueryStore movimentoRepository)
        {
            _movimentoRepository = movimentoRepository;
        }

        public async Task<MovimentoQueryResponse> Handle(MovimentoQueryRequest request, CancellationToken cancellationToken)
        {
            var response = await _movimentoRepository.GetAllByCurrentAccount(request.contaCorrenteId);
            
            decimal saldo = CalculateBalance(response);

            var movimento = response.movimentos.FirstOrDefault();

            return new MovimentoQueryResponse(movimento?.ContaCorrente?.Numero ?? 0, movimento?.ContaCorrente?.NomeTitular ?? "", DateTime.Now, saldo);
        }

        private decimal CalculateBalance(Infrastructure.Database.QueryStore.Responses.ConsultaMovimentoQueryStoreResponse response)
        {
            return response.movimentos.Where(x => string.Equals(x.TipoMovimento, "C", StringComparison.CurrentCultureIgnoreCase)).Sum(x => x.Valor) - response.movimentos.Where(x => string.Equals(x.TipoMovimento, "D", StringComparison.CurrentCultureIgnoreCase)).Sum(x => x.Valor);
        }
    }
}
