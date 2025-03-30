using MediatR;
using Questao5.Application.Queries.Responses;

namespace Questao5.Application.Queries.Requests
{
    public record MovimentoQueryRequest(string contaCorrenteId) : IRequest<MovimentoQueryResponse>;
    
}
