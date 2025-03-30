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
            ValidateRequest(request);

            var response = await _movimentoRepository.CreateAsync(request);

            return new CreateMovimentoResponse(response.id);
        }


        private async void ValidateRequest(CreateMovimentoCommand request)
        {
            if (string.IsNullOrWhiteSpace(request.idContaCorrente)) throw new Exception("Id da conta corrente é obrigatório");
            if (string.IsNullOrWhiteSpace(request.tipoMovimento) || (!string.Equals(request.tipoMovimento, "c", StringComparison.CurrentCultureIgnoreCase) && (!string.Equals(request.tipoMovimento, "d", StringComparison.CurrentCultureIgnoreCase)))) throw new Exception("Tipo do movimento é obrigatório");
            if (request.valor <= 0) throw new Exception("Valor deve ser positivo");

             var response = await _contaRepository.GetAsync(request.idContaCorrente);
            if (response.contaCorrente == null) throw new Exception("A Conta Corrente não foi encontrada");
        }

    }
}
