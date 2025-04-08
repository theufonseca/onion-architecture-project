using Application.Interfaces.Applications;
using Application.Interfaces.Repositorios;
using Dominio.Entidades;
using Dominio.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Applications
{
    public class CambioApplication : ICambioApplication
    {
        private readonly IMoedaRepositorio moedaRepositorio;

        public CambioApplication(IMoedaRepositorio moedaRepositorio)
        {
            this.moedaRepositorio = moedaRepositorio;
        }

        public decimal CalcularTaxa(decimal valor)
        {
            var valorTaxa = Transacao.CalcularTaxa(valor);
            return valorTaxa;
        }

        public async Task<Transacao> ConverterMoeda(MoedaType moedaOrigem, MoedaType moedaDestino, decimal valor)
        {
            var transacao = new Transacao(valor, moedaOrigem, moedaDestino);

            var cotacaoDolar = await moedaRepositorio.ObterCotacao(moedaOrigem, MoedaType.USD);
            var cotacaoDestino = await moedaRepositorio.ObterCotacao(moedaOrigem, moedaDestino);

            if (cotacaoDolar is null || cotacaoDestino is null)
                throw new ArgumentException("Falha ao obter as cotações das moedas!");

            transacao.Converter(cotacaoDolar.Value, cotacaoDestino.Value);

            return transacao;
        }
    }
}
