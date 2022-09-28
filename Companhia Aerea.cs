using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PProjetoOnTheFly_Banco
{
    internal class Companhia_Aerea
    {
        public string CNPJ { get; set; }
        public string Razao_Social { get; set; }
        public DateTime Data_CadastroComp { get; set; }
        public DateTime Data_AberturaComp { get; set; }
        public string Ultimo_Voo { get; set; }
        public string Situacao { get; set; }

        public Companhia_Aerea()
        {

        }
        public Companhia_Aerea(string cNPJ, string razao_Social, DateTime data_CadastroComp, DateTime data_AberturaComp, string ultimo_Voo, string situacao)
        {
            CNPJ = cNPJ;
            Razao_Social = razao_Social;
            Data_CadastroComp = data_CadastroComp;
            Data_AberturaComp = data_AberturaComp;
            Ultimo_Voo = ultimo_Voo;
            Situacao = situacao;
        }
    }
}
