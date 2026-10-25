using Proyecto2EDD2.Estructuras;
using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using System.Xml.Linq;
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
            Console.Clear();
            try
            {
                Universal.Universal.graph.InsertNode(name);
                Console.WriteLine($"La sede {name} fue agregada");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocurrio un error... \n{ex.Message}");
            }

            foreach(var i in Universal.Universal.graph.GetNames())
            {
                Console.WriteLine(i);
            }
        }

        static void ConnectCenter()
        {
            Console.Write("> Ingresa el nombre de la sede de origen: ");
            string nameA = Console.ReadLine() ?? "";
            Console.Write("> Ingresa el nombre de la sede de destino: ");
            string nameB = Console.ReadLine() ?? "";

            Console.Write("> Ingresa el TIMEPO de envio en minutos: ");
            double time = double.Parse(Console.ReadLine() ?? "0.00");

            Console.Write("> Ingresa el COSTO de envio: ");
            double cost = double.Parse(Console.ReadLine() ?? "0.00");

            Console.Write("> Deseas que la conexión sea en ambos sentidos? (Y/n): ");
            string bidi = Console.ReadLine() ?? "n";

            CenterEdge conn = new CenterEdge(time, cost, (bidi == "Y")? 1: 0);

            Console.Clear();
            try
            {
                Universal.Universal.graph.InsertConnection(nameA, nameB, conn);
                Console.WriteLine($"Conexion {nameA} <-> {nameB} creada [Tiempo: {time} | Costo: {cost}]");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocurrio un error... \n{ex.Message}");
            }
        }

        public static RouteResult? CalcFast()
        {
            Console.Write("> Ingresa el nombre de la sede de origen: ");
            string nameA = Console.ReadLine() ?? "";
            Console.Write("> Ingresa el nombre de la sede de destino: ");
            string nameB = Console.ReadLine() ?? "";

            Console.Clear();
            try
            {
                RouteResult? r = Universal.Universal.graph.DijkstraTime(nameA, nameB);
                return r;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocurrio un error... \n{ex.Message}");
                return null;
            }
        }

        static RouteResult? CalcEco()
        {
            Console.Write("> Ingresa el nombre de la sede de origen: ");
            string nameA = Console.ReadLine() ?? "";
            Console.Write("> Ingresa el nombre de la sede de destino: ");
            string nameB = Console.ReadLine() ?? "";

            Console.Clear();

            try
            {
                RouteResult? r = Universal.Universal.graph.DijkstraTime(nameA, nameB);
                return r;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocurrio un error... \n{ex.Message}");
                return null;
            }
        }

        public static RouteResult? CalcBoth(string nameA, string nameB)
        {
            try
            {
                RouteResult? r = Universal.Universal.graph.Dijkstra(nameA, nameB);
                return r;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocurrio un error... \n{ex.Message}");
                return null;
            }
        }

        public static RouteResult? CalcBoth()
        {
            Console.Write("> Ingresa el nombre de la sede de origen: ");
            string nameA = Console.ReadLine() ?? "";
            Console.Write("> Ingresa el nombre de la sede de destino: ");
            string nameB = Console.ReadLine() ?? "";

            Console.Clear();
            try
            {
                RouteResult? r = Universal.Universal.graph.Dijkstra(nameA, nameB);
                return r;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocurrio un error... \n{ex.Message}");
                return null;
            }
        }


        public static void PrintRoute(RouteResult? r)
        {
            if (r != null)
                Console.WriteLine(r.ToString());
            else
                Console.WriteLine($"No se encontró esa ruta");
        }

        static void ShowRouteByTime()
        {
            RouteResult? r = CalcFast();
            PrintRoute(r);
        }

        static void ShowRouteByEco()
        {
            RouteResult? r = CalcEco();
            PrintRoute(r);
        }

        static void ShowRouteByBoth()
        {
            RouteResult? r = CalcBoth();
            PrintRoute(r);
        }

        static void ShowDFS()
        {
            Universal.Universal.graph.Dfs();
        }

        static void ShowBFS()
        {
            Universal.Universal.graph.Bfs();
        }

        static void SameRed()
        {
            Console.Write("> Ingresa el nombre de la sede de origen: ");
            string nameA = Console.ReadLine() ?? "";
            Console.Write("> Ingresa el nombre de la sede de destino: ");
            string nameB = Console.ReadLine() ?? "";

            if (Universal.Universal.graph.IsSameRed(nameA, nameB))
            {
                Console.WriteLine($"La sede {nameA} y la sede {nameB} ya se encuentran conectadas");
                return;
            }

            string op = string.Empty;
            Console.WriteLine($"La sede {nameA} y la sede {nameB} no se encuentran conectadas aún");
            Console.Write($"Deseas conectarlas? (Y/n): ");
            op = Console.ReadLine() ?? "n";
            if (op == "Y")
            {
                Console.Write("> Ingresa el TIMEPO de envio en minutos: ");
                double time = double.Parse(Console.ReadLine() ?? "0.00");

                Console.Write("> Ingresa el COSTO de envio: ");
                double cost = double.Parse(Console.ReadLine() ?? "0.00");

                Console.Write("> Deseas que la conexión sea en ambos sentidos? (Y/n): ");
                string bidi = Console.ReadLine() ?? "n";

                CenterEdge conn = new CenterEdge(time, cost, (bidi == "Y") ? 1 : 0);

                Universal.Universal.graph.InsertConnection(nameA, nameB, conn);
            }
            
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
                                    "9) Comprobar que dos sedes se encuentran en la misma red de distribución\n" +
                                    "10) Salir");
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
                        ShowRouteByTime();
                        break;
                    case "4":
                        ShowRouteByEco();
                        break;
                    case "5":
                        ShowRouteByBoth();
                        break;
                    case "6":
                        ShowBFS();
                        break;
                    case "7":
                        ShowDFS();
                        break;
                    case "8":
                        Universal.Universal.graph.ShowMatrix();
                        break;
                    case "9": 
                        SameRed();
                        break;
                    case "10": break;
                    default:
                        Console.WriteLine("La opción seleccionada no es valida, vuelve a intentarlo");
                        break;
                }
                Console.WriteLine();
            } while (opBook != "10");
        }
    }
    
}
