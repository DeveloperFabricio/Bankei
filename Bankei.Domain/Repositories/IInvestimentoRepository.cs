using Bankei.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bankei.Domain.Repositories
{
    public interface IInvestimentoRepository
    {
        Task Adicionar(Investimento investimento);
        Task<Investimento> ObterPorId(int id);
        Task Atualizar(Investimento investimento);
    }
}
