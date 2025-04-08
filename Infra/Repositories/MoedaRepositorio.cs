using Application.Interfaces.Repositorios;
using Dominio.Enums;
using System.Net.Http;
using System.Text.Json;

namespace Infra.Repositories
{
    public class MoedaRepositorio : IMoedaRepositorio
    {
        private readonly IHttpClientFactory httpClientFactory;

        public MoedaRepositorio(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        public async Task<decimal?> ObterCotacao(MoedaType moedaDeOrigem, MoedaType moedaDeDestino)
        {
            var client = httpClientFactory.CreateClient("moedaclient");
            var result = await client.GetAsync($"json/last/{moedaDeOrigem}-{moedaDeDestino}");

            if (result.IsSuccessStatusCode)
            {
                var conteudo = await result.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(conteudo);

                if (doc.RootElement.TryGetProperty($"{moedaDeOrigem}{moedaDeDestino}", out var moedaInfo) &&
                    moedaInfo.TryGetProperty("high", out var highProperty))
                {
                    var cotacaoMoedaDestinoString = highProperty.GetString();
                    if (decimal.TryParse(cotacaoMoedaDestinoString, out var cotacaoMoedaDestino))
                        return cotacaoMoedaDestino;

                    return null;
                }
            }

            return null;
        }
    }
}
