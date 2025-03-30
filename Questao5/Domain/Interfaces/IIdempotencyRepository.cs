namespace Questao5.Domain.Interfaces
{
    public interface IIdempotencyRepository
    {
        Task<string?> GetExistingResultAsync(string idempotencyKey);
        Task SaveRequestAsync(string idempotencyKey, string requestData, string responseData);
    }
}
