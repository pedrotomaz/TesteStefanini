namespace Questao5.Application.Queries.Responses
{
    public record MovimentoQueryResponse(int numero, string titular, DateTime dataConsulta, decimal saldo);
}
