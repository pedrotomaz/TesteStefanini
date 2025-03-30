using Questao5.Domain.Enumerators;

namespace Questao5.Domain.Entities
{
    public class Movimento
    {
        #region PROPERTIES
        public string Id { get; private set; }
        public string IdContaCorrente { get; private set; }
        public DateTime DataMovimento { get; private set; }
        public string TipoMovimento { get; private set; }
        public decimal Valor { get; private set; } 
        #endregion


        #region CONSTRUCTORS
        public Movimento() { }

        public Movimento(string idContaCorrente, string tipoMovimento, decimal valor)
        {
            Validate(valor, tipoMovimento, idContaCorrente);

            Id = Guid.NewGuid().ToString().ToUpper();
            DataMovimento = DateTime.Now;

            IdContaCorrente = idContaCorrente;
            TipoMovimento = tipoMovimento.Trim().ToUpper();
            Valor = valor;

        }
        #endregion


        #region PRIVATE METHODS
        private void Validate(decimal valor, string tipoMovimento, string idContaCorrente)
        {
            if (valor < 0) throw new ArgumentOutOfRangeException("Valor", "O valor do movimento não pode ser negativo.");
            if (string.IsNullOrWhiteSpace(tipoMovimento) || (tipoMovimento != "C" && tipoMovimento != "D")) throw new ArgumentException("TipoMovimento", "Tipo de movimento inválido.");
            if (string.IsNullOrWhiteSpace(idContaCorrente)) throw new ArgumentNullException("IdContaCorrente", "Id da conta corrente não pode ser nulo ou vazio.");
        }
        #endregion
    }
}
