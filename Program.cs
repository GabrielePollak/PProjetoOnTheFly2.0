using System;
using System.Threading;

namespace PProjetoOnTheFly_Banco
{
    internal class Program
    {
        static void Main(string[] args)
        {

            int opcao = 9;
            bool condicaoDeParada;

            do
            {

                Console.Clear();

                Console.WriteLine("                                       <<<<Bem-Vindo ao menu do aeroporto On The Fly!>>>>                          ");

                Console.WriteLine("Informe a opção desejada:\n1-Passageiros\n2-Companhias Aereas\n3-Aeronaves\n4-Voo\n5-Passagens\n6-Vendas\n7-Restritos\n0-Sair\nOpção: ");

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
                    opcao = 9;
                }

                if (opcao < 0 || opcao > 7)
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
                        Passageiros.AcessarPassageiro();
                        break;
                    case 2:
                        Companhia_Aerea.AcessarCompanhia();
                        break;

                    case 3:
                        Aeronave.AcessarAeronave();
                        break;

                    /*case 4:
                        Voo.AcessarVoo();
                        break;

                    /*case 5:
                        Passagem.AcessarPassagem();
                        break;

                    /*case 6:
                        Venda.AcessarVenda();
                        break;*/

                     

                    //case 7:
                       // Restricao.AcessarRestritos();
                       // break;
                }

            } while (opcao < 1 || opcao > 7);
        }





















        /*int opc;

        do
        {
            Console.WriteLine("                                   <<<<<Bem-vindo ao ON THE FLY!>>>>\n\nEscolha a opção desejada: \n1-Cadastrar passageiro\n2-Cadastrar aeronave\n3-Cadastrar companhia aerea\n4-Realizar venda\n5-Sair\nOpção:");
            opc = int.Parse(Console.ReadLine());




                 Passageiros passageiros = new Passageiros();

                    passageiros.CadastrarPassageiros();
                    break;

        }while (true);*/








    }
    
}
