using Dapper;
using Microsoft.Data.Sqlite;
using Questao5.Domain.Entities;
using Questao5.Domain.Interfaces;
using Questao5.Infrastructure.Database.QueryStore.Responses;
using Questao5.Infrastructure.Sqlite;

namespace Questao5.Infrastructure.Database.QueryStore.Requests
{
    public class ConsultaMovimentoQueryStore : IConsultaMovimentoQueryStore
    {
        private readonly DatabaseConfig _dataBaseConfig;

        public ConsultaMovimentoQueryStore(DatabaseConfig dataBaseConfig)
        {
            _dataBaseConfig = dataBaseConfig;
        }

        public async Task<ConsultaMovimentoQueryStoreResponse> GetAllByCurrentAccount(string contaCorrenteId)
        {
            string sql = @"
                    SELECT 
                            m.idmovimento AS Id, 
                            m.idcontacorrente AS IdContaCorrente, 
                            m.valor AS Valor, 
                            m.tipomovimento AS TipoMovimento,
                            CAST(STRFTIME('%Y-%m-%d', '20' || substr(m.datamovimento,7,4) || '-' || substr(m.datamovimento,4,2) || '-' || substr(m.datamovimento,1,2)) AS TEXT) AS DataMovimento,
                            cc.idcontacorrente AS Id, 
                            cc.numero AS Numero,    
                            cc.nome AS NomeTitular, 
                            cc.ativo AS Ativo
                    FROM movimento m 
                        INNER JOIN contacorrente cc 
                            ON m.idcontacorrente = cc.idcontacorrente
                    WHERE m.idcontacorrente = @ContaCorrenteId";

            await using var connection = new SqliteConnection(_dataBaseConfig.Name);

            await connection.OpenAsync();

            var movimentos = await connection.QueryAsync<Movimento, ContaCorrente, Movimento>(
                sql,
                (movimento, contaCorrente) =>
                {
                    movimento.ContaCorrente = contaCorrente;
                    return movimento;
                },
                new { ContaCorrenteId = contaCorrenteId },
                splitOn: "Id"
            );

            return new ConsultaMovimentoQueryStoreResponse(movimentos?.ToList() ?? new List<Movimento>());
        }

    }
}
