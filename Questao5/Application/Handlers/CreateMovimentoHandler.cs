using MediatR;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Commands.Responses;
using Questao5.Domain.Entities;
using Questao5.Domain.Interfaces;
using System.Text.Json;

namespace Questao5.Application.Handlers
{
    public class CreateMovimentoHandler : IRequestHandler<CreateMovimentoCommand, CreateMovimentoResponse>
    {
        private readonly ICreateMovimentoCommandStore _movimentoRepository;
        private readonly IConsultaContaCorrenteQueryStore _contaRepository;
        private readonly IIdempotencyRepository _idempotencyRepository;

        public CreateMovimentoHandler(ICreateMovimentoCommandStore movimentoRepository, IConsultaContaCorrenteQueryStore contaRepository, IIdempotencyRepository idempotencyRepository)
        {
            _movimentoRepository = movimentoRepository;
            _contaRepository = contaRepository;
            _idempotencyRepository = idempotencyRepository;
        }

       

        public async Task<CreateMovimentoResponse> Handle(CreateMovimentoCommand request, CancellationToken cancellationToken)
        {
            var requestJson = JsonSerializer.Serialize(request);

            // 🟢 Verifica se a requisição já foi processada
            //var existingResponseJson = await _idempotencyRepository.GetExistingResultAsync(request.IdRequisicao);
            //if (existingResponseJson != null)
            //{
            //    return JsonSerializer.Deserialize<CreateMovimentoResponse>(existingResponseJson);
            //}

            await ValidateRequest(request);

            var response = await _movimentoRepository.CreateAsync(request);

            var responseJson = JsonSerializer.Serialize(new CreateMovimentoResponse(response.id));

            //await _idempotencyRepository.SaveRequestAsync(request.IdRequisicao, requestJson, responseJson);

            return new CreateMovimentoResponse(response.id);
        }


        private async Task ValidateRequest(CreateMovimentoCommand request)
        {            
            if (string.IsNullOrWhiteSpace(request.tipoMovimento) ||
                (!string.Equals(request.tipoMovimento, "c", StringComparison.CurrentCultureIgnoreCase) &&
                !string.Equals(request.tipoMovimento, "d", StringComparison.CurrentCultureIgnoreCase)))
            {
                throw new BusinessValidationException("INVALID_TYPE", "Apenas os tipos 'débito' ou 'crédito' são aceitos");
            }

            if (request.valor < 0) throw new BusinessValidationException("INVALID_VALUE", "Valor deve ser positivo");
            if (string.IsNullOrWhiteSpace(request.idContaCorrente)) throw new BusinessValidationException("INVALID_ACCOUNT", "A Conta Corrente não foi encontrada");

            var response = await _contaRepository.GetAsync(request.idContaCorrente);
            if (response?.contaCorrente == null) throw new BusinessValidationException("INVALID_ACCOUNT", "Conta Corrente não encontrada");
            if (!response.contaCorrente.ativo) throw new BusinessValidationException("INACTIVE_ACCOUNT", "Conta Corrente está inativa");
        }
    }
}
