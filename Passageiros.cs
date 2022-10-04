using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace PProjetoOnTheFly_Banco
{
    internal class Passageiros
    {
        public static bool ReadCPF(string cpf)
        {
            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            string tempCpf;
            string digito;
            int soma;
            int resto;
            cpf = cpf.Trim();
            cpf = cpf.Replace(".", "").Replace("-", "");
            if (cpf.Length != 11)
                return false;
            tempCpf = cpf.Substring(0, 9);
            soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = resto.ToString();
            tempCpf = tempCpf + digito;
            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = digito + resto.ToString();
            return cpf.EndsWith(digito);
        }
        public static void CadastrarPassageiro()
        {
            SqlConnection conexao = new(BancoOTF.CaminhoConexao());
            string sql, cpf, nome = "", sexo;
            bool validacao = false;
            DateTime nascimento = new();
            int opcao = 0;

            Console.Clear();

            Console.WriteLine("Formulário de cadastro:\n");

            do
            {
                Console.Write("Informe o nome completo do passeiro: ");
                try
                {
                    nome = Console.ReadLine().ToUpper();
                    validacao = false;
                }
                catch (Exception)
                {
                    Console.Write("\nDados inválidos!\n");
                    validacao = true;
                }

                if (nome.Length == 0)
                {
                    if (!validacao)
                    {
                        Console.WriteLine("\nNome informado não pode ser nullo!\n");
                        validacao = true;
                    }
                }

            } while (validacao);


            Console.Write("Digite o número do CPF do passageiro: ");
            cpf = Console.ReadLine().Replace(".", string.Empty).Replace("-", string.Empty).Replace(",", string.Empty).Replace("/", string.Empty);

            if (!ReadCPF(cpf))
            {
                Console.WriteLine("\nCPF inválido!\n");
                return;
            }


            sql = $"SELECT * FROM Passageiro WHERE CPF = '{cpf}'";

            if (BancoOTF.LocalizarDados(sql, conexao))
            {
                Console.Write("\nPassageiro já possue um cadastro!");
                return;
            }

            do
            {
                Console.Write("Informe sua data de nascimento: ");
                try
                {
                    nascimento = DateTime.Parse(Console.ReadLine());
                    validacao = false;
                }
                catch (Exception)
                {
                    Console.WriteLine("\nParametro digitado é inválido!");
                    Console.WriteLine("Formato correto: [dd/mm/yyyy]\n");
                    validacao = true;
                }

            } while (validacao);

            Console.WriteLine("\nEscolha a opção desejada:");
            Console.WriteLine("\n1-Feminino\n2-Masculino\n3-Não Binário");
            do
            {
                Console.Write("\nOpção: ");
                try
                {
                    opcao = int.Parse(Console.ReadLine());
                    validacao = false;
                }

                catch (Exception)
                {
                    Console.Write("\nDados inválidos!\n");
                    validacao = true;
                }

                if (opcao < 1 || opcao > 3)
                {
                    if (!validacao)
                    {
                        Console.Write("\nA opção informada é inválida!\n");
                        validacao = true;
                    }
                }

            } while (validacao);

            if (opcao == 1)
            {
                sexo = "FEMININO";
            }
            else
            {
                if (opcao == 2)
                {
                    sexo = "MASCULINO";
                }

                else
                {
                    sexo = "NAO-BINARIO";
                }
            }

            sql = $"INSERT Passageiro VALUES('{cpf}', '{nome}', '{nascimento}', '{sexo}', '{DateTime.Now.ToShortDateString()}', 'ATIVO');";

            if (BancoOTF.InsertDados(sql, conexao))
            {
                Console.Write("\nCadastrado com sucesso!");
                return;
            }

            Console.Write("\nError ao realizar cadastro!");
        }
        public static void EditarPassageiro()
        {
            BancoOTF caminho = new();
            SqlConnection conexao = new(BancoOTF.CaminhoConexao());
            DateTime nascimento = new();
            string cpf, sql, nome, parametro, retorno;
            bool validacao;
            int opcao = 10;

            Console.Clear();

            Console.WriteLine("MENU DE EDIÇÃO:\n");

            Console.Write("Informe o CPF do passageiro: ");
            cpf = Console.ReadLine().Replace(".", string.Empty).Replace("-", string.Empty).Replace(",", string.Empty).Replace("/", string.Empty);

            sql = $"SELECT * From Passageiro WHERE CPF = '{cpf}'";

            if (!BancoOTF.LocalizarDados(sql, conexao))
            {
                Console.Write("CPF informado não possue cadastro!");
                return;
            }

            do
            {
                Console.Clear();

                Console.WriteLine("MENU DE EDIÇÃO\n");

                Console.WriteLine("Informe qual dado deseja alterar:\n1-Nome\n2-Data de Nascimento.\n3-Sexo.\n4-Situação.\n0-Voltar ao menu anterior.\nOpção: ");

                try
                {
                    opcao = int.Parse(Console.ReadLine());
                    validacao = false;
                }

                catch (Exception)
                {
                    Console.Write("\nDados inválidos!\n");
                    validacao = true;
                }

                if (opcao < 1 || opcao > 4 && opcao != 0)
                {
                    if (!validacao)
                    {
                        Console.Write("\nOpção digitada é inválida!\n");
                        validacao = true;
                    }
                }

            } while (validacao);

            if (opcao == 0)
            {
                Console.Write("\nAté Logo");
                return;
            }


            if (opcao == 1)
            {
                do
                {
                    Console.Write("\nInforme o nome do Passageiro: ");
                    nome = Console.ReadLine().ToUpper();
                    validacao = false;

                    if (nome.Length == 0)
                    {
                        Console.Write("\nNome não pode ser nullo!\n");
                        validacao = true;
                    }

                } while (validacao);

                sql = $"UPDATE Passageiro SET Nome = '{nome}' WHERE CPF = '{cpf}';";

                if (BancoOTF.UpdateDados(sql, conexao))
                {
                    Console.Write("\nAlterado com sucesso!");
                    return;
                }

                Console.Write("\nErro na alteração!");
                return;
            }

            if (opcao == 2)
            {

                do
                {
                    Console.Write("\nInforme a data de nascimento do passageiro: ");
                    try
                    {
                        nascimento = DateTime.Parse(Console.ReadLine());
                        validacao = false;
                    }
                    catch (Exception)
                    {
                        Console.Write("\nParametro de dados inválidos!\n");
                        validacao = true;
                    }

                    if (nascimento > DateTime.Now)
                    {
                        Console.Write("\nData de nascimento não pode ser futura!\n");
                        validacao = true;
                    }
                } while (validacao);

                sql = $"UPDATE Passageiro SET Nascimento = '{nascimento}' WHERE CPF = '{cpf}';";

                if (BancoOTF.UpdateDados(sql, conexao))
                {
                    Console.Write("\nAlterado com sucesso!");
                    return;
                }

                Console.Write("\nErro na alteração!");
                return;

            }


            if (opcao == 3)
            {
                Console.WriteLine("\nEscolha uma das opções:\n1-Feminino\n2-Masculino\n3-Não Binário");
                do
                {
                    Console.Write("\nOpção: ");
                    try
                    {
                        opcao = int.Parse(Console.ReadLine());
                        validacao = false;
                    }

                    catch (Exception)
                    {
                        Console.Write("\nDados inválidos!\n");
                        validacao = true;
                    }

                    if (opcao < 1 || opcao > 3)
                    {
                        if (!validacao)
                        {
                            Console.Write("\nA opção informada é inválida!\n");
                            validacao = true;
                        }
                    }

                } while (validacao);

                if (opcao == 1)
                {
                    sql = $"UPDATE Passageiro SET Sexo = 'FEMININO' WHERE CPF = '{cpf}';";

                    if (BancoOTF.UpdateDados(sql, conexao))
                    {
                        Console.Write("\nAlterado com sucesso!");
                        return;
                    }

                    Console.Write("\nErro na alteração!");
                    return;
                }
                else
                {
                    if (opcao == 2)
                    {
                        sql = $"UPDATE Passageiro SET Sexo = 'MASCULINO' WHERE CPF = '{cpf}';";

                        if (BancoOTF.UpdateDados(sql, conexao))
                        {
                            Console.Write("\nAlterado com sucesso!");
                            return;
                        }

                        Console.Write("\nErro na alteração!");
                        return;
                    }

                    else
                    {
                        sql = $"UPDATE Passageiro SET Sexo = 'INDEFINIDO' WHERE CPF = '{cpf}';";

                        if (BancoOTF.UpdateDados(sql, conexao))
                        {
                            Console.Write("\nAlterado com sucesso!");
                            return;
                        }

                        Console.Write("\nErro na alteração!");
                        return;
                    }
                }
            }


            parametro = "Situacao";

            sql = $"SELECT Situacao FROM Passageiro WHERE CPF = '{cpf}';";

            retorno = BancoOTF.RetornoDados(sql, conexao, parametro);

            if (retorno == "ATIVO")
            {
                Console.WriteLine("\nPassageiro está ATIVO!");
                Console.WriteLine("Deseja alterar a situação do passageiro para INATIVO:\n1-Sim\n2-Não");
                do
                {
                    Console.Write("\nOpção: ");

                    try
                    {
                        opcao = int.Parse(Console.ReadLine());
                        validacao = false;
                    }

                    catch (Exception)
                    {
                        Console.WriteLine("\nDado inválido!\n");
                        validacao = true;
                    }

                    if (opcao < 1 || opcao > 2)
                    {
                        if (!validacao)
                        {
                            Console.WriteLine("\nEscolha uma das opções exibidas!\n");
                            validacao = true;
                        }
                    }

                } while (validacao);

                if (opcao == 1)
                {
                    sql = $"UPDATE Passageiro SET Situacao = 'INATIVO' WHERE CPF = '{cpf}';";

                    if (BancoOTF.UpdateDados(sql, conexao))
                    {
                        Console.Write("\nAlterado com sucesso!");
                        return;
                    }

                    Console.Write("\nErro na alteração!");
                    return;
                }

                Console.Write("\nAté logo");
                return;
            }


            Console.WriteLine("\nPassageiro está INATIVO!");
            Console.WriteLine("Deseja alterar a situação do passageiro para ATIVO: \n1-Sim\n2-Não");
            do
            {
                Console.Write("\nOpção: ");

                try
                {
                    opcao = int.Parse(Console.ReadLine());
                    validacao = false;
                }

                catch (Exception)
                {
                    Console.WriteLine("\nDado inválido!\n");
                    validacao = true;
                }

                if (opcao < 1 || opcao > 2)
                {
                    if (!validacao)
                    {
                        Console.WriteLine("\nEscolha uma das opções exibidas!\n");
                        validacao = true;
                    }
                }

            } while (validacao);

            if (opcao == 1)
            {
                sql = $"UPDATE Passageiro SET Situacao = 'ATIVO' WHERE CPF = '{cpf}';";

                if (BancoOTF.UpdateDados(sql, conexao))
                {
                    Console.Write("\nAlterado com sucesso!");
                    return;
                }

                Console.Write("\nErro na alteração!");
                return;
            }

            Console.Write("\nAté logo");
            return;
        }
        public static void ImprimirPassageiros()
        {
            BancoOTF caminho = new();
            SqlConnection conexao = new(BancoOTF.CaminhoConexao());
            SqlCommand cmd;
            int opcao = 0;
            bool validacao;
            string sql, cpf;

            Console.Clear();

            Console.WriteLine("Ola,");
            Console.WriteLine("\nEscolha a opção desejada:\n1-Ver Passageiros Cadastrados\n2-Ver um especifico\n0-Voltar ao menu anterior");
            do
            {
                Console.Write("\nOpção: ");
                try
                {
                    opcao = int.Parse(Console.ReadLine());
                    validacao = false;
                }
                catch (Exception)
                {
                    Console.Write("\nParametro de dados inválidos!\n");
                    validacao = true;
                }

                if (opcao < 1 || opcao > 2 && opcao != 9)
                {
                    if (!validacao)
                    {
                        Console.Write("\nA Opção informada é inválida!\n");
                        validacao = true;
                    }
                }
            } while (validacao);

            if (opcao == 1)
            {
                conexao.Open();

                cmd = new("SELECT * FROM Passageiro WHERE Situacao = 'ATIVO';", conexao);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    Console.Clear();

                    while (reader.Read())
                    {
                        Console.WriteLine("PASSAGEIROS CADASTRADOS\n");
                        Console.WriteLine($"CPF: {reader.GetString(0)}");
                        Console.WriteLine($"Nome: {reader.GetString(1)}");
                        Console.WriteLine($"Data de Nascimento: {reader.GetDateTime(2).ToShortDateString()}");
                        Console.WriteLine($"Sexo: {reader.GetString(3)}");
                        Console.WriteLine($"Data Ulima Compra: R$ {reader.GetDateTime(4).ToShortDateString()}");
                        Console.WriteLine($"Situação: {reader.GetString(5)}");
                    }
                }

                Console.Write("\nPressione enter para continuar!");
                conexao.Close();
                Console.ReadKey();
                return;
            }

            if (opcao == 0)
            {
                return;
            }

            Console.Clear();

            Console.Write("\nInforme o CPF do Passageiro que deseja localizar: ");
            try
            {
                cpf = Console.ReadLine().Replace(".", string.Empty).Replace("-", string.Empty).Replace(",", string.Empty).Replace("/", string.Empty);
            }
            catch (Exception)
            {
                Console.Write("\nParametro de dados inválidos!\n");
                return;
            }

            sql = $"SELECT * FROM Passageiro WHERE CPF = '{cpf}';";


            if (!BancoOTF.LocalizarDados(sql, conexao))
            {
                Console.WriteLine("\nPassageiro não cadastrado!");
                return;
            }

            conexao.Open();

            cmd = new($"SELECT * FROM Passageiro WHERE CPF = '{cpf}';", conexao);

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                Console.Clear();

                while (reader.Read())
                {
                    Console.WriteLine("PASSAGEIROS CADASTRADOS\n");
                    Console.WriteLine($"CPF: {reader.GetString(0)}");
                    Console.WriteLine($"Nome: {reader.GetString(1)}");
                    Console.WriteLine($"Data de Nascimento: {reader.GetDateTime(2).ToShortDateString()}");
                    Console.WriteLine($"Sexo: {reader.GetString(3)}");
                    Console.WriteLine($"Data Ulima Compra: R$ {reader.GetDateTime(4).ToShortDateString()}");
                    Console.WriteLine($"Situação: {reader.GetString(5)}");
                }
            }

            Console.Write("\nPressione enter para continuar!");
            Console.ReadKey();
            conexao.Close();
            return;
        }
        public static void PrincipalPassageiro()
        {
            int opcao = -1;
            bool validacao;

            do
            {
                Console.Clear();

                Console.WriteLine("                                          <<<<MENU PASSAGEIROS>>>>                             \n");

                Console.WriteLine("1-Cadastrar Passageiro\n2-Editar Passageiro\n3-Imprimir Passageiro\n0-Voltar ao menu anterior\nOpção: ");

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

                if (opcao < 0 || opcao > 4 && opcao != 0)
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
                        CadastrarPassageiro();
                        Console.ReadKey();
                        break;

                    case 2:
                        EditarPassageiro();
                        Console.ReadKey();
                        break;

                    case 3:
                        ImprimirPassageiros();
                        break;
                }

            } while (opcao != 0);
        }

    }
        



    
}
