using Bankei.Application.Commands.CriarInvestimentos;
using Bankei.Application.Commands.SacarInvestimentos;
using Bankei.Application.Queries.ObterInvestimentos;
using Bankei.Domain.Entities;
using Bankei.Domain.Exceptions;
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
            try
            {
                
                if (command.ValorInicial <= 0)
                {
                    throw new ArgumentException("O valor inicial do investimento deve ser maior que zero.");
                }

                var investimentoId = await _mediator.Send(command);

                var responseBody = new
                {
                    InvestimentoId = investimentoId,
                    ValorInicial = command.ValorInicial,
                    Mensagem = "Investimento criado com sucesso!"
                };

                return Ok(responseBody);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                
                return StatusCode(500, "Ocorreu um erro inesperado. Por favor, tente novamente mais tarde.");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterInvestimento(int id, [FromQuery] DateTime? data)
        {
            try
            {
                var query = new ObterInvestimentoQuery(id, data);

                var investimento = await _mediator.Send(query);

                if (investimento == null)
                {
                    throw new InvalidOperationException("Investimento não encontrado.");
                }

                return Ok(investimento);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message); 
            }
            catch (Exception ex)
            {
                
                return StatusCode(500, "Ocorreu um erro inesperado. Por favor, tente novamente mais tarde.");
            }
        }

        [HttpPost("{id}/sacar")]
        public async Task<IActionResult> SacarInvestimento(int id, decimal valorSaque)
        {
            try
            {
                var command = new SacarInvestimentoCommand(id, valorSaque, DateTime.UtcNow);
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
                    TotalSacado = investimento.CalcularTotalSacado(investimento),
                    Mensagem = "Saque realizado com sucesso!"
                };

                return Ok(responseBody);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                
                return StatusCode(500, "Ocorreu um erro inesperado. Por favor, tente novamente mais tarde.");
            }
        }
    }
}
