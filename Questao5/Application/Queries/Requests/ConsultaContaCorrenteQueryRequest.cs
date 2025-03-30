using MediatR;
using Questao5.Application.Queries.Responses;

namespace Questao5.Application.Queries.Requests
{
    public record ConsultaContaCorrenteQueryRequest(string id) : IRequest<ConsultaContaCorrenteQueryResponse>;
}
