using MediatR;
using Questao5.Application.Queries.Requests;
using Questao5.Application.Queries.Responses;
using Questao5.Domain.Interfaces;

namespace Questao5.Application.Handlers
{
    public class ConsultaContaCorrenteHanlder : IRequestHandler<ConsultaContaCorrenteQueryRequest, ConsultaContaCorrenteQueryResponse?>
    {
        private readonly IConsultaContaCorrenteQueryStore _contaCorrenteQueryStore;

        public ConsultaContaCorrenteHanlder(IConsultaContaCorrenteQueryStore contaCorrenteQueryStore)
        {
            _contaCorrenteQueryStore = contaCorrenteQueryStore;
        }

        public async Task<ConsultaContaCorrenteQueryResponse?> Handle(ConsultaContaCorrenteQueryRequest request, CancellationToken cancellationToken)
        {
            var response = await _contaCorrenteQueryStore.GetAsync(request.id);

            return response != null ? new ConsultaContaCorrenteQueryResponse(response.id, response.numero, response.nome, response.ativo) : null;
        }
    }
}
