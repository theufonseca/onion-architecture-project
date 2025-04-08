using Dominio.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositorios
{
    public interface IMoedaRepositorio
    {
        Task<decimal?> ObterCotacao(MoedaType moedaDeOrigem, MoedaType moedaDeDestino);
    }
}
