using Microsoft.Data.Sqlite;
using Questao5.Domain.Interfaces;
using Questao5.Infrastructure.Sqlite;
using Dapper;

namespace Questao5.Infrastructure.Repositories
{
    public class IdempotencyRepository : IIdempotencyRepository
    {
        private readonly DatabaseConfig _dataBaseConfig;

        public IdempotencyRepository(DatabaseConfig dataBaseConfig)
        {
            _dataBaseConfig = dataBaseConfig;
        }

        public async Task<string?> GetExistingResultAsync(string idempotencyKey)
        {
            await using var connection = new SqliteConnection(_dataBaseConfig.Name);
            await connection.OpenAsync();

            return await connection.QueryFirstOrDefaultAsync<string>(
                "SELECT resultado FROM idempotencia WHERE chave_idempotencia = @Key",
                new { Key = idempotencyKey });
        }

        public async Task SaveRequestAsync(string idempotencyKey, string requestData, string responseData)
        {
            await using var connection = new SqliteConnection(_dataBaseConfig.Name);
            await connection.OpenAsync();
            
            await connection.QueryAsync(
                "INSERT INTO idempotencia (chave_idempotencia, requisicao, resultado) VALUES (@Key, @Request, @Response)",
                new { Key = idempotencyKey, Request = requestData, Response = responseData });
        }
    }

}
