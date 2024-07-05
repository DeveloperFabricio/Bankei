using Bankei.Application.Commands.CriarInvestimentos;
using Bankei.Domain.Entities;
using Bankei.Domain.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bankei.UnitTests.CriarInvestimentos
{
    public class CriarInvestimentoCommandHandlerTests
    {
        private readonly Mock<IInvestimentoRepository> _investimentoRepositoryMock;
        private readonly CriarInvestimentoCommandHandler _handler;

        public CriarInvestimentoCommandHandlerTests()
        {
            _investimentoRepositoryMock = new Mock<IInvestimentoRepository>();
            _handler = new CriarInvestimentoCommandHandler(_investimentoRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ValidRequest_ReturnsInvestimentoId()
        {
           
            var command = new CriarInvestimentoCommand(1000, DateTime.Now);
            var investimento = new Investimento(1000, DateTime.Now);
            _investimentoRepositoryMock.Setup(repo => repo.Adicionar(It.IsAny<Investimento>()))
                .Returns(Task.CompletedTask)
                .Callback<Investimento>(inv => inv.GetType()
                                                  .GetProperty("Id")
                                                  .SetValue(inv, 1, null));

            
            var result = await _handler.Handle(command, CancellationToken.None);

            
            Assert.Equal(1, result);
            _investimentoRepositoryMock.Verify(repo => repo.Adicionar(It.IsAny<Investimento>()), Times.Once);
        }

        [Fact]
        public async Task Handle_InvalidRequest_ThrowsException()
        {
            
            var command = new CriarInvestimentoCommand(-1000, DateTime.Now);

            
            await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(command, CancellationToken.None));
        }
    }
}
