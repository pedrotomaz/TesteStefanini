using Questao5.Domain.Entities;

namespace Questao5.Infrastructure.Database.QueryStore.Responses
{
    public record ContaCorrenteQueryStoreResponse(ContaCorrenteResponse? contaCorrente);

    public record ContaCorrenteResponse(string id, string nomeTitular, int numero, bool ativo);
}
