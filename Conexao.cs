using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PProjetoOnTheFly_Banco
{
    internal class BancoOTF
    {
        
            public static string CaminhoConexao()
            {
                return @"Data Source=localhost; Initial Catalog=AeroOnTheFly; User ID=sa; password=scoobypolly;";
            }
            

            public static bool LocalizarDados(string sql, SqlConnection conexao)
            {
                BancoOTF caminho = new();
                conexao = new(CaminhoConexao());
                conexao.Open();

                SqlCommand cmd = new(sql, conexao);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            return true;
                        }
                    }
                }

                conexao.Close();

                return false;

            }
            public static string RetornoDados(string sql, SqlConnection conexao, string parametro)
            {
                var situacao = "";
                BancoOTF caminho = new();
                conexao = new(CaminhoConexao());
                conexao.Open();

                SqlCommand cmd = new(sql, conexao);
                cmd.CommandType = CommandType.Text;

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            situacao = reader[$"{parametro}"].ToString();
                        }
                    }
                }

                conexao.Close();

                return situacao;
            }
            public static bool InsertDados(string sql, SqlConnection conexao)
            {
                BancoOTF caminho = new();
                conexao = new(CaminhoConexao());
                conexao.Open();

                SqlCommand cmd = new(sql, conexao);
                try
                {
                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        return true;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine();
                    Console.WriteLine(e.Message);
                }

                conexao.Close();

                return false;
            }
            public static bool UpdateDados(string sql, SqlConnection conexao)
            {
                int contador;
                BancoOTF caminho = new();
                conexao = new(CaminhoConexao());
                conexao.Open();

                SqlCommand cmd = new(sql, conexao);

                contador = cmd.ExecuteNonQuery();
                conexao.Close();

                if (contador > 0)
                {
                    return true;
                }

                return false;
            }
            public static bool DeleteDados(string sql, SqlConnection conexao)
            {
                int contador = 0;
                BancoOTF caminho = new();
                conexao = new(CaminhoConexao());
                conexao.Open();

                SqlCommand cmd = new(sql, conexao);
                try
                {
                    contador = cmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    Console.WriteLine();
                    Console.WriteLine(e.Message);
                }

                conexao.Close();

                if (contador > 0)
                {
                    return true;
                }

                return false;
            }

        

    }
}