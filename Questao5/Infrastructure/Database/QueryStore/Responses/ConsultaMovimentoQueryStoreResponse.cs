using Questao5.Domain.Entities;

namespace Questao5.Infrastructure.Database.QueryStore.Responses
{
    public record ConsultaMovimentoQueryStoreResponse(ICollection<Movimento> movimentos);
}
