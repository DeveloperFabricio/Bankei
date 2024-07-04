namespace Bankei.Domain.Entities
{
    public class Investimento
    {
        public int Id { get; private set; }
        public decimal ValorInicial { get; private set; }
        public DateTime DataInvestimento { get; private set; }
        public bool FoiSacado { get; private set; }
        public decimal Saldo { get; private set; }

        private const decimal JurosMensais = 0.0116m;

        public Investimento(decimal valorInicial, DateTime dataInvestimento)
        {
            if (valorInicial <= 0)
                throw new ArgumentException("Valor do investimento deve ser positivo.");

            if (dataInvestimento > DateTime.UtcNow)
                throw new ArgumentException("A data do investimento não pode ser no futuro.");

            ValorInicial = valorInicial;
            DataInvestimento = dataInvestimento;
            Saldo = valorInicial;
            FoiSacado = false;
        }

        public decimal CalcularSaldoParaData(DateTime data)
        {
            if (data < DataInvestimento)
                throw new ArgumentException("A data não pode ser inferior à data do investimento.");

            int meses = ((data.Year - DataInvestimento.Year) * 12) + data.Month - DataInvestimento.Month;

            decimal saldo = ValorInicial;
            for (int i = 0; i < meses; i++)
            {
                saldo += saldo * JurosMensais;
            }

            return saldo;
        }

        public void Sacar()
        {
            if (FoiSacado)
                throw new InvalidOperationException("O investimento já foi sacado.");

            FoiSacado = true;
            Saldo = 0; 
        }

        public decimal CalcularJuros(Investimento investimento)
        {
            var jurosMensal = 0.0116m;
            var meses = (DateTime.Now - investimento.DataInvestimento).Days / 30;
            var valorInicial = investimento.ValorInicial;


            var ganhosAcumulados = valorInicial * (decimal)Math.Pow(1 + (double)jurosMensal, meses) - valorInicial;

            return ganhosAcumulados;
        }

        public decimal CalcularTotalSacado(Investimento investimento)
        {
            var meses = (DateTime.Now - investimento.DataInvestimento).Days / 30;

            var jurosMensal = 0.0116m;
            var valorInicial = investimento.ValorInicial;
            var ganhosAcumulados = valorInicial * (decimal)Math.Pow(1 + (double)jurosMensal, meses) - valorInicial;

            var totalSacado = valorInicial + ganhosAcumulados;

            return totalSacado;
        }
    }
}
