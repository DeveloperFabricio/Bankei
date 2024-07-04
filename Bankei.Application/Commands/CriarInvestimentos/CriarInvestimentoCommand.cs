using MediatR;

namespace Bankei.Application.Commands.CriarInvestimentos
{
    public class CriarInvestimentoCommand : IRequest<int>
    {
        public decimal ValorInicial { get; set; }
        public DateTime DataInvestimento { get; set; }

        public CriarInvestimentoCommand(decimal valorInicial, DateTime dataInvestimento)
        {
            ValorInicial = valorInicial;
            DataInvestimento = dataInvestimento;
        }
    }
}
