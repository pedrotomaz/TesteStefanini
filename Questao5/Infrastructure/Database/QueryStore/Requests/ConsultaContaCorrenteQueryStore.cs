using Dapper;
using Microsoft.Data.Sqlite;
using Questao5.Domain.Entities;
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
            const string sql = @"SELECT 
                                    idcontacorrente AS Id, 
                                    numero AS Numero,    
                                    nome AS NomeTitular, 
                                    ativo AS Ativo
                                FROM contacorrente WHERE id = @Id 
                                LIMIT 1;";

            await using var connection = new SqliteConnection(_dataBaseConfig.Name);
            await connection.OpenAsync();

            ContaCorrente contaCorrente = await connection.QueryFirstOrDefaultAsync<ContaCorrente>(sql, new { Id = id });

            return new ContaCorrenteQueryStoreResponse(contaCorrente != null ? new ContaCorrenteResponse(contaCorrente.Id, contaCorrente.NomeTitular, contaCorrente.Numero, contaCorrente.Ativo) : null);
        }
    }
}
