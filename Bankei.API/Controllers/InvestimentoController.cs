using Bankei.Application.Commands.CriarInvestimentos;
using Bankei.Application.Commands.SacarInvestimentos;
using Bankei.Application.Queries.ObterInvestimentos;
using Bankei.Domain.Entities;
using Bankei.Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Bankei.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvestimentoController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IInvestimentoRepository _investimentoRepository;

        public InvestimentoController(IMediator mediator, IInvestimentoRepository investimentoRepository)
        {
            _mediator = mediator;
            _investimentoRepository = investimentoRepository;
        }

        [HttpPost("criar")]
        public async Task<IActionResult> CriarInvestimento([FromBody] CriarInvestimentoCommand command)
        {
            var investimentoId = await _mediator.Send(command);

            var responseBody = new
            {
                InvestimentoId = investimentoId,
                ValorInicial = command.ValorInicial,
                Mensagem = "Investimento criado com sucesso!"
            };

            return Ok(responseBody);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterInvestimento(int id, [FromQuery] DateTime? data)
        {
            var query = new ObterInvestimentoQuery(id, data);

            var investimento = await _mediator.Send(query);
            return Ok(investimento);
        }

        [HttpPost("{id}/sacar")]
        public async Task<IActionResult> SacarInvestimento(int id)
        { 
            var command = new SacarInvestimentoCommand(id);
            var result = await _mediator.Send(command);
                        
            var investimento = await _investimentoRepository.ObterPorId(command.InvestimentoId);
            if (investimento == null)
            {
                return NotFound("Investimento não encontrado.");
            }

            var responseBody = new
            {
                ValorInicial = investimento.ValorInicial,
                Juros = investimento.CalcularJuros(investimento), 
                TotalSacado = investimento.CalcularTotalSacado(investimento) 
            };

            return Ok(responseBody);
        }
    }
}
