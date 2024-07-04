using Bankei.Domain.Entities;
using Bankei.Domain.Repositories;
using Bankei.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Bankei.Infrastructure.Repositories
{
    public class InvestimentoRepository : IInvestimentoRepository
    {
        private readonly AppDbContext _context;

        public InvestimentoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task Adicionar(Investimento investimento)
        {
            _context.Investimentos.AddAsync(investimento);
            await _context.SaveChangesAsync();
        }

        public async Task Atualizar(Investimento investimento)
        {
            _context.Investimentos.Update(investimento);
            await _context.SaveChangesAsync();
        }

        public async Task<Investimento> ObterPorId(int id)
        {
           return await _context.Investimentos.SingleOrDefaultAsync(i => i.Id == id);
        }
    }
}
