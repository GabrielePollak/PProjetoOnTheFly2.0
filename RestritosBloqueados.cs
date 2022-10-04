using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PProjetoOnTheFly_Banco
{
    internal class RestritosBloqueados
    {
        public static void CadastrarCPFRestrito()
        {
            SqlConnection conexao = new(BancoOTF.CaminhoConexao());
            string cpf, sql;

            Console.Clear();

            Console.WriteLine("                                                      <<<<CADASTRO DE RESTRITOS>>>>                            \n");

            Console.Write("Informe o CPF que deseja restringir: ");
            cpf = Console.ReadLine().Replace(".", string.Empty).Replace("-", string.Empty).Replace(",", string.Empty).Replace("/", string.Empty);

            if (!Passageiros.ReadCPF(cpf))
            {
                Console.Write("\nEste CPF é Inválido!");
                return;
            }

            sql = $"SELECT * FROM RestritosCPF WHERE CPF = '{cpf}'";

            if (BancoOTF.LocalizarDados(sql, conexao))
            {
                Console.Write("\nO CPF informado já consta como restrito!");
                return;
            }

            sql = $"INSERT RestritosCPF VALUES('{cpf}')";

            if (BancoOTF.InsertDados(sql, conexao))
            {
                Console.Write("\nCadastrado com sucesso!");
                return;
            }

            Console.Write("\nErro ao realizar cadastro!");
        }

        public static void ExcluirCPFRestrito()
        {
            SqlConnection conexao = new(BancoOTF.CaminhoConexao());
            string cpf, sql;

            Console.Clear();

            Console.WriteLine("                            <<<<EXCLUSAO DE RESTRITOS>>>>               \n");

            Console.Write("Informe o CPF Restrito: ");
            cpf = Console.ReadLine().Replace(".", string.Empty).Replace("-", string.Empty).Replace(",", string.Empty).Replace("/", string.Empty);

            sql = $"SELECT * FROM RestritosCPF WHERE CPF = '{cpf}'";

            if (!BancoOTF.LocalizarDados(sql, conexao))
            {
                Console.Write("\nCPF informado não consta como restrito!");
                return;
            }

            sql = $"DELETE FROM RestritosCPF WHERE CPF = '{cpf}'";

            if (BancoOTF.DeleteDados(sql, conexao))
            {
                Console.Write("\nExcluido com sucesso!");
                return;
            }

            Console.Write("\nFalha na exclusão!");
            return;

        }
        public static void CadastrarCNPJBloqueado()
        {
            SqlConnection conexao = new(BancoOTF.CaminhoConexao());
            string cnpj, sql;

            Console.Clear();

            Console.WriteLine("                                                    <<<<CADASTRO DE BLOQUEADOS>>>>                        \n");

            Console.Write("Informe o CNPJ que deseja bloquear: ");
            cnpj = Console.ReadLine().Replace(".", string.Empty).Replace("-", string.Empty).Replace(",", string.Empty).Replace("/", string.Empty);

            if (!Companhia_Aerea.ValidarCnpj(cnpj))
            {
                Console.Write("\nCNPJ Inválido!");
                return;
            }

            sql = $"SELECT * FROM RestritosCNPJ WHERE CNPJ = '{cnpj}'";

            if (BancoOTF.LocalizarDados(sql, conexao))
            {
                Console.Write("\nCNPJ informado já consta como bloqueado!");
                return;
            }

            sql = $"INSERT RestritosCNPJ VALUES('{cnpj}')";

            if (BancoOTF.InsertDados(sql, conexao))
            {
                Console.Write("\nCadastrado com sucesso!");
                return;
            }

            Console.Write("\nErro ao tentar realizar cadastro!");
        }
        
        public static void ExcluirCNPJBloqueado()
        {
            SqlConnection conexao = new(BancoOTF.CaminhoConexao());
            string cnpj, sql;

            Console.Clear();

            Console.WriteLine("                                       <<<<EXCLUSAO DE BLOQUEADOS>>>>                    \n");

            Console.Write("Informe o CNPJ Bloqueado: ");
            cnpj = Console.ReadLine().Replace(".", string.Empty).Replace("-", string.Empty).Replace(",", string.Empty).Replace("/", string.Empty);

            sql = $"SELECT * FROM RestritosCNPJ WHERE CNPJ = '{cnpj}'";

            if (!BancoOTF.LocalizarDados(sql, conexao))
            {
                Console.Write("\nCNPJ informado não está cadastrado como restrito!");
                return;
            }

            sql = $"DELETE FROM RestritosCNPJ WHERE CNPJ = '{cnpj}'";

            if (BancoOTF.DeleteDados(sql, conexao))
            {
                Console.Write("\nExcluido com sucesso!");
                return;
            }

            Console.Write("\nFalha na exclusão!");
            return;

        }
        public static void ImprimirRestritosBloqueados()
        {
            SqlConnection conexao = new(BancoOTF.CaminhoConexao());
            SqlCommand cmd;
            string cpf, cnpj, sql;
            int opcao;
            Console.Clear();

            Console.WriteLine("                                                          <<<<MENU DE RESTRITOS E BLOQUEADOS>>>>                                ");

            Console.WriteLine("\nEscolha uma das opções abaixo:\n1-Exibir CPFs Restritos\n2-Exibir CPF Restrito especifico\n3-Exibir CNPJs Bloquedos\n4-Exibir CNPJ Bloqueado especifico\n0-Sair");
            Console.Write("\nOpção:");
            try
            {
                opcao = int.Parse(Console.ReadLine());
            }
            catch (Exception)
            {
                Console.Write("\nDados inválidos!\n");
                return;
            }
            if (opcao < 1 && opcao > 4 && opcao != 0)
            {
                Console.Write("\nA opção informada é inválida!\n");
                return;
            }

            if (opcao == 1)
            {
                Console.Clear();

                conexao.Open();

                sql = $"SELECT * FROM RestritosCPF";

                cmd = new(sql, conexao);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine("CPF RESTRITO\n");
                            Console.WriteLine($"CPF: {reader.GetString(0)}\n");
                        }
                    }
                }

                conexao.Close();
                Console.Write("Pressine [enter] para retornar!");
                Console.ReadKey();
                return;
            }

            if (opcao == 2)
            {
                Console.Clear();

                Console.Write("Informe o CPF que deseja localizar: ");
                cpf = Console.ReadLine().Replace(".", string.Empty).Replace("-", string.Empty).Replace(",", string.Empty).Replace("/", string.Empty);

                Console.Clear();

                conexao.Open();

                sql = $"SELECT * FROM RestritosCPF WHERE CPF = '{cpf}'";

                cmd = new(sql, conexao);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine("CPF RESTRITO\n");
                            Console.WriteLine($"CPF: {reader.GetString(0)}\n");
                        }
                    }
                }

                conexao.Close();
                Console.Write("Pressine [enter[ para retornar!");
                Console.ReadKey();
                return;
            }

            if (opcao == 3)
            {
                Console.Clear();

                conexao.Open();

                sql = $"SELECT * FROM RestritosCNPJ";

                cmd = new(sql, conexao);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine("CNPJ RESTRITO\n");
                            Console.WriteLine($"CNPJ: {reader.GetString(0)}\n");
                        }
                    }
                }

                conexao.Close();

                Console.Write("Pressine enter para retornar!");
                Console.ReadKey();
                return;
            }

            if (opcao == 0)
            {
                return;
            }

            Console.Clear();

            Console.Write("Informe o CNPJ que deseja localizar: ");
            cnpj = Console.ReadLine().Replace(".", string.Empty).Replace("-", string.Empty).Replace(",", string.Empty).Replace("/", string.Empty);

            Console.Clear();

            conexao.Open();

            sql = $"SELECT * FROM RestritosCNPJ WHERE CNPJ = '{cnpj}'";

            cmd = new(sql, conexao);

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Console.WriteLine("CPF RESTRITO\n");
                        Console.WriteLine($"CPF: {reader.GetString(0)}\n");
                    }
                }
            }

            conexao.Close();

            Console.Write("Pressine enter para retornar!");
            Console.ReadKey();
            return;
        }
        public static void PrincipalRestritosBloqueados()
        {
            int opcao = -1;
            bool validacao;

            do
            {
                Console.Clear();

                Console.WriteLine("                                  <<<<MENU RESTRITOS E BLOQUEADOS>>>>                         \n");

                Console.WriteLine("1-Cadastrar CPF\n2-Cadastrar CNPJ\n3-Excluir CPF\n4-Excluir CNPJ\n5-Imprimir Restritos e Bloqueados\n0-Sair!\nOpção: ");

                try
                {
                    opcao = int.Parse(Console.ReadLine());
                    validacao = false;
                }

                catch (Exception)
                {
                    Console.Write("\nDados inválidos!\n");
                    Console.ReadKey();
                    validacao = true;
                }

                if (opcao < 1 || opcao > 5 && opcao != 0)
                {
                    if (!validacao)
                    {
                        Console.Write("\nA opção informada é inválida!\n");
                        Console.ReadKey();
                    }
                }

                switch (opcao)
                {
                    case 1:
                        CadastrarCPFRestrito();
                        Console.ReadKey();
                        break;

                    case 2:
                        CadastrarCNPJBloqueado();
                        Console.ReadKey();
                        break;

                    case 3:
                        ExcluirCPFRestrito();
                        Console.ReadKey();
                        break;

                    case 4:
                        ExcluirCNPJBloqueado();
                        Console.ReadKey();
                        break;

                    case 5:
                        ImprimirRestritosBloqueados();
                        break;
                    case 0:
                        Console.WriteLine("Saindo...");
                        break;
                }

            } while (opcao != 0);
        }
    }
}
