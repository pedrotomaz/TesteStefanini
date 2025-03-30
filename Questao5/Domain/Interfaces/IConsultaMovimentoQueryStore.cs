using Questao5.Infrastructure.Database.QueryStore.Responses;

namespace Questao5.Domain.Interfaces
{
    public interface IConsultaMovimentoQueryStore
    {
        Task<ConsultaMovimentoQueryStoreResponse> GetAllByCurrentAccount(string contaCorrenteId);
    }
}
