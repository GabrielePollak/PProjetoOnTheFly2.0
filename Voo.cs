using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PProjetoOnTheFly_Banco
{
    internal class Voo
    {
        public int ID_Voo { get; set; }
        public string Destino { get; set; }
        public DateTime Data_Cadastro {get; set; }
        public DateTime Data_Hora_Voo {get; set; }
        //assentos ocupados aqui e no banco!!!
        public string Situação { get; set; }

        
        public Voo ()
        {

        }
        
        
        public Voo(int iD_Voo, string destino, DateTime data_Cadastro, DateTime data_Hora_Voo, string situação)
        {
            ID_Voo = iD_Voo;
            Destino = destino;
            Data_Cadastro = data_Cadastro;
            Data_Hora_Voo = data_Hora_Voo;
            Situação = situação;
        }
    }
}
