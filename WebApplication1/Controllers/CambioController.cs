using Application.Interfaces.Applications;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Dtos;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CambioController : ControllerBase
    {
        private readonly ICambioApplication cambioApplication;

        public CambioController(ICambioApplication cambioApplication)
        {
            this.cambioApplication = cambioApplication;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CambioDto cambio)
        {
            var resultado = await cambioApplication.ConverterMoeda(cambio.MoedaOrigem, cambio.MoedaDestino, cambio.Valor);
            return Ok(resultado);
        }
    }
}
