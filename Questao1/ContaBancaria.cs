using System;
using System.Drawing;
using System.Globalization;

namespace Questao1
{
    class ContaBancaria {
        public int Numero { get; private set; }
        public string Titular { get; private set; }
        public double ValorDeposito { get; private set; }

        public double Saldo { get; private set; }

        public ContaBancaria()
        {
        }

        public ContaBancaria(int numero, string titular, double depositoInicial)
        {
            Numero = numero;
            Titular = titular;
            Saldo = Math.Abs(depositoInicial);
        }

        public ContaBancaria(int numero, string titular)
        {
            Numero = numero;
            Titular = titular;
            Saldo = 0;
        }

        #region Métodos Públicos
        public void Deposito(double valor)
        {
            Saldo += Math.Abs(valor);
        }

        public void Saque(double valor)
        {
            Saldo -= Math.Abs(valor);
            CobrarTaxa();
        }

        public string ExibirInformacoes() => $"Conta {Numero}, Titular {Titular}, Saldo: $ {Saldo.ToString("n2")}";
        
        #endregion

        #region Métodos Privados
        private void CobrarTaxa()
        {
            Saldo -= 3.5;
        }
        #endregion

    }
}
