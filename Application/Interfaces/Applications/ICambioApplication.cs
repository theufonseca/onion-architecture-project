using Dominio.Entidades;
using Dominio.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Applications
{
    public interface ICambioApplication
    {
        Task<Transacao> ConverterMoeda(MoedaType moedaOrigem, MoedaType moedaDestino, decimal valor);
        decimal CalcularTaxa(decimal valor);
    }
}
