using FluentAssertions;
using NSubstitute;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Commands.Responses;
using Questao5.Application.Handlers;
using Questao5.Domain.Entities;
using Questao5.Domain.Interfaces;
using Questao5.Infrastructure.Database.CommandStore.Responses;
using Questao5.Infrastructure.Database.QueryStore.Responses;
using Questao5.Infrastructure.Repositories;
using System.Text.Json;

namespace TesteMovimento.Tests
{
    public class CreateMovimentoHandlerTests
    {
        private readonly ICreateMovimentoCommandStore _movimentoRepository;
        private readonly IConsultaContaCorrenteQueryStore _contaRepository;
        private readonly IIdempotencyRepository _idempotencyRepository;
        private readonly CreateMovimentoHandler _handler;

        public CreateMovimentoHandlerTests()
        {
            _movimentoRepository = Substitute.For<ICreateMovimentoCommandStore>();
            _contaRepository = Substitute.For<IConsultaContaCorrenteQueryStore>();
            _handler = new CreateMovimentoHandler(_movimentoRepository, _contaRepository, _idempotencyRepository);
        }

        [Fact]
        public async Task Handle_ValidRequest_ReturnsCreateMovimentoResponse()
        {

            // Arrange
            var command = new CreateMovimentoCommand("B6BAFC09-6967-ED11-A567-055DFA4A16C9", "C", 100M);
            

            var contaCorrenteResponse = new ContaCorrenteQueryStoreResponse(new ContaCorrenteResponse(
                "B6BAFC09-6967-ED11-A567-055DFA4A16C9", "Titular", 123, true)
            );

            var expectedResponse = new CreateMovimentoResponse("987e4567-e89b-12d3-a456-426614174999");
            var createdMovimentoResponse = new CreateMovimentoCommandStoreResponse(Guid.NewGuid().ToString());

            

            _contaRepository.GetAsync(command.idContaCorrente)
                .Returns(Task.FromResult(contaCorrenteResponse));

            _movimentoRepository.CreateAsync(command)
                .Returns(Task.FromResult(createdMovimentoResponse));

            // Act
            var result = await _handler.Handle(command, default);

            // Assert
            result.Should().NotBeNull();
            result.id.Should().NotBeNullOrEmpty();
        }


        [Fact]
        public async Task Handle_InvalidAccount_ThrowsBusinessValidationException()
        {
            // Arrange
            var command = new CreateMovimentoCommand("id_invalido", "C", 100M);

            _contaRepository.GetAsync(command.idContaCorrente).Returns(Task.FromResult<ContaCorrenteQueryStoreResponse>(null));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<BusinessValidationException>(() => _handler.Handle(command, default));

            exception.Should().NotBeNull();
            exception.Message.Should().Be("Conta Corrente não encontrada");
            exception.ErrorCode.Should().Be("INVALID_ACCOUNT");
        }

        [Fact]
        public async Task Handle_InvalidValue_ThrowsBusinessValidationException()
        {
            // Arrange
            var command = new CreateMovimentoCommand("B6BAFC09-6967-ED11-A567-055DFA4A16C9", "C", -10M);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<BusinessValidationException>(() => _handler.Handle(command, default));

            exception.Should().NotBeNull();
            exception.Message.Should().Be("Valor deve ser positivo");
            exception.ErrorCode.Should().Be("INVALID_VALUE");
        }

        [Fact]
        public async Task Handle_InvalidType_ThrowsBusinessValidationException()
        {
            // Arrange
            var command = new CreateMovimentoCommand("B6BAFC09-6967-ED11-A567-055DFA4A16C9", "X", 100M);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<BusinessValidationException>(() => _handler.Handle(command, default));

            exception.Should().NotBeNull();
            exception.Message.Should().Be("Apenas os tipos 'débito' ou 'crédito' são aceitos");
            exception.ErrorCode.Should().Be("INVALID_TYPE");
        }

        //[Fact]
        //public async Task Handle_DuplicateRequest_ReturnsSameResponse()
        //{
        //    // Arrange
        //    var idempotencyKey = "123e4567-e89b-12d3-a456-426614174000";
        //    var command = new CreateMovimentoCommand(idempotencyKey, "B6BAFC09-6967-ED11-A567-055DFA4A16C9", "C", 100M);
        //    var expectedResponse = new CreateMovimentoResponse("987e4567-e89b-12d3-a456-426614174999");

        //    _idempotencyRepository.GetExistingResultAsync(idempotencyKey)
        //        .Returns(Task.FromResult(JsonSerializer.Serialize(expectedResponse)));

        //    // Act
        //    var result = await _handler.Handle(command, default);

        //    // Assert
        //    result.Should().NotBeNull();
        //    result.id.Should().Be(expectedResponse.id);
        //}


    }
}

