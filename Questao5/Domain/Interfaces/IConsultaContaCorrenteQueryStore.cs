using Questao5.Infrastructure.Database.QueryStore.Responses;

namespace Questao5.Domain.Interfaces
{
    public interface IConsultaContaCorrenteQueryStore
    {
        Task<ContaCorrenteQueryStoreResponse> GetAsync(string id);
    }
}
