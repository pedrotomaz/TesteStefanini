using Questao5.Domain.Entities;
using Questao5.Infrastructure.Database.QueryStore.Responses;

namespace Questao5.Application.Queries.Responses
{
    public record ConsultaContaCorrenteQueryResponse(ContaCorrenteResponse? contaCorrente); 
}
