using Bankei.Application.DTO_s;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bankei.Application.Queries.ObterInvestimentos
{
    public class ObterInvestimentoQuery : IRequest<InvestimentoDTO>
    {
        public int InvestimentoId { get; set; }
        public DateTime? Data { get; set; }

        public ObterInvestimentoQuery(int investimentoId, DateTime? data = null)
        {
            InvestimentoId = investimentoId;
            Data = data;
        }
    }
}
