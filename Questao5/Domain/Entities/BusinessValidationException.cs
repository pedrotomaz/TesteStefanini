namespace Questao5.Domain.Entities
{
    public class BusinessValidationException : Exception
    {
        public string ErrorCode { get; }

        public BusinessValidationException(string errorCode, string message)
            : base(message)
        {
            ErrorCode = errorCode;
        }
    }

}
