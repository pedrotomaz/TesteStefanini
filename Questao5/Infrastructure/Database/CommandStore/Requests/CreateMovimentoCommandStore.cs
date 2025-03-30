using Dapper;
using Microsoft.Data.Sqlite;
using Questao5.Application.Commands.Requests;
using Questao5.Domain.Entities;
using Questao5.Domain.Interfaces;
using Questao5.Infrastructure.Database.CommandStore.Responses;
using Questao5.Infrastructure.Sqlite;

namespace Questao5.Infrastructure.Database.CommandStore.Requests
{
    public class CreateMovimentoCommandStore : ICreateMovimentoCommandStore
    {
        private readonly DatabaseConfig _dataBaseConfig;

        public CreateMovimentoCommandStore(DatabaseConfig dataBaseConfig)
        {
            _dataBaseConfig = dataBaseConfig;
        }

        public async Task<CreateMovimentoCommandStoreResponse> CreateAsync(CreateMovimentoCommand request)
        {
            Movimento movimento = new Movimento(request.idContaCorrente, request.tipoMovimento, request.valor);

            const string sql = @"
                INSERT INTO movimento (idcontacorrente, datamovimento, tipomovimento, valor)
                VALUES (@IdContaCorrente, @DataMovimento, @TipoMovimento, @Valor);
        
                SELECT last_insert_rowid();"
            ;

            await using var connection = new SqliteConnection(_dataBaseConfig.Name);
            await connection.OpenAsync();

            string movimentacaoId = await connection.QuerySingleAsync<string>(sql, new {IdContaCorrente = movimento.IdContaCorrente, DataMovimento = movimento.DataMovimento.ToString("dd/MM/hhhh"), TipoMovimento = movimento.TipoMovimento, Valor = movimento.Valor});

            return new CreateMovimentoCommandStoreResponse(movimentacaoId);
        }
    }
}
