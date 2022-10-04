using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PProjetoOnTheFly_Banco
{
    internal class Passagem
    {
        public static void CadastrarPassagem()
        {
            SqlConnection conexao = new(BancoOTF.CaminhoConexao());
            string sql, inscricao, parametro, idvoo, idpassagem;
            int retorno, identificador;
            bool validacao;
            double valor = -1;

            Console.Clear();

            Console.WriteLine("                                            <<<<MENU DE CADASTRO>>>>                            \n");

            Console.Write("Informe o ID do Voo: ");

            idvoo = Console.ReadLine().ToUpper();

            sql = $"SELECT * FROM Voo WHERE IDVOO = '{idvoo}'";

            if (!BancoOTF.LocalizarDados(sql, conexao))
            {
                Console.Write("\nVoo não cadastrado!");
                return;
            }

            parametro = "Situacao";

            sql = $"SELECT Situacao FROM Voo WHERE IDVOO = '{idvoo}'";

            if (BancoOTF.RetornoDados(sql, conexao, parametro) == "INATIVO")
            {
                Console.Write("\nNão é possivel cadastrar Passagem, Voo está INATIVO!");
                return;
            }

            Console.Write("Informe a série de inscrição da Aeronave: ");
            try
            {
                inscricao = Console.ReadLine().ToUpper();
            }
            catch (Exception)
            {
                Console.Write("\nDados inválidos!\n");
                return;
            }

            sql = $"SELECT * FROM Aeronave WHERE INSCRICAO = '{inscricao}'";

            if (!BancoOTF.LocalizarDados(sql, conexao))
            {
                Console.Write("\nAeronave não cadastrada!");
                return;
            }

            sql = $"SELECT * FROM AeronavePossueVoo WHERE INSCRICAO = '{inscricao}' AND IDVOO = '{idvoo}'";

            if (!BancoOTF.LocalizarDados(sql, conexao))
            {
                Console.WriteLine("\nEste Voo não está cadastrado!!");
                return;
            }

            parametro = "Passagem";

            sql = $"SELECT Passagem FROM AeronavePossueVoo WHERE INSCRICAO = '{inscricao}' AND IDVOO = '{idvoo}'";

            if (BancoOTF.RetornoDados(sql, conexao, parametro) == "BLOQUEADA")
            {
                Console.Write("\nEste Voo já possue passagem criada!");
                return;
            }

            sql = $"UPDATE AeronavePossueVoo SET Passagem = 'BLOQUEADA' WHERE INSCRICAO = '{inscricao}' AND IDVOO = '{idvoo}'";

            BancoOTF.UpdateDados(sql, conexao);

            do
            {
                Console.Write("Insira o valor das passagens deste voo em reais: R$ ");
                try
                {
                    valor = double.Parse(Console.ReadLine());
                    validacao = false;
                }
                catch (Exception)
                {
                    Console.Write("\nDados inválidos!\n");
                    validacao = true;
                }

                if (valor >= 10000 || valor < 0)
                {
                    if (!validacao)
                    {
                        Console.WriteLine("\nValor de Passagem informado excedeu o limite pré estabelecido de [R$ 9999,99]\n");
                        validacao = true;
                    }
                }

            } while (validacao);

            parametro = "Identificador";

            sql = $"SELECT Identificador FROM Passagem";

            if (string.IsNullOrWhiteSpace(BancoOTF.RetornoDados(sql, conexao, parametro)))
            {
                identificador = 0;
            }
            else
            {
                identificador = Convert.ToInt32(BancoOTF.RetornoDados(sql, conexao, parametro));
            }

            idpassagem = "PA" + (identificador + 1).ToString().PadLeft(4, '0');

            sql = $"INSERT Passagem(IDPASSAGEM, DataUltimaOperacao, ValorPassagem, Situacao, IDVOO) VALUES(@IDPASSAGEM, @DATAULTIMAOPERACAO, @VALORPASSAGEM, " +
                  $"@SITUACAO, @IDVOO);";

            conexao.Open();
            SqlCommand cmd = new(sql, conexao);

            cmd.Parameters.Add(new SqlParameter("@IDPASSAGEM", idpassagem));
            cmd.Parameters.Add(new SqlParameter("@DATAULTIMAOPERACAO", DateTime.Now.ToShortDateString()));
            cmd.Parameters.Add(new SqlParameter("@VALORPASSAGEM", valor));
            cmd.Parameters.Add(new SqlParameter("@SITUACAO", "ATIVA"));
            cmd.Parameters.Add(new SqlParameter("@IDVOO", idvoo));

            retorno = cmd.ExecuteNonQuery();

            if (retorno > 0)
            {
                retorno = 0;

                sql = $"INSERT Venda VALUES('{DateTime.Now.ToShortDateString()}', '0')";

                if (BancoOTF.InsertDados(sql, conexao))
                {
                    Console.Write("\nCadastrado com sucesso!");
                    return;
                }
            }
            conexao.Close();
            Console.Write("\nErro ao realizar cadastro!");
        }
        public static void EditarPassagem()
        {
            BancoOTF caminho = new();
            SqlConnection conexao = new(BancoOTF.CaminhoConexao());
            string sql, retorno, parametro, idpassagem;
            int opcao = 0, retorno1;
            bool validacao;
            double valor = -1;

            Console.Clear();

            Console.WriteLine("PAINEL EDIÇÃO PASSAGEM\n");

            Console.Write("Informe o ID da passagem: ");
            try
            {
                idpassagem = Console.ReadLine();
            }
            catch (Exception)
            {
                Console.Write("\nDados inválidos!\n");
                return;
            }

            sql = $"SELECT * FROM Passagem WHERE IDPASSAGEM = '{idpassagem}';";

            if (!BancoOTF.LocalizarDados(sql, conexao))
            {
                Console.Write("\nPassagem não cadastrada!");
                return;
            }

            Console.Clear();

            Console.WriteLine("PAINEL DE EDIÇÃO");

            Console.WriteLine("\nInforme qual dado deseja alterar: ");
            Console.WriteLine("\n1-Valor da passagem\n2-Situação da passagem\n0-Voltar ao meu anterior");
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

                if (opcao < 1 || opcao > 2 && opcao != 9)
                {
                    if (!validacao)
                    {
                        Console.Write("\nOpção digitada é inválida!\n");
                    }
                }

            } while (validacao);


            switch (opcao)
            {
                case 1:
                    do
                    {
                        Console.Write("\nInsira o valor da passagem em reais: R$ ");
                        try
                        {
                            valor = double.Parse(Console.ReadLine());
                            validacao = false;
                        }
                        catch (Exception)
                        {
                            Console.Write("\nDados inválidos!\n");
                            validacao = true;
                        }

                        if (valor >= 10000 || valor < 0)
                        {
                            if (!validacao)
                            {
                                Console.Write("\nO valor inserido excedeu o limite pré estabelecido de [R$ 9999,99]\n");
                                validacao = true;
                            }
                        }
                    } while (validacao);

                    sql = $"UPDATE Passagem SET DataUltimaOperacao = '{DateTime.Now.ToShortDateString()}', ValorPassagem = @VALORPASSAGEM " +
                        $"WHERE IDPASSAGEM = '{idpassagem}';";

                    conexao.Open();
                    SqlCommand cmd = new(sql, conexao);

                    cmd.Parameters.Add(new SqlParameter("@VALORPASSAGEM", valor));

                    retorno1 = cmd.ExecuteNonQuery();

                    if (retorno1 > 0)
                    {
                        sql = $"UPDATE Passagem SET DataUltimaOperacao = '{DateTime.Now.ToShortDateString()}' WHERE IDPASSAGEM = '{idpassagem}'";

                        if (BancoOTF.UpdateDados(sql, conexao))
                        {
                            Console.Write("\nCadastrado com sucesso!");
                            return;
                        }

                        Console.Write("\nErro ao realizar cadastro!");
                        break;

                    }

                    conexao.Close();
                    Console.Write("\nErro ao realizar cadastro!");
                    break;

                case 2:
                    sql = $"SELECT Situacao FROM Passagem WHERE IDPASSAGEM = '{idpassagem}';";

                    parametro = "Situacao";
                    retorno = BancoOTF.RetornoDados(sql, conexao, parametro);

                    opcao = 0;

                    if (retorno == "ATIVA")
                    {
                        Console.WriteLine("\nSituação desta Passagem está atualmente ATIVA!\nDeseja alterar a situação desta Companhia para INATIVA?");
                        Console.Write("\n1-Sim\n2-Não\n\n");
                        Console.Write("Opção: ");
                        opcao = int.Parse(Console.ReadLine());


                        if (opcao == 1)
                        {
                            sql = $"UPDATE Passagem SET Situacao = 'INATIVA' WHERE IDPASSAGEM = '{idpassagem}'";

                            if (BancoOTF.UpdateDados(sql, conexao))
                            {
                                sql = $"UPDATE Passagem SET DataUltimaOperacao = '{DateTime.Now.ToShortDateString()}' WHERE IDPASSAGEM = '{idpassagem}'";

                                if (BancoOTF.UpdateDados(sql, conexao))
                                {
                                    Console.Write("\nCadastrado com sucesso!");
                                    return;
                                }

                                Console.Write("\nError ao realizar cadastro!");
                                break;
                            }

                            Console.Write("\nError na alteração!");
                            return;
                        }

                        Console.Write("\nAté logo!");
                        return;
                    }

                    Console.WriteLine("\nSituação desta Passagem está atualmente INATIVA!\nDeseja alterar a situação deste Passagem para ATIVA?");
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
                            Console.Write("\nDados inválidos!\n");
                            validacao = true;
                        }
                        if (opcao < 1 || opcao > 2)
                        {
                            if (!validacao)
                            {
                                Console.Write("\nA opção informada é inválida!\n");
                            }
                        }
                    } while (validacao);

                    if (opcao == 1)
                    {
                        sql = $"UPDATE Passagem SET Situacao = 'ATIVA' WHERE IDPASSAGEM = '{idpassagem}'";

                        if (BancoOTF.UpdateDados(sql, conexao))
                        {
                            sql = $"UPDATE Passagem SET DataUltimaOperacao = '{DateTime.Now.ToShortDateString()}' WHERE IDPASSAGEM = '{idpassagem}'";

                            if (BancoOTF.UpdateDados(sql, conexao))
                            {
                                Console.Write("\nCadastrado com sucesso!");
                                return;
                            }

                            Console.Write("\nErro ao realizar cadastro!");
                            break;
                        }

                        Console.Write("\nErro na alteração!");
                        return;
                    }

                    Console.Write("\nAté logo!");
                    break;
            }
        }
        public static void ImprimirPassagem()
        {
            BancoOTF caminho = new();
            SqlConnection conexao = new(BancoOTF.CaminhoConexao());
            SqlCommand cmd;
            int opcao = 0;
            bool validacao;
            string sql, idpassagem;

            Console.Clear();

            
            Console.WriteLine("\nInforme a opção desejada:\n1-Ver Passagens cadastradas\n2-Ver uma especifica\n3-Voltar ao menu anterior!");
            
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

                if (opcao < 1 || opcao > 2 && opcao != 9)
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
                conexao.Open();

                cmd = new("SELECT passagem.IDVOO, passagem.IDPASSAGEM, passagem.ValorPassagem, passagem.DataUltimaOperacao, " +
                    "passagem.Situacao FROM Passagem WHERE passagem.Situacao = 'ATIVA';", conexao);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    Console.Clear();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine("Passagem\n");
                            Console.WriteLine($"IDVOO: {reader.GetString(0)}");
                            Console.WriteLine($"IDPASSAGEM: {reader.GetString(1)}");
                            Console.WriteLine($"ValorPassagem: R$ {reader.GetDecimal(2).ToString("F2")}");
                            Console.WriteLine($"Data Ultime Operação: {reader.GetDateTime(3).ToShortDateString()}");
                            Console.WriteLine($"Situacao: {reader.GetString(4)}\n");
                        }

                        Console.Write("Pressione enter para continuar!");
                        Console.ReadKey();
                        conexao.Close();
                        return;
                    }

                    Console.Write("Ainda não há passagens cadastradas ou ativas!");
                    Console.ReadKey();
                    return;
                }
            }

            if (opcao == 0)
            {
                return;
            }

            Console.Clear();


            Console.Write("Informe qual ID da Passagem que deseja localizar: ");
            try
            {
                idpassagem = Console.ReadLine();
            }
            catch (Exception)
            {
                Console.WriteLine("\nParametro de dado inválido!");
                return;
            }

            sql = $"SELECT * FROM Passagem WHERE IDPASSAGEM = '{idpassagem}';";

            if (!BancoOTF.LocalizarDados(sql, conexao))
            {
                Console.WriteLine("\nVoo não cadastrado!");
                return;
            }

            conexao.Open();

            cmd = new($"SELECT passagem.IDVOO, passagem.IDPASSAGEM, passagem.ValorPassagem, passagem.DataUltimaOperacao, passagem.Situacao FROM Passagem WHERE passagem.IDPASSAGEM = '{idpassagem}';", conexao);


            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                Console.Clear();

                while (reader.Read())
                {
                    Console.WriteLine("Passagem\n");
                    Console.WriteLine($"IDVOO: {reader.GetString(0)}");
                    Console.WriteLine($"IDPASSAGEM: {reader.GetString(1)}");
                    Console.WriteLine($"ValorPassagem: R$ {reader.GetDecimal(2).ToString("F2")}");
                    Console.WriteLine($"Data Ultime Operação: {reader.GetDateTime(3).ToShortDateString()}");
                    Console.WriteLine($"Situacao: {reader.GetString(4)}\n");
                }
            }

            Console.Write("Pressione enter para continuar!");
            Console.ReadKey();
            conexao.Close();
            return;
        }
        public static void PrincipalPassagem()
        {
            int opcao = 0;
            bool condicaoDeParada;
           

            do
            {
                Console.Clear();

                Console.WriteLine("                                                   <<<<MENU DE PASSAGENS>>>>                                 \n");

                Console.WriteLine("1-Cadastrar Passagem\n2-Editar Passagem\n3-Imprimir Passagens\nOpção: ");

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

                if (opcao < 1 || opcao > 3)
                {
                    if (!condicaoDeParada)
                    {
                        Console.Write("\nA opção informada é inválida!\n");
                        Console.ReadKey();
                    }
                }

                switch (opcao)
                {
                    case 1:
                        CadastrarPassagem();
                        Console.ReadKey();
                        break;

                    case 2:
                        EditarPassagem();
                        Console.ReadKey();
                        break;

                    case 3:
                        ImprimirPassagem();
                        break;
                }
            } while (opcao != 0);
        }

    }
}
