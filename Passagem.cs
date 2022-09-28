using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PProjetoOnTheFly_Banco
{
    internal class Passagem
    {
        public int ID_Passagem { get; set; }
        public double Valor_Passagem { get; set; }
        public DateTime Data_UltimaOperacao { get; set; }
        public string Situacao { get; set; }

        public Passagem ()
        {

        }

        public Passagem(int id_Passagem, double valor_Passagem, DateTime data_UltimaOperacao, string situacao)
        {
            ID_Passagem = id_Passagem;
            Valor_Passagem = valor_Passagem;
            Data_UltimaOperacao = data_UltimaOperacao;
            Situacao = situacao;
        }   
    }
}
