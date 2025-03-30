using MediatR;
using Questao5.Application.Commands.Responses;

namespace Questao5.Application.Commands.Requests
{
    public record CreateMovimentoCommand(string idContaCorrente, string tipoMovimento, decimal valor) : IRequest<CreateMovimentoResponse>;
}
