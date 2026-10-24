using Proyecto2EDD2.Estructuras;
using System;
using System.Collections.Generic;
using System.Text;
using Universal;

namespace Proyecto2EDD2.Menus
{
    internal class CenterMenu
    {

        static void CreateCenter()
        {
            Console.Write("> Ingresa el nombre de la sede: ");
            string name = Console.ReadLine() ?? "";
            Center center = new Center(name);
            Universal.Universal.graph.InsertNode(name);
        }

        static void ConnectCenter()
        {
            Console.Write("> Ingresa el nombre de la sede A: ");
            string nameA = Console.ReadLine() ?? "";
            Console.Write("> Ingresa el nombre de la sede B: ");
            string nameB = Console.ReadLine() ?? "";

            Console.Write("> Ingresa el tiempo de envio en minutos de sede A a sede B: ");
            double time = double.Parse(Console.ReadLine() ?? "0.00");

            Console.Write("> Ingresa el costo de envio de seded A a sede B: ");
            double cost = double.Parse(Console.ReadLine() ?? "0.00");


            CenterEdge conn = new CenterEdge(time, cost,1);

            Universal.Universal.graph.InsertConnection(nameA, nameB, conn);
        }

        public static void SetMenuCenter()
        {
            string opBook = string.Empty;

            do
            {
                Console.WriteLine("============== GESTION DE SEDES ==============");
                Console.WriteLine("1) Registrar una sede\n" +
                                    "2) Conectar sedes\n" +
                                    "3) Calcular sede más rápida\n" +
                                    "4) Calcular sede más económica\n" +
                                    "5) Calcular sede más rápida y económica\n" +
                                    "6) Ver sede por amplitud\n" +
                                    "7) Ver sede por profundidad\n" +
                                    "8) Ver conexion entre sedes\n" +
                                    "9) Salir");
                Console.Write("> Ingresa una opción: ");
                opBook = Console.ReadLine() ?? "";
                Console.Clear();
                switch (opBook)
                {
                    case "1":
                        CreateCenter();
                        break;
                    case "2":
                        ConnectCenter();
                        break;
                    case "3":
                        break;
                    case "4": 
                        break;
                    case "5":
                        break;
                    case "6":
                        break;
                    case "7":
                        break;
                    case "8":
                        Universal.Universal.graph.ShowMatrix();
                        break;
                    case "9": break;
                    default:
                        Console.WriteLine("La opción seleccionada no es valida, vuelve a intentarlo");
                        Universal.Universal.WaitEntry();
                        break;
                }
            } while (opBook != "9");
        }
    }
    
}
