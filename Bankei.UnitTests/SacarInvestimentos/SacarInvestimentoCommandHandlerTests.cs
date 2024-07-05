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

            var valorSaque = 1000; 
            var dataSaque = DateTime.UtcNow;

            var command = new SacarInvestimentoCommand(1, valorSaque, dataSaque);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(Unit.Value, result);
            _investimentoRepositoryMock.Verify(repo => repo.Atualizar(investimento), Times.Once);
            Assert.True(investimento.FoiSacado);
            Assert.Equal(0, investimento.Saldo);
        }

        [Fact]
        public async Task Handle_InvestimentoNotFound_ThrowsNotFoundException()
        {

            var investimento = new Investimento(1000, DateTime.UtcNow.AddMonths(-6));
            SetInvestimentoId(investimento, 1);
            _investimentoRepositoryMock.Setup(repo => repo.ObterPorId(It.IsAny<int>()))
                .ReturnsAsync((Investimento)null);

            var valorSaque = 500; 
            var dataSaque = DateTime.UtcNow; 

            var command = new SacarInvestimentoCommand(1, valorSaque, dataSaque); 

            // Act e Assert
            var exception = await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
            Assert.Equal("Investimento não encontrado.", exception.Message);
        }

        [Fact]
        public async Task Handle_UnexpectedError_ThrowsException()
        {

            var investimento = new Investimento(1000, DateTime.UtcNow.AddMonths(-6));
            SetInvestimentoId(investimento, 1);
            _investimentoRepositoryMock.Setup(repo => repo.ObterPorId(It.IsAny<int>()))
                .ReturnsAsync(investimento);

            _investimentoRepositoryMock.Setup(repo => repo.Atualizar(It.IsAny<Investimento>()))
                .ThrowsAsync(new InvalidOperationException("Erro de atualização do banco de dados"));

            var valorSaque = 1000; 
            var dataSaque = DateTime.UtcNow;

            var command = new SacarInvestimentoCommand(1, valorSaque, dataSaque);

            // Act
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command, CancellationToken.None));

            // Assert
            Assert.Equal("Erro de atualização do banco de dados", exception.Message);
        }

        private void SetInvestimentoId(Investimento investimento, int id)
        {
            var idProperty = typeof(Investimento).GetProperty("Id", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
            idProperty.SetValue(investimento, id);
        }
    }
}
