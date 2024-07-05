using Bankei.Application.Commands.SacarInvestimentos;
using Bankei.Domain.Entities;
using Bankei.Domain.Exceptions;
using Bankei.Domain.Repositories;
using MediatR;
using Moq;
using System.Reflection;

namespace Bankei.UnitTests.SacarInvestimentos
{
    public class SacarInvestimentoCommandHandlerTests
    {
        private readonly Mock<IInvestimentoRepository> _investimentoRepositoryMock;
        private readonly SacarInvestimentoCommandHandler _handler;

        public SacarInvestimentoCommandHandlerTests()
        {
            _investimentoRepositoryMock = new Mock<IInvestimentoRepository>();
            _handler = new SacarInvestimentoCommandHandler(_investimentoRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ValidRequest_SacaInvestimento()
        {
           
            var investimento = new Investimento(1000, DateTime.UtcNow.AddMonths(-6));
            SetInvestimentoId(investimento, 1);
            _investimentoRepositoryMock.Setup(repo => repo.ObterPorId(It.IsAny<int>()))
                .ReturnsAsync(investimento);

            var command = new SacarInvestimentoCommand(1);

           
            var result = await _handler.Handle(command, CancellationToken.None);

            
            _investimentoRepositoryMock.Verify(repo => repo.Atualizar(It.IsAny<Investimento>()), Times.Once);
            Assert.Equal(Unit.Value, result);
        }

        [Fact]
        public async Task Handle_InvestimentoNotFound_ThrowsNotFoundException()
        {
            
            _investimentoRepositoryMock.Setup(repo => repo.ObterPorId(It.IsAny<int>()))
                .ReturnsAsync((Investimento)null);

            var command = new SacarInvestimentoCommand(1);

            
            await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_UnexpectedError_ThrowsException()
        {
            
            var investimento = new Investimento(1000, DateTime.UtcNow.AddMonths(-6));
            SetInvestimentoId(investimento, 1);
            _investimentoRepositoryMock.Setup(repo => repo.ObterPorId(It.IsAny<int>()))
                .ReturnsAsync(investimento);

            _investimentoRepositoryMock.Setup(repo => repo.Atualizar(It.IsAny<Investimento>()))
                .ThrowsAsync(new Exception("Database update error"));

            var command = new SacarInvestimentoCommand(1);

           
            var exception = await Assert.ThrowsAsync<Exception>(() => _handler.Handle(command, CancellationToken.None));
            Assert.Equal("Database update error", exception.Message);
        }

        private void SetInvestimentoId(Investimento investimento, int id)
        {
            var idProperty = typeof(Investimento).GetProperty("Id", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
            idProperty.SetValue(investimento, id);
        }
    }
}
