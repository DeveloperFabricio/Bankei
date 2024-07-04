using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bankei.Application.DTO_s
{
    public class InvestimentoDTO
    {
        public int Id { get; set; }
        public decimal ValorInicial { get; set; }
        public DateTime DataInvestimento { get; set; }
        public decimal Saldo { get; set; }
    }
}
