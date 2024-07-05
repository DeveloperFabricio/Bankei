using Bankei.Application.DTO_s;
using Bankei.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bankei.Application.Queries.ObterInvestimentos
{
    public class ObterInvestimentoQueryHandler : IRequestHandler<ObterInvestimentoQuery, InvestimentoDTO>
    {
        private readonly IInvestimentoRepository _investimentoRepository;

        public ObterInvestimentoQueryHandler(IInvestimentoRepository investimentoRepository)
        {
            _investimentoRepository = investimentoRepository;
        }

        public async Task<InvestimentoDTO> Handle(ObterInvestimentoQuery request, CancellationToken cancellationToken)
        {
            var investimento = await _investimentoRepository.ObterPorId(request.InvestimentoId);
            if (investimento == null)
            {
                throw new InvalidOperationException("Investimento não encontrado.");
            }

            var dataConsulta = request.Data ?? DateTime.UtcNow;
            var saldo = investimento.CalcularSaldoParaData(dataConsulta);

            var investimentoDTO = new InvestimentoDTO
            {
                Id = investimento.Id,
                ValorInicial = investimento.ValorInicial,
                DataInvestimento = investimento.DataInvestimento,
                Saldo = saldo
            };

            return investimentoDTO;
        }
    }

}
