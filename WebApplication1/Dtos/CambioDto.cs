using Dominio.Enums;

namespace WebApplication1.Dtos
{
    public record CambioDto(MoedaType MoedaOrigem, MoedaType MoedaDestino, decimal Valor);
}
