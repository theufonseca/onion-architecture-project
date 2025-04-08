using Dominio.Entidades;
using Dominio.Enums;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Testes.Dominio
{
    public class TransacaoTest
    {
        [Fact]
        public void Transacao_DeveSerCriada_ComValoresValidos()
        {
            // Arrange
            decimal valor = 100;
            MoedaType moedaOrigem = MoedaType.USD;
            MoedaType moedaDestino = MoedaType.BRL;

            // Act
            var transacao = new Transacao(valor, moedaOrigem, moedaDestino);

            // Assert
            transacao.Valor.Should().Be(valor);
            transacao.MoedaDeOrigem.Should().Be(moedaOrigem);
            transacao.MoedaDeDestino.Should().Be(moedaDestino);
            transacao.Id.Should().NotBeEmpty();
        }

        [Fact]
        public void Transacao_DeveLancarExcecao_SeMoedasForemIguais()
        {
            // Arrange
            decimal valor = 100;
            MoedaType moeda = MoedaType.USD;

            // Act
            Action act = () => new Transacao(valor, moeda, moeda);

            // Assert
            act.Should().Throw<ArgumentException>().WithMessage("Erro! Moeda de entrada igual à moeda de saída");
        }

        [Fact]
        public void Transacao_DeveLancarExcecao_SeValorForMenorOuIgualAZero()
        {
            // Arrange
            decimal valorInvalido = 0;
            MoedaType moedaOrigem = MoedaType.USD;
            MoedaType moedaDestino = MoedaType.BRL;

            // Act
            Action act = () => new Transacao(valorInvalido, moedaOrigem, moedaDestino);

            // Assert
            act.Should().Throw<ArgumentException>().WithMessage("Erro! Valor de entrada inválido!");
        }

        [Fact]
        public void Converter_DeveCalcularValorSaida_Corretamente()
        {
            // Arrange
            var transacao = new Transacao(100, MoedaType.USD, MoedaType.BRL);
            decimal cotacaoDolar = 1; // 1:1 para facilitar
            decimal cotacaoMoedaSaida = 5; // 1 USD = 5 BRL

            // Act
            transacao.Converter(cotacaoDolar, cotacaoMoedaSaida);

            // Assert
            transacao.ValorSaida.Should().Be(90 * 5); // 100 - 10% taxa = 90 * 5 BRL
        }

        [Fact]
        public void Converter_DeveLancarExcecao_SeConversaoMenorQue10Dolares()
        {
            // Arrange
            var transacao = new Transacao(10, MoedaType.USD, MoedaType.BRL);
            decimal cotacaoDolar = 1;
            decimal cotacaoMoedaSaida = 5;

            // Act
            Action act = () => transacao.Converter(cotacaoDolar, cotacaoMoedaSaida);

            // Assert
            act.Should().Throw<ArgumentException>().WithMessage("Erro! A conversão mínima é de 10 dólares!");
        }
    }
}
