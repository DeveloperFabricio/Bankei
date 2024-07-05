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

            // Verificar se o saque está sendo feito na data atual
            if (request.DataSaque.Date != DateTime.UtcNow.Date)
            {
                throw new InvalidOperationException("O saque só pode ser realizado na data atual.");
            }

            // Verificar se o saque é parcial ou maior do que o valor disponível
            if (request.ValorSaque <= 0 || request.ValorSaque > investimento.ValorInicial)
            {
                throw new InvalidOperationException("O valor do saque é inválido.");
            }

            // Verificar se o investimento já foi sacado antes
            if (investimento.FoiSacado)
            {
                throw new InvalidOperationException("Este investimento já foi sacado anteriormente.");
            }

            var valorInicial = investimento.ValorInicial;
            var valorComJuros = CalcularJuros(investimento);
            var totalSacado = CalcularTotalSacado(investimento);

            if (request.ValorSaque <= 0 || request.ValorSaque != investimento.Saldo)
            {
                throw new InvalidOperationException("O valor do saque é inválido.");
            }

            investimento.AtualizarSaldo(request.ValorSaque);

            investimento.Sacar();
                       
            await _investimentoRepository.Atualizar(investimento);

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
