using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PProjetoOnTheFly_Banco
{
    internal class Aeronave
    {
        public string Incricao { get; set; }
        public int Capacidade { get; set; }
        public string Ultima_VendaAero { get; set; }
        public DateTime Data_CadastroAero { get; set; }
        public string Situacao { get; set; }

       public Aeronave()
        {
           
        }
        
        public Aeronave(string incricao, int capacidade, string ultima_VendaAero, DateTime data_CadastroAero, string situacao)
        {
            Incricao = incricao;
            Capacidade = capacidade;
            Ultima_VendaAero = ultima_VendaAero;
            Data_CadastroAero = data_CadastroAero;
            Situacao = situacao;
        }
    }
}
