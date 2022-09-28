using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PProjetoOnTheFly_Banco
{
    internal class Passageiros
    {
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public char Sexo { get; set; }
        public char Situacao { get; set; }
        public DateTime DataNascimento { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime UltimaCompra { get; set; }

        public Passageiros()
        {

        }

        public Passageiros(string Cpf, string Nome, DateTime DataNascimento, char Sexo, DateTime UltimaCompra, DateTime DataCadastro, char Situacao)
        {
            this.Cpf = Cpf;
            this.Nome = Nome;
            this.DataNascimento = DataNascimento;
            this.Sexo = Sexo;
            this.UltimaCompra = DateTime.Now;
            this.DataCadastro = DateTime.Now;
            this.Situacao = 'A';
        }

        public void CadastrarPessoa()
        {
            Conexao conexao = new Conexao();
            do
            {
                Nome = Utils.ColetarString("Informe seu nome: ");
                if (Nome.Length > 50) Console.WriteLine("Informe um nome que contenha menos de 50 caracteres");
                else break;
            } while (true);
            do Cpf = Utils.ColetarString("Informe seu CPF: ").Replace("-", "").Replace(".", "");
            while (!Utils.ValidarCpf(Cpf));
            do
            {
                DataNascimento = Utils.ColetarData("Informe sua data de nascimento: ");
                if (DataNascimento > DateTime.Now) Console.WriteLine("Informe uma data válida");
                else break;
            } while (true);
            do
            {
                UltimaCompra = Utils.ColetarData("Informe sua data de nascimento: ");
                if (DataNascimento > DateTime.Now) Console.WriteLine("Informe uma data válida");
                else break;
            } while (true);
            do
            {
                DataCadastro = Utils.ColetarData("Informe sua data de nascimento: ");
                if (DataNascimento > DateTime.Now) Console.WriteLine("Informe uma data válida");
                else break;
            } while (true);
            do
            {
                Sexo = Utils.ColetarValorChar("Informe o sexo informado no RG (M  - Masculino) ou (F - Feminino): ");
                if (Sexo != 'M' && Sexo != 'F') Console.WriteLine("Informe um valor válido...");
                else break;
            } while (true);

           
            Situacao = 'A';
            conexao.InsertTablePassageiros(this);
        }





        public override string ToString()
        {
            return ($"Nome: {this.Nome}\nCPF: {this.Cpf}\nSexo: {this.Sexo}\nDada de nascimento: {this.DataNascimento}\nÚltima compra: {this.UltimaCompra}\nData do cadastro: {this.DataCadastro}\nSituação de cadastro: {this.Situacao}");
        }



    }
}
