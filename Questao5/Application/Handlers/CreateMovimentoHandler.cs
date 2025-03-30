using MediatR;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Commands.Responses;
using Questao5.Domain.Interfaces;

namespace Questao5.Application.Handlers
{
    public class CreateMovimentoHandler : IRequestHandler<CreateMovimentoCommand, CreateMovimentoResponse>
    {
        private readonly ICreateMovimentoCommandStore _movimentoRepository;
        private readonly IConsultaContaCorrenteQueryStore _contaRepository;

        public CreateMovimentoHandler(ICreateMovimentoCommandStore movimentoRepository, IConsultaContaCorrenteQueryStore contaCorrenteQueryStore)
        {
            _movimentoRepository = movimentoRepository;
            _contaRepository = contaCorrenteQueryStore;
        }


        public async Task<CreateMovimentoResponse> Handle(CreateMovimentoCommand request, CancellationToken cancellationToken)
        {

            var response = await _movimentoRepository.CreateAsync(request);

            return new CreateMovimentoResponse(response.id);
        }


        private void ValidateRequest(CreateMovimentoCommand request)
        {
            
        }

    }
}
