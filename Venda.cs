using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PProjetoOnTheFly_Banco
{
    internal class Venda
    {
        public int ID_Venda { get; set; }
        public DateTime Data_Venda { get; set; }
        //Total da venda

        public Venda(int id_venda, DateTime data_venda)
        {
            Data_Venda = data_venda;
            ID_Venda = id_venda;

        }
    }
}
