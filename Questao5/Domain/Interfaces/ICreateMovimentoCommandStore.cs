using Questao5.Application.Commands.Requests;
using Questao5.Infrastructure.Database.CommandStore.Responses;

namespace Questao5.Domain.Interfaces
{
    public interface ICreateMovimentoCommandStore
    {
        Task<CreateMovimentoCommandStoreResponse> CreateAsync(CreateMovimentoCommand request);
    }
}
