using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PProjetoOnTheFly_Banco
{
    internal class Aeronave
    {
        public static void CadastrarAeronave()
        {
            SqlConnection conexao = new(BancoOTF.CaminhoConexao());
            string sql;

            int opcao = 0, capacidade = 0;
            bool condicaoDeParada;
            string inscricao, codigoInscricao, cnpj, parametro;

            Console.Clear();

            Console.WriteLine("TELA DE CADASTRO\n");

            Console.Write("Informe o CNPJ da Companhia Aerea responsável pela Aeronave: ");
            cnpj = Console.ReadLine().Replace(".", string.Empty).Replace("/", string.Empty).Replace("-", string.Empty).Replace("*", string.Empty);

            sql = $"SELECT * FROM CompanhiaAerea WHERE CNPJ = '{cnpj}'";

            if (!BancoOTF.LocalizarDados(sql, conexao))
            {
                Console.Write("\nCNPJ informado não está cadastrado como uma Companhia Aerea!");
                return;
            }

            sql = $"SELECT * FROM RestritosCNPJ WHERE CNPJ = '{cnpj}'";

            if (BancoOTF.LocalizarDados(sql, conexao))
            {
                Console.Write("\nCompanhia com CNPJ restrito, não é possivel realizar cadastro de Aeronaves!");
                return;
            }

            parametro = "Situacao";

            sql = $"SELECT Situacao FROM CompanhiaAerea WHERE CNPJ = '{cnpj}'";

            if (BancoOTF.RetornoDados(sql, conexao, parametro) == "INATIVA")
            {
                Console.WriteLine("\nCompanhia Aerea com status INATIVO, não é possivel cadastrar Aeronaves!");
                return;
            }

            do
            {
                Console.WriteLine("\nQual o código inicial da inscrição da Aeronave:  ");
                Console.WriteLine("\n1-PT\n2-PP\n3-PR\n4-PU\n");
                Console.Write("Opção: ");

                try
                {
                    opcao = int.Parse(Console.ReadLine());
                    condicaoDeParada = false;
                }

                catch (Exception)
                {
                    Console.Write("\nDados inválidos!\n");
                    condicaoDeParada = true;
                }

                if (opcao < 1 || opcao > 4)
                {
                    if (!condicaoDeParada)
                    {
                        Console.Write("\nA opção informada é inválida!\n");
                        condicaoDeParada = true;
                    }
                }

            } while (condicaoDeParada);

            do
            {
                Console.Write("\nInforme a incrição da Aeronave sem o código: ");

                inscricao = Console.ReadLine().ToUpper();
                condicaoDeParada = false;

                if (inscricao.Length != 3)
                {
                    Console.Write("\nInscrição sem o código deve ter 3 letras!\n");
                    condicaoDeParada = true;
                }

            } while (condicaoDeParada);

            if (opcao == 1)
            {
                codigoInscricao = "PT" + inscricao;
            }

            else if (opcao == 2)
            {
                codigoInscricao = "PP" + inscricao;
            }

            else
            {
                if (opcao == 3)
                {
                    codigoInscricao = "PR" + inscricao;
                }

                else
                {
                    codigoInscricao = "PU" + inscricao;
                }
            }

            sql = $"SELECT * FROM Aeronave WHERE INSCRICAO = '{codigoInscricao}'";

            if (BancoOTF.LocalizarDados(sql, conexao))
            {
                Console.Write("\nCódigo de inscrição informado já está cadastrado em outra aeronave!");
                return;
            }

            do
            {
                Console.Write("Qual a capacidade de passageiros da Aeronave: ");

                try
                {
                    capacidade = int.Parse(Console.ReadLine());
                    condicaoDeParada = false;
                }

                catch (Exception)
                {
                    Console.Write("\nParametro de dados inválidos!\n");
                    condicaoDeParada = true;
                }

                if (capacidade < 0 || capacidade > 999)
                {
                    Console.WriteLine("\nCapacidade da Aeronave, não pode ser menor que 0\n");
                    condicaoDeParada = true;
                }

            } while (condicaoDeParada);

            sql = $"INSERT Aeronave VALUES('{codigoInscricao}', '{capacidade}', '{DateTime.Now.ToShortDateString()}', '{DateTime.Now.ToShortDateString()}', 'ATIVA')";

            if (BancoOTF.InsertDados(sql, conexao))
            {
                sql = $"INSERT CompanhiaPossueAeronave VALUES('{cnpj}', '{codigoInscricao}')";

                if (BancoOTF.InsertDados(sql, conexao))
                {
                    Console.Write("\nCadastrado com sucesso!");
                    return;
                }
            }

            Console.Write("\nErro ao realizar cadastro!");
        }

        public static void EditarAeronave()
        {
            SqlConnection conexao = new(BancoOTF.CaminhoConexao());
            string sql, cnpj, inscricao, parametro, retorno;
            int capacidade = -1, opcao = 0;
            bool validacao;

            Console.Clear();

            Console.WriteLine("PAINEL DE EDIÇÃO!\n");

            Console.Write("Informe o CNPJ da Companhia Aerea responsável pela Aeronave: ");
            cnpj = Console.ReadLine().Replace(".", string.Empty).Replace("/", string.Empty).Replace("-", string.Empty).Replace("*", string.Empty);

            sql = $"SELECT * FROM CompanhiaAerea WHERE CNPJ = '{cnpj}'";

            if (!BancoOTF.LocalizarDados(sql, conexao))
            {
                Console.Write("\nCNPJ informado não está cadastrado como uma Companhia Aerea!");
                return;
            }

            Console.Write("\nInforme a INSCRIÇÃO da Aeronave que deseja editar: ");
            inscricao = Console.ReadLine().ToUpper().Replace(".", string.Empty).Replace("/", string.Empty).Replace("-", string.Empty).Replace("*", string.Empty);

            sql = $"SELECT * FROM CompanhiaPossueAeronave WHERE CNPJ = '{cnpj}' AND INSCRICAO = '{inscricao}'";

            if (!BancoOTF.LocalizarDados(sql, conexao))
            {
                Console.Write("\nINSCRICAO informada não tem associação com o CNPJ da Companhia informado!");
                return;
            }

            Console.WriteLine("\nInforme qual será alteração: \n");

            Console.Write("1-Capacidade\n2-Situação\n\nOpcão: ");
            int resposta = int.Parse(Console.ReadLine());

            if (resposta == 1)
            {
                do
                {
                    Console.Write("\nInforme qual a capicade da Aeronave: ");
                    try
                    {
                        capacidade = int.Parse(Console.ReadLine());
                        validacao = false;
                    }
                    catch (Exception)
                    {
                        Console.Write("\nDados inválidos!\n");
                        validacao = true;
                    }

                    if (capacidade < 0)
                    {
                        if (!validacao)
                        {
                            Console.WriteLine("\nCapacidade de passageiros não pode ser menor que 0!\n");
                            validacao = true;
                        }
                    }

                } while (validacao);

                sql = $"UPDATE Aeronave SET Capacidade = '{capacidade}' WHERE INSCRICAO = '{inscricao}'";

                if (BancoOTF.UpdateDados(sql, conexao))
                {
                    Console.Write("\nAlterado com sucesso!");
                    return;
                }

                Console.Write("\nErro na alteração!");
                return;
            }

            sql = $"SELECT Situacao FROM Aeronave WHERE INSCRICAO = '{inscricao}'";

            parametro = "Situacao";

            retorno = BancoOTF.RetornoDados(sql, conexao, parametro);

            if (retorno == "ATIVA")
            {
                Console.WriteLine("\nSituação desta Aeronave está atualmente ATIVA!\nDeseja alterar a situação desta Aeronave para INATIVA?");
                Console.Write("\n1-Sim\n2-Não\n\n");
                do
                {
                    Console.Write("Opção: ");
                    try
                    {
                        opcao = int.Parse(Console.ReadLine());
                        validacao = false;
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("\nParametro de entrada inválido!\n");
                        validacao = true;
                    }
                    if (opcao < 1 || opcao > 2)
                    {
                        if (!validacao)
                        {
                            Console.WriteLine("\nOpção inválida!\n");
                        }
                    }
                } while (validacao);


                if (opcao == 1)
                {
                    sql = $"UPDATE Aeronave SET Situacao = 'INATIVA' WHERE INSCRICAO = '{inscricao}'";

                    if (BancoOTF.UpdateDados(sql, conexao))
                    {
                        Console.Write("\nAlterado com sucesso!");
                        return;
                    }

                    Console.Write("\nError na alteração!");
                    return;
                }

                Console.WriteLine("\nAté logo!");
                return;
            }

            Console.WriteLine("\nSituação desta Aeronave está atualmente INATIVA!\nDeseja alterar a situação desta Aeronave para ATIVA?");
            Console.Write("\n1-Sim\n2-Não\n\nOpção: ");
            opcao = int.Parse(Console.ReadLine());

            if (opcao == 1)
            {
                sql = $"UPDATE Aeronave SET Situacao = 'ATIVA' WHERE INSCRICAO = '{inscricao}'";

                if (BancoOTF.UpdateDados(sql, conexao))
                {
                    Console.Write("\nAlterado com sucesso!");
                    return;
                }

                Console.Write("\nErro na alteração!");
                return;
            }

            Console.WriteLine("\nAté logo!");
        }
        public static void ImprimirAeronave()
        {
            SqlConnection conexao = new(BancoOTF.CaminhoConexao());
            SqlCommand cmd;
            int opcao = 0;
            bool validacao;
            string inscricao, sql;

            Console.Clear();

            Console.WriteLine("PAINEL DE IMPRESSAO\n");
            Console.WriteLine("Escolha a opção desejada:\n1-Ver todas Aeronaves cadastradas\n2-Ver uma especifica\n0-Voltar ao menu anterior");
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

                if (opcao < 1 || opcao > 2 && opcao != 0)
                {
                    if (!validacao)
                    {
                        Console.Write("\nOpção digitada é inválida!\n");
                        validacao = true;
                    }
                }

            } while (validacao);

            if (opcao == 1)
            {
                conexao.Open();

                cmd = new("SELECT companhiaaerea.RazaoSocial, companhiaaerea.CNPJ, aeronave.INSCRICAO, " +
                          "aeronave.Capacidade, aeronave.UltimaVenda, aeronave.DataCadastro, " +
                          "aeronave.Situacao FROM Aeronave JOIN CompanhiaPossueAeronave ON  " +
                          "companhiapossueaeronave.INSCRICAO = aeronave.INSCRICAO " +
                          "JOIN CompanhiaAerea ON companhiaaerea.CNPJ = companhiapossueaeronave.CNPJ " +
                         $"WHERE aeronave.Situacao = 'ATIVA';", conexao);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    Console.Clear();

                    while (reader.Read())
                    {
                        Console.WriteLine("AERONAVE\n");
                        Console.WriteLine($"Proprietaria: {reader.GetString(0)}");
                        Console.WriteLine($"CNPJ: {reader.GetString(1)}");
                        Console.WriteLine($"Inscrição Aeronave: {reader.GetString(2)}");
                        Console.WriteLine($"Capacidade: {reader.GetInt32(3)}");
                        Console.WriteLine($"Última Venda: {reader.GetDateTime(4).ToShortDateString()}");
                        Console.WriteLine($"Data do Cadastro: {reader.GetDateTime(5).ToShortDateString()}");
                        Console.WriteLine($"Situação: {reader.GetString(6)}\n");
                    }
                }

                Console.Write("Pressione enter para continuar!");
                Console.ReadKey();
                conexao.Close();
                return;
            }

            if (opcao == 0)
            {
                return;
            }

            Console.Clear();
            do
            {
                Console.Write("\nInforme qual a INSCRIÇÃO da Aeronave que deseja localizar: ");

                inscricao = Console.ReadLine().ToUpper().Replace(".", string.Empty).Replace("/", string.Empty).Replace("-", string.Empty).Replace("*", string.Empty);
                validacao = false;

                if (inscricao.Length > 5)
                {
                    Console.WriteLine("Inscrição da Aeronave possue 5 letras alfanuméricas!");
                    validacao = true;
                }

            } while (validacao);


            sql = "SELECT companhiaaerea.RazaoSocial, companhiaaerea.CNPJ, aeronave.INSCRICAO, " +
                          "aeronave.Capacidade, aeronave.UltimaVenda, aeronave.DataCadastro, aeronave.Situacao " +
                          "FROM Aeronave JOIN CompanhiaPossueAeronave ON " +
                          "companhiapossueaeronave.INSCRICAO = aeronave.INSCRICAO " +
                          $"JOIN CompanhiaAerea ON companhiapossueaeronave.CNPJ = companhiaaerea.CNPJ WHERE aeronave.INSCRICAO = '{inscricao}'";

            if (!BancoOTF.LocalizarDados(sql, conexao))
            {
                Console.Write("\nINSCRIÇÃO informada não está cadastrado em nosso banco de dados!");
                return;
            }

            conexao.Open();

            cmd = new("SELECT companhiaaerea.RazaoSocial, companhiaaerea.CNPJ, aeronave.INSCRICAO, " +
                          "aeronave.Capacidade, aeronave.UltimaVenda, aeronave.DataCadastro, aeronave.Situacao " +
                          "FROM Aeronave JOIN CompanhiaPossueAeronave ON " +
                          "companhiapossueaeronave.INSCRICAO = aeronave.INSCRICAO " +
                          $"JOIN CompanhiaAerea ON companhiapossueaeronave.CNPJ = companhiaaerea.CNPJ WHERE aeronave.INSCRICAO = '{inscricao}'", conexao);

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                Console.Clear();

                while (reader.Read())
                {
                    Console.WriteLine("AERONAVE\n");
                    Console.WriteLine($"Propietário: {reader.GetString(0)}");
                    Console.WriteLine($"CNPJ: {reader.GetString(1)}");
                    Console.WriteLine($"Inscrição: {reader.GetString(2)}");
                    Console.WriteLine($"Capacidade: {reader.GetInt32(3)}");
                    Console.WriteLine($"Última Venda: {reader.GetDateTime(4).ToShortDateString()}");
                    Console.WriteLine($"Data Cadastro: {reader.GetDateTime(5).ToShortDateString()}");
                    Console.WriteLine($"Situacao: {reader.GetString(6)}\n");
                }
            }

            conexao.Close();
            Console.Write("Pressione enter para continuar!");
            Console.ReadKey();
            return;
        }
        public static void AcessarAeronave()
        {
            int opcao = -1;
            bool condicaoDeParada;

            do
            {
                Console.Clear();

                Console.WriteLine("MENU DAS AERONAVES\n");

                Console.WriteLine("1-Cadastrar Aeronave\n2-Editar Aeronave\n3-Imprimir Aeronaves");
                Console.Write("\nOpção: ");

                try
                {
                    opcao = int.Parse(Console.ReadLine());
                    condicaoDeParada = false;
                }

                catch (Exception)
                {
                    Console.Write("\nDados inválidos!\n");
                    Console.ReadKey();
                    condicaoDeParada = true;
                }

                if (opcao < 1 || opcao > 3 && opcao != 0)
                {
                    if (!condicaoDeParada)
                    {
                        Console.Write("\nOpção digitada é inválida!\n");
                        Console.ReadKey();
                    }
                }

                switch (opcao)
                {
                    case 1:
                        CadastrarAeronave();
                        Console.ReadKey();
                        break;

                    case 2:
                        EditarAeronave();
                        Console.ReadKey();
                        break;

                    case 3:
                        ImprimirAeronave();
                        break;
                }

            } while (opcao != 0);
        }

            
    }
    
}
