using Dominio.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Entidades
{
    public class Transacao
    {
        public Transacao(decimal valor, MoedaType moedaDeOrigem, MoedaType moedaDeDestino)
        {
            if (moedaDeOrigem == moedaDeDestino)
                throw new ArgumentException("Erro! Moeda de entrada igual à moeda de saída");

            if (valor <= 0)
                throw new ArgumentException("Erro! Valor de entrada inválido!");

            Id = Guid.NewGuid();
            Valor = valor;
            MoedaDeOrigem = moedaDeOrigem;
            MoedaDeDestino = moedaDeDestino;
        }

        public Guid Id { get; private set; }
        public decimal Valor { get; private set; }
        public MoedaType MoedaDeOrigem { get; private set; }
        public MoedaType MoedaDeDestino { get; private set; }
        public decimal ValorSaida { get; private set; }

        public void Converter(decimal cotacaoDolar, decimal cotacaoMoedaSaida)
        {
            var valorSemTaxa = Valor - CalcularTaxa(Valor);

            var totalEmDolar = valorSemTaxa * cotacaoDolar;

            if (totalEmDolar < 10)
                throw new ArgumentException("Erro! A conversão mínima é de 10 dólares!");

            ValorSaida = valorSemTaxa * cotacaoMoedaSaida;
        }

        public static decimal CalcularTaxa(decimal valorTotal)
        {
            return valorTotal * 0.1M;
        }
    }
}
