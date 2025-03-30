using Dapper;
using Microsoft.Data.Sqlite;
using Questao5.Domain.Interfaces;
using Questao5.Infrastructure.Database.QueryStore.Responses;
using Questao5.Infrastructure.Sqlite;

namespace Questao5.Infrastructure.Database.QueryStore.Requests
{
    public class ConsultaContaCorrenteQueryStore : IConsultaContaCorrenteQueryStore
    {
        private readonly DatabaseConfig _dataBaseConfig;

        public ConsultaContaCorrenteQueryStore(DatabaseConfig dataBaseConfig)
        {
            _dataBaseConfig = dataBaseConfig;
        }

        public async Task<ContaCorrenteQueryStoreResponse?> GetAsync(string id)
        {
            const string sql = @"SELECT * FROM movimento WITH(NOLOCK) WHERE id = @Id;";

            await using var connection = new SqliteConnection(_dataBaseConfig.Name);
            await connection.OpenAsync();

            return await connection.QueryFirstOrDefaultAsync<ContaCorrenteQueryStoreResponse?>(sql, new { Id = id });
        }
    }
}
