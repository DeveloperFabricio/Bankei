using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bankei.Application.Commands.SacarInvestimentos
{
    public class SacarInvestimentoCommand : IRequest<Unit>
    {
        public int InvestimentoId { get; set; }

        public SacarInvestimentoCommand(int investimentoId)
        {
            InvestimentoId = investimentoId;
        }
    }
}
