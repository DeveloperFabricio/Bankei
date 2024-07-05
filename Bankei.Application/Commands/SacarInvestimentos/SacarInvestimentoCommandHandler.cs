using Bankei.Domain.Entities;
using Bankei.Domain.Exceptions;
using Bankei.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bankei.Application.Commands.SacarInvestimentos
{
    public class SacarInvestimentoCommandHandler : IRequestHandler<SacarInvestimentoCommand, Unit>
    {
        private readonly IInvestimentoRepository _investimentoRepository;

        public SacarInvestimentoCommandHandler(IInvestimentoRepository investimentoRepository)
        {
            _investimentoRepository = investimentoRepository;
        }

        public async Task<Unit> Handle(SacarInvestimentoCommand request, CancellationToken cancellationToken)
        {
            var investimento = await _investimentoRepository.ObterPorId(request.InvestimentoId);
            if (investimento == null)
            {
                throw new NotFoundException("Investimento não encontrado.");
            }

            investimento.Sacar();

            await _investimentoRepository.Atualizar(investimento);

            var valorInicial = investimento.ValorInicial;
            var valorComJuros = CalcularJuros(investimento);
            var totalSacado = CalcularTotalSacado(investimento);

            return Unit.Value;
        }

        public decimal CalcularJuros(Investimento investimento)
        {
            var jurosMensal = 0.0116m;
            var meses = (DateTime.Now - investimento.DataInvestimento).Days / 30;
            var valorInicial = investimento.ValorInicial;

            var ganhosAcumulados = valorInicial * (decimal)Math.Pow(1 + (double)jurosMensal, meses) - valorInicial;

            return ganhosAcumulados;
        }

        public decimal CalcularTotalSacado(Investimento investimento)
        {
            var meses = (DateTime.Now - investimento.DataInvestimento).Days / 30;

            var jurosMensal = 0.0116m;
            var valorInicial = investimento.ValorInicial;
            var ganhosAcumulados = valorInicial * (decimal)Math.Pow(1 + (double)jurosMensal, meses) - valorInicial;

            var totalSacado = valorInicial + ganhosAcumulados;

            return totalSacado;
        }
    }


}
