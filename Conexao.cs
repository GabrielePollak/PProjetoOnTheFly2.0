using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PProjetoOnTheFly_Banco
{
    internal class Conexao
    {
        internal class Banco_De_Dados
        {
            string Conexao = "Data Source=localhost; Initial Catalog=OnTheFly; User ID=sa; password=scoobypolly;";

            public string Caminho()
            {
                return Conexao;
            }
        }

    }
}