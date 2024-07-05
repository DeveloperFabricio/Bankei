using Bankei.Application.Queries.ObterInvestimentos;
using Bankei.Domain.Entities;
using Bankei.Domain.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Bankei.UnitTests.ObterInvestimentos
{
    public class ObterInvestimentoQueryHandlerTests
    {
        private readonly Mock<IInvestimentoRepository> _investimentoRepositoryMock;
        private readonly ObterInvestimentoQueryHandler _handler;

        public ObterInvestimentoQueryHandlerTests()
        {
            _investimentoRepositoryMock = new Mock<IInvestimentoRepository>();
            _handler = new ObterInvestimentoQueryHandler(_investimentoRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ValidRequest_ReturnsInvestimentoDTO()
        {
            
            var dataInvestimento = DateTime.UtcNow.AddMonths(-3); 
            var query = new ObterInvestimentoQuery(1, dataInvestimento.AddDays(1)); 
            var investimento = new Investimento(1000, dataInvestimento);
            SetInvestimentoId(investimento, 1);
            _investimentoRepositoryMock.Setup(repo => repo.ObterPorId(It.IsAny<int>()))
                .ReturnsAsync(investimento);

            
            var result = await _handler.Handle(query, CancellationToken.None);

            
            Assert.NotNull(result);
            Assert.Equal(investimento.Id, result.Id);
            Assert.Equal(investimento.ValorInicial, result.ValorInicial);
            Assert.Equal(investimento.DataInvestimento, result.DataInvestimento);
            Assert.Equal(investimento.CalcularSaldoParaData(query.Data.Value), result.Saldo);
        }

        [Fact]
        public async Task Handle_InvestimentoNotFound_ThrowsException()
        {
            
            var query = new ObterInvestimentoQuery(1, DateTime.UtcNow);
            _investimentoRepositoryMock.Setup(repo => repo.ObterPorId(It.IsAny<int>()))
                .ReturnsAsync((Investimento)null);

            
            await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(query, CancellationToken.None));
        }

        private void SetInvestimentoId(Investimento investimento, int id)
        {
            var idProperty = typeof(Investimento).GetProperty("Id", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
            idProperty.SetValue(investimento, id);
        }
    }
}
