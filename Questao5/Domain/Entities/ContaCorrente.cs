namespace Questao5.Domain.Entities
{
    public class ContaCorrente
    {
        public string Id { get; private set; }
        public int Numero { get; private set; }
        public string NomeTitular { get; private set; }
        public bool Ativo { get; private set; }

        public ContaCorrente() { }
    }
}
