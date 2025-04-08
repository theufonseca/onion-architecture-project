using Application.Applications;
using Application.Interfaces.Repositorios;
using Dominio.Enums;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Testes.Application
{
    public class CambioApplicationTest
    {
        private readonly Mock<IMoedaRepositorio> _moedaRepositorioMock;
        private readonly CambioApplication _cambioApplication;

        public CambioApplicationTest()
        {
            _moedaRepositorioMock = new Mock<IMoedaRepositorio>();
            _cambioApplication = new CambioApplication(_moedaRepositorioMock.Object);
        }

        [Fact]
        public void CalcularTaxa_DeveRetornarTaxaCorreta()
        {
            // Arrange
            decimal valor = 100;

            // Act
            decimal taxa = _cambioApplication.CalcularTaxa(valor);

            // Assert
            taxa.Should().Be(10); // 10% de 100
        }

        [Fact]
        public async Task ConverterMoeda_DeveRetornarTransacao_ComValoresCorretos()
        {
            // Arrange
            decimal valor = 100;
            MoedaType moedaOrigem = MoedaType.USD;
            MoedaType moedaDestino = MoedaType.BRL;
            decimal cotacaoDolar = 1; // 1:1 para facilitar
            decimal cotacaoDestino = 5; // 1 USD = 5 BRL

            _moedaRepositorioMock.Setup(m => m.ObterCotacao(moedaOrigem, MoedaType.USD)).ReturnsAsync(cotacaoDolar);
            _moedaRepositorioMock.Setup(m => m.ObterCotacao(moedaOrigem, moedaDestino)).ReturnsAsync(cotacaoDestino);

            // Act
            var transacao = await _cambioApplication.ConverterMoeda(moedaOrigem, moedaDestino, valor);

            // Assert
            transacao.ValorSaida.Should().Be(90 * 5); // 100 - 10% taxa = 90 * 5 BRL
        }

        [Fact]
        public async Task ConverterMoeda_DeveLancarExcecao_SeCotacaoForNula()
        {
            // Arrange
            MoedaType moedaOrigem = MoedaType.USD;
            MoedaType moedaDestino = MoedaType.BRL;
            decimal valor = 100;

            _moedaRepositorioMock.Setup(m => m.ObterCotacao(moedaOrigem, MoedaType.USD)).ReturnsAsync((decimal?)null);
            _moedaRepositorioMock.Setup(m => m.ObterCotacao(moedaOrigem, moedaDestino)).ReturnsAsync((decimal?)null);

            // Act
            Func<Task> act = async () => await _cambioApplication.ConverterMoeda(moedaOrigem, moedaDestino, valor);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>().WithMessage("Falha ao obter as cotações das moedas!");
        }
    }
}
