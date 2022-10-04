using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PProjetoOnTheFly_Banco
{
    internal class Voo
    {
        public static void CadastrarVoo()
        {
            SqlConnection conexao = new(BancoOTF.CaminhoConexao());
            string sql, inscricao, retorno, parametro, cnpj, idvoo;
            int identificador = 0, capacidade = 0;
            Destinos destino = new();
            DateTime dataVoo = DateTime.Now;
            bool condicaoDeSaida;

            Console.Clear();

            Console.WriteLine("PAINEL DE CADASTRO\n");

            Console.Write("Informe a inscrição da Aeronave que irá realizar o voo: ");
            inscricao = Console.ReadLine().ToUpper();

            sql = $"SELECT * FROM Aeronave WHERE INSCRICAO = '{inscricao}';";

            if (!BancoOTF.LocalizarDados(sql, conexao))
            {
                Console.Write("\nAeronave não localizada!");
                return;
            }

            parametro = "Situacao";

            sql = $"SELECT Situacao FROM Aeronave WHERE INSCRICAO = '{inscricao}'";

            retorno = BancoOTF.RetornoDados(sql, conexao, parametro);

            if (retorno == "INATIVA")
            {
                Console.Write("\nAeronave inativada, não é possivel efetuar o cadastro do Voo!");
                return;
            }

            parametro = "CNPJ";

            sql = $"SELECT CNPJ FROM CompanhiaPossueAeronave WHERE INSCRICAO = '{inscricao}';";

            cnpj = BancoOTF.RetornoDados(sql, conexao, parametro);

            parametro = "Identificador";

            sql = $"SELECT Identificador FROM Voo";

            if (string.IsNullOrWhiteSpace(BancoOTF.RetornoDados(sql, conexao, parametro)))
            {
                identificador = 0;
            }
            else
            {
                identificador = Convert.ToInt32(BancoOTF.RetornoDados(sql, conexao, parametro));
            }

            if (identificador >= 999)
            {
                Console.Write("\nO número máximo de voo cadastrados pré estabelecidos foram excedidos!");
                return;
            }

            Console.Write("Insira a IATA do destino do voo: ");
            string dest = Console.ReadLine().ToUpper();

            if (!destino.ListaDestinos(dest))
            {
                Console.Write("\nIATA informado não foi cadastrada!");
                return;
            }

            do
            {
                Console.Write("Infome a data e hora do voo como no formato indicado [dd/mm/yyyy] [hh:mm]: ");

                try
                {
                    dataVoo = DateTime.Parse(Console.ReadLine());
                    condicaoDeSaida = false;
                }

                catch (Exception)
                {
                    Console.WriteLine("\nData informada deve seguir o formato indicado: [dd/mm/aaaa] [hh:mm]\n");
                    condicaoDeSaida = true;
                }

                if (dataVoo <= DateTime.Now)
                {
                    if (!condicaoDeSaida)
                    {
                        Console.WriteLine("\nData e hora do voo não pode ser para o dia, nem posterior!\n");
                        condicaoDeSaida = true;
                    }
                }

            } while (condicaoDeSaida);

            idvoo = "V" + (identificador + 1).ToString().PadLeft(4, '0');

            sql = $"INSERT Voo VALUES('{idvoo}', '{destino}', '{dataVoo.ToShortDateString()}', '{dataVoo.Hour}:{dataVoo.Minute}', '{DateTime.Now.ToShortDateString()}', 'ATIVO');";

            if (BancoOTF.InsertDados(sql, conexao))
            {
                parametro = "Capacidade";

                sql = $"SELECT Capacidade FROM Aeronave WHERE INSCRICAO = '{inscricao}'";

                capacidade = Convert.ToInt32(BancoOTF.RetornoDados(sql, conexao, parametro));

                sql = $"INSERT AeronavePossueVoo(INSCRICAO, IDVOO, Capacidade, AssentosOcupados, Passagem) VALUES('{inscricao}', '{idvoo}', '{capacidade}', 0, 'LIBERADA');";

                if (BancoOTF.InsertDados(sql, conexao))
                {
                    sql = $"UPDATE CompanhiaAerea SET UltimoVoo = '{DateTime.Now.ToShortDateString()}' WHERE CNPJ = '{cnpj}'";

                    if (BancoOTF.UpdateDados(sql, conexao))
                    {
                        Console.Write("\nCadastrado com sucesso!");
                        return;
                    }

                    Console.Write("\nErro ao realizar cadastro!");
                }
            }

            Console.Write("\nErro ao realizar cadastro!");
        }
        public static void EditarVoo()
        {
            SqlConnection conexao = new(BancoOTF.CaminhoConexao());
            Destinos destino = new();
            string sql, inscricao, parametro, retorno, idvoo;
            DateTime data = new();
            bool validacao;
            int op = 0, capacidade, ocupados;

            Console.Clear();

            Console.WriteLine("PAINEL DE EDIÇÃO\n");

            Console.Write("Informe o ID do voo que deseja editar: ");

            idvoo = Console.ReadLine();

            sql = $"SELECT * FROM Voo WHERE IDVOO = '{idvoo}';";

            if (!BancoOTF.LocalizarDados(sql, conexao))
            {
                Console.WriteLine("\nVoo não cadastrado!");
                return;
            }

            Console.Clear();

            Console.WriteLine("                                               <<<<MENU DE EDIÇÃO>>>>                                \n");

            Console.WriteLine("Escolha a opção desejada");
            Console.WriteLine("\n1-Editar IATA\n2-Editar inscrição\n3-Editar data e hora do Voo\n4-Editar Situação");
            
            do
            {
                Console.Write("\nOpção: ");
                try
                {
                    op = int.Parse(Console.ReadLine());
                    validacao = false;
                }
                catch (Exception)
                {
                    Console.Write("\nDados inválidos!\n");
                    validacao = true;
                }

                if (op < 1 || op > 4)
                {
                    if (!validacao)
                    {
                        Console.Write("\nA opção informada é inválida!\n");
                        validacao = true;
                    }
                }
            } while (validacao);

            if (op == 1)
            {
                Console.Write("\nInforme a IATA destino do novo destino: ");
                string dest = Console.ReadLine().ToUpper();

                validacao = destino.ListaDestinos(dest);

                if (!validacao)
                {
                    Console.Write("\nNovo destino não cadastrado!");
                    return;
                }

                sql = $"UPDATE Voo SET Destino = '{dest}' WHERE IDVOO = '{idvoo}';";

                if (BancoOTF.UpdateDados(sql, conexao))
                {
                    Console.Write("\nAlterado com sucesso!");
                    return;
                }

                Console.Write("\nErro de alteração!");
                return;
            }

            if (op == 2)
            {
                Console.Write("\nInsira a inscrição da nova Aeronave: ");
                inscricao = Console.ReadLine().ToUpper();

                sql = $"SELECT * FROM Aeronave WHERE INSCRICAO = '{inscricao}'";

                if (!BancoOTF.LocalizarDados(sql, conexao))
                {
                    Console.Write("\nAeronave informada não possue cadastro!");
                    return;
                }

                parametro = "Situacao";

                sql = $"SELECT Situacao FROM Aeronave WHERE INSCRICAO = '{inscricao}'";

                if (BancoOTF.RetornoDados(sql, conexao, parametro) == "INATIVA")
                {
                    Console.Write("\nA solicitação não pode ser realizada! Inscrição inserida é de uma aeronave INATIVADA!");
                    return;
                }

                sql = $"SELECT * FROM AeronavePossueVoo WHERE IDVOO = '{idvoo}'";

                parametro = "INSCRICAO";

                retorno = BancoOTF.RetornoDados(sql, conexao, parametro);

                if (retorno == inscricao)
                {
                    Console.Write("\nIATA informada é a mesma do Voo!");
                    return;
                }

                sql = $"SELECT * FROM AeronavePossueVoo WHERE IDVOO = '{idvoo}' AND INSCRICAO = '{retorno}';";

                parametro = "Capacidade";

                capacidade = Convert.ToInt32(BancoOTF.RetornoDados(sql, conexao, parametro));

                sql = $"SELECT * FROM AeronavePossueVoo WHERE IDVOO = '{idvoo}' AND INSCRICAO = '{retorno}';";

                parametro = "AcentosOcupados";

                ocupados = Convert.ToInt32(BancoOTF.RetornoDados(sql, conexao, parametro));

                sql = $"DELETE FROM AeronavePossueVoo WHERE IDVOO = '{idvoo}' AND INSCRICAO = '{retorno}';";

                BancoOTF.DeleteDados(sql, conexao);

                sql = $"INSERT AeronavePossueVoo(INSCRICAO, IDVOO, Capacidade, AcentosOcupados, Passagem) VALUES('{inscricao}', '{idvoo}', '{capacidade}', '{ocupados}', 'LIBERADA');";

                if (BancoOTF.InsertDados(sql, conexao))
                {
                    Console.Write("\nAlterado com sucesso!");
                    return;
                }

                Console.Write("\nErro de alteração!");
                return;
            }

            if (op == 3)
            {
                do
                {
                    Console.Write("\nInforme a data e hora do voo [dd/mm/yyyy] [hh:mm]: ");
                    try
                    {
                        data = DateTime.Parse(Console.ReadLine());
                        validacao = false;
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("\nFormato inválido! Use o formato inválido [dd/mm/yyyy hh:mm]\n");
                        validacao = true;
                    }

                    if (data < DateTime.Now)
                    {
                        if (!validacao)
                        {
                            Console.WriteLine("\nData DEVE ser futura!");
                        }
                    }
                } while (validacao);

                sql = $"UPDATE Voo SET DataVoo = '{data.ToShortDateString()}', HoraVoo = '{data.Hour}:{data.Minute}' WHERE IDVOO = '{idvoo}';";

                ;

                if (BancoOTF.UpdateDados(sql, conexao))
                {
                    Console.Write("\nAlterado com sucesso!");
                    return;
                }

                Console.Write("\nErro de alteração!");
                return;
            }

            if (op == 4)
            {
                sql = $"SELECT Situacao FROM Voo WHERE IDVOO = '{idvoo}'";

                parametro = "Situacao";
                retorno = BancoOTF.RetornoDados(sql, conexao, parametro);

                if (retorno == "ATIVO")
                {
                    Console.WriteLine("\nSituação deste Voo está atualmente ATIVO!\nDeseja alterar a situação desta Companhia para INATIVO?");
                    Console.Write("\n1-Sim\n2-Não\n\n");
                    do
                    {
                        Console.Write("Opção: ");
                        try
                        {
                            op = int.Parse(Console.ReadLine());
                            validacao = false;
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("\nInválido!\n");
                            validacao = true;
                        }
                        if (op < 1 || op > 2)
                        {
                            if (!validacao)
                            {
                                Console.WriteLine("\nOpção inválida!\n");
                            }
                        }
                    } while (validacao);


                    if (op == 1)
                    {
                        sql = $"UPDATE Voo SET Situacao = 'INATIVO' WHERE IDVOO = '{idvoo}'";

                        if (BancoOTF.UpdateDados(sql, conexao))
                        {
                            Console.Write("\nAlterado com sucesso!");
                            return;
                        }

                        Console.Write("\nErro de alteração!");
                        return;
                    }

                    Console.Write("\nAté logo!");
                    return;
                }

                Console.WriteLine("\nSituação deste Voo está atualmente INATIVO!\nDeseja alterar a situação deste Voo para ATIVO?");
                Console.Write("\n1-Sim\n2-Não\n\nOpção: ");
                op = int.Parse(Console.ReadLine());

                if (op == 1)
                {
                    sql = $"UPDATE Voo SET Situacao = 'ATIVO' WHERE IDVOO = '{idvoo}'";

                    if (BancoOTF.UpdateDados(sql, conexao))
                    {
                        Console.Write("\nAlterado com sucesso!");
                        return;
                    }

                    Console.Write("\nErro de alteração!");
                    return;
                }

                Console.Write("\nAté logo!");
                return;
            }
            Console.Write("\nAté logo!");
        }
        public static void ImprimirVoo()
        {
            SqlConnection conexao = new(BancoOTF.CaminhoConexao());
            SqlCommand cmd;
            int opcao = 0;
            bool validacao;
            string sql, idvoo;

            Console.Clear();

            Console.WriteLine("PAINEL DE IMPRESSÃO");
            Console.WriteLine("\nEscolha a opção desejada:\n1-Ver todas Passagens cadastradas\n2-Ver uma especifica\n0-Voltar ao menu anterior");
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
                        Console.Write("\nA opção informada é inválida!\n");
                        validacao = true;
                    }
                }

            } while (validacao);

            if (opcao == 1)
            {
                conexao.Open();

                cmd = new("SELECT aeronavepossuevoo.INSCRICAO, aeronavepossuevoo.IDVOO, voo.Destino, voo.DataVoo, " +
                          "voo.HoraVoo, voo.DataCadastro, aeronavepossuevoo.AcentosOcupados, voo.Situacao FROM AeronavePossueVoo " +
                          "JOIN Voo ON voo.IDVOO = aeronavepossuevoo.IDVOO WHERE voo.Situacao = 'ATIVO'", conexao);


                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    Console.Clear();

                    while (reader.Read())
                    {
                        Console.WriteLine("Voo\n");
                        Console.WriteLine($"INSCRIÇÃO AERONAVE: {reader.GetString(0)}");
                        Console.WriteLine($"IDVOO: {reader.GetString(1)}");
                        Console.WriteLine($"DESTINO: {reader.GetString(2)}");
                        Console.WriteLine($"DATA VOO: {reader.GetDateTime(3).ToShortDateString()}");
                        Console.WriteLine($"HORA VOO: {reader.GetTimeSpan(4)}");
                        Console.WriteLine($"DATA CADASTRO: {reader.GetDateTime(5).ToShortDateString()}");
                        Console.WriteLine($"ACENTOS OCUPADOS: {reader.GetInt32(6)}");
                        Console.WriteLine($"SITUACAO: {reader.GetString(7)}\n");
                    }
                }

                Console.Write("Pressione [enter] para continuar!");
                conexao.Close();
                return;
            }

            if (opcao == 0)
            {
                return;
            }

            Console.Clear();


            Console.Write("Informe o ID do Voo que deseja localizar: ");
            idvoo = Console.ReadLine().ToUpper();

            sql = $"SELECT * FROM Voo WHERE IDVOO = '{idvoo}';";

            if (!BancoOTF.LocalizarDados(sql, conexao))
            {
                Console.Write("\nVoo não cadastrado!");
                return;
            }

            conexao.Open();

            cmd = new("SELECT aeronavepossuevoo.INSCRICAO, aeronavepossuevoo.IDVOO, voo.Destino, voo.DataVoo, " +
                      "voo.HoraVoo, voo.DataCadastro, aeronavepossuevoo.AcentosOcupados, voo.Situacao FROM AeronavePossueVoo " +
                      "JOIN Voo ON " +
                      $"voo.IDVOO = aeronavepossuevoo.IDVOO WHERE voo.IDVOO = '{idvoo}'", conexao);

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                Console.Clear();

                while (reader.Read())
                {
                    Console.WriteLine("Voo\n");
                    Console.WriteLine($"INSCRIÇÃO AERONAVE: {reader.GetString(0)}");
                    Console.WriteLine($"IDVOO: {reader.GetString(1)}");
                    Console.WriteLine($"DESTINO: {reader.GetString(2)}");
                    Console.WriteLine($"DATA VOO: {reader.GetDateTime(3).ToShortDateString()}");
                    Console.WriteLine($"HORA VOO: {reader.GetTimeSpan(4)}");
                    Console.WriteLine($"DATA CADASTRO: {reader.GetDateTime(5).ToShortDateString()}");
                    Console.WriteLine($"ACENTOS OCUPADOS: {reader.GetInt32(6)}");
                    Console.WriteLine($"SITUACAO: {reader.GetString(7)}\n");
                }
            }

            Console.Write("Pressione [enter] para continuar!");
            conexao.Close();
            return;
        }
        public static void PrincipalVoo()
        {
            int opcao = 0;
            bool condicaoDeParada;

            do
            {
                Console.Clear();

                Console.WriteLine("                                                              <<<<MENU DE VOO>>>>                                   \n");

                Console.WriteLine("1-Cadastrar Voo\n2-Editar Voo\n3-Imprimir Voo\n0-Retornar ao menu anterior\n\nOpção: ");

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
                        Console.Write("\nA opção informada é inválida!\n");
                        Console.ReadKey();
                    }
                }
               

                switch (opcao)
                {
                    case 1:
                        CadastrarVoo();
                        Console.ReadKey();
                        break;


                    case 2:
                        EditarVoo();
                        Console.ReadKey();
                        break;

                    case 3:
                        ImprimirVoo();
                        Console.ReadKey();
                        break;

                    case 0:
                        Console.WriteLine("Finalizando...");
                        break;
                }

            } while (opcao != 0);
        }
    }
}
