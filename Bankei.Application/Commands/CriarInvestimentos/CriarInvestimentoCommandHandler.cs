using Bankei.Domain.Entities;
using Bankei.Domain.Repositories;
using MediatR;

namespace Bankei.Application.Commands.CriarInvestimentos
{
    public class CriarInvestimentoCommandHandler : IRequestHandler<CriarInvestimentoCommand, int>
    {
        private readonly IInvestimentoRepository _investimentoRepository;

        public CriarInvestimentoCommandHandler(IInvestimentoRepository investimentoRepository)
        {
            _investimentoRepository = investimentoRepository;
        }
        public async Task<int> Handle(CriarInvestimentoCommand request, CancellationToken cancellationToken)
        {
            if (request.ValorInicial <= 0)
            {
                throw new ArgumentException("O valor inicial do investimento deve ser maior que zero.");
            }

            var investimento = new Investimento(request.ValorInicial, request.DataInvestimento);

            await _investimentoRepository.Adicionar(investimento);

            return investimento.Id;
        }
    }
}
