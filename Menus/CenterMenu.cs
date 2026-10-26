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
                Universal.Universal.graph.InsertNode(
                    Universal.Universal.RemoverTildes(name)
                    );
                Universal.Universal.trieCentres.Insert(
                    Universal.Universal.RemoverTildes(name)
                    );
                Console.WriteLine($"La sede {name} fue agregada");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocurrio un error... \n{ex.Message}");
            }
        }


        static string SearchNameCenter(string tittle)
        {
            string[] coincidences = Universal.Universal.trieCentres.GetCoincidences(tittle);

            Console.Clear();
            if (coincidences.Length > 0)
            {
                int id = -1;
                do
                {
                    Console.WriteLine("============== COINCIDENCIAS ENCONTRADAS ==============");
                    for (int i = 0; i < coincidences.Length; i++)
                    {
                        Console.WriteLine($"\t{i}) {coincidences[i]}");
                    }
                    Console.Write("> Ingresa el numero de coincidencia del libro (-1 para ninguno): ");
                    if (!int.TryParse(Console.ReadLine(), out id))
                    {
                        id = -2;
                    }

                } while (id + 1 < -1 || id + 1 > coincidences.Length);

                Console.Clear();

                if (id != -1)
                {
                    //Book? book = Universal.Universal.table.(coincidences[id]);
                    return coincidences[id];
                    //return book;
                }

            }
            else
            {
                Console.WriteLine($"No hay ninguna coincidencia con: {tittle}");
                return string.Empty;
            }
            return string.Empty;
        }

        static void ConnectCenter()
        {
            Console.Write("> Ingresa el nombre de la sede de origen: ");
            string nameA = Console.ReadLine() ?? "";

            if (!Universal.Universal.graph.InGraph(nameA))
            {
                nameA = SearchNameCenter(nameA);
                if (nameA == string.Empty)
                {
                    Console.WriteLine("Sin origen no se puede completar el proceso");
                    return;
                } 
            }

            Console.Write("> Ingresa el nombre de la sede de destino: ");
            string nameB = Console.ReadLine() ?? "";

            if (!Universal.Universal.graph.InGraph(nameB))
            {
                nameB = SearchNameCenter(nameB);
                if (nameB == string.Empty)
                {
                    Console.WriteLine("Sin destino no se puede completar el proceso");
                    return;
                }
            }

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

        static RouteResult? CalcFast()
        {
            Console.Write("> Ingresa el nombre de la sede de origen: ");
            string nameA = Console.ReadLine() ?? "";
            if (!Universal.Universal.graph.InGraph(nameA))
            {
                nameA = SearchNameCenter(nameA);
                if (nameA == string.Empty)
                {
                    Console.WriteLine("Sin origen no se puede completar el proceso");
                    return null;
                }
            }

            Console.Write("> Ingresa el nombre de la sede de destino: ");
            string nameB = Console.ReadLine() ?? "";

            if (!Universal.Universal.graph.InGraph(nameB))
            {
                nameB = SearchNameCenter(nameB);
                if (nameB == string.Empty)
                {
                    Console.WriteLine("Sin origen no se puede completar el proceso");
                    return null;
                }
            }

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
            if (!Universal.Universal.graph.InGraph(nameA))
            {
                nameA = SearchNameCenter(nameA);
                if (nameA == string.Empty)
                {
                    Console.WriteLine("Sin origen no se puede completar el proceso");
                    return null;
                }
            }

            Console.Write("> Ingresa el nombre de la sede de destino: ");
            string nameB = Console.ReadLine() ?? "";

            if (!Universal.Universal.graph.InGraph(nameB))
            {
                nameB = SearchNameCenter(nameB);
                if (nameB == string.Empty)
                {
                    Console.WriteLine("Sin origen no se puede completar el proceso");
                    return null;
                }
            }

            Console.Clear();

            try
            {
                RouteResult? r = Universal.Universal.graph.DijkstraCost(nameA, nameB);
                return r;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocurrio un error... \n{ex.Message}");
                return null;
            }
        }

        public static RouteResult? CalcFast(string nameA, string nameB)
        {
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

        public static RouteResult? CalcEco(string nameA, string nameB)
        {  
            try
            {
                RouteResult? r = Universal.Universal.graph.DijkstraCost(nameA, nameB);
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
            if (!Universal.Universal.graph.InGraph(nameA))
            {
                nameA = SearchNameCenter(nameA);
                if (nameA == string.Empty)
                {
                    Console.WriteLine("Sin origen no se puede completar el proceso");
                    return null;
                }
            }

            Console.Write("> Ingresa el nombre de la sede de destino: ");
            string nameB = Console.ReadLine() ?? "";

            if (!Universal.Universal.graph.InGraph(nameB))
            {
                nameB = SearchNameCenter(nameB);
                if (nameB == string.Empty)
                {
                    Console.WriteLine("Sin origen no se puede completar el proceso");
                    return null;
                }
            }

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
            {
                Console.WriteLine("============================");
                Console.WriteLine(r.ToString());
            }
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
            if (!Universal.Universal.graph.InGraph(nameA))
            {
                nameA = SearchNameCenter(nameA);
                if (nameA == string.Empty)
                {
                    Console.WriteLine("Sin origen no se puede completar el proceso");
                    return;
                }
            }

            Console.Write("> Ingresa el nombre de la sede de destino: ");
            string nameB = Console.ReadLine() ?? "";

            if (!Universal.Universal.graph.InGraph(nameB))
            {
                nameB = SearchNameCenter(nameB);
                if (nameB == string.Empty)
                {
                    Console.WriteLine("Sin origen no se puede completar el proceso");
                    return;
                }
            }
            Console.Clear();
            try
            {
                if (Universal.Universal.graph.IsSameRed(nameA, nameB))
                {
                    Console.WriteLine($"La sede {nameA} y la sede {nameB} ya se encuentran conectadas");
                    return;
                }
            } catch(Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
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
                Console.WriteLine($"La sede {nameA} y la sede {nameB} ya se encuentran conectadas");
            }
            
        }

        static void DeleteCenter()
        {
            Console.Write("> Ingresa el nombre de la sede de origen: ");
            string nameA = Console.ReadLine() ?? "";
            if (!Universal.Universal.graph.InGraph(nameA))
            {
                nameA = SearchNameCenter(nameA);
                if (nameA == string.Empty)
                {
                    Console.WriteLine("Sin origen no se puede completar el proceso");
                    return;
                }
            }


            Console.Write($"> Estas seguro en eliminar la sede: {nameA}? (Y/n): ");
            string y = Console.ReadLine() ?? "n";
            Console.Clear();
            if (y == "Y")
            {
                Universal.Universal.table.DisconnectCenter(nameA);
                Universal.Universal.graph.RemoveNode(nameA);
                Universal.Universal.trieCentres.Remove(nameA);

                Console.WriteLine($"La sede {nameA} fue eliminada");
            }
        }


        static void MenuCenters()
        {
            string opBook = string.Empty;

            do
            {
                Console.WriteLine("============== GESTION DE SEDES ==============");
                Console.WriteLine("1) Registrar una sede\n" +
                                    "2) Conectar sedes\n" +
                                    "3) Ver sedes por amplitud\n" +
                                    "4) Ver sedes por profundidad\n" +
                                    "5) Eliminar sede\n" +
                                    "6) Salir");
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
                        ShowBFS();
                        break;
                    case "4":
                        ShowDFS();
                        break;
                    case "5":
                        DeleteCenter();
                        break;
                    case "6":
                        break;
                    default:
                        Console.WriteLine("La opción seleccionada no es valida, vuelve a intentarlo");
                        break;
                }
                Console.WriteLine();
            } while (opBook != "6");
        }

        static void MenuRoutes()
        {
            string opBook = string.Empty;

            do
            {
                Console.WriteLine("============== GESTION DE RUTAS ==============");
                Console.WriteLine(  "1) Calcular ruta más rápida\n" +
                                    "2) Calcular ruta más económica\n" +
                                    "3) Calcular ruta más rápida y económica\n" +
                                    "4) Ver todas las rutas\n" +
                                    "5) Comprobar que dos sedes se encuentran en la misma red de distribución\n" +
                                    "6) Salir");
                Console.Write("> Ingresa una opción: ");
                opBook = Console.ReadLine() ?? "";
                Console.Clear();
                switch (opBook)
                {
                    case "1":
                        ShowRouteByTime();
                        break;
                    case "2":
                        ShowRouteByEco();
                        break;
                    case "3":
                        ShowRouteByBoth();
                        break;
                    case "4":
                        Universal.Universal.graph.ShowMatrix();
                        break;
                    case "5":
                        SameRed();
                        break;
                    case "6":
                        break;
                    default:
                        Console.WriteLine("La opción seleccionada no es valida, vuelve a intentarlo");
                        break;
                }
                Console.WriteLine();
            } while (opBook != "6");
        }

        public static void SetMenuCenter()
        {
            string opBook = string.Empty;

            do
            {
                Console.WriteLine("============== GESTION DE SEDES Y RUTAS ==============");
                Console.WriteLine("1) Gestionar y ver sedes\n"+
                                    "2) Ver rutas\n"+
                                    "3) Salir");
                Console.Write("> Ingresa una opción: ");
                opBook = Console.ReadLine() ?? "";
                Console.Clear();
                switch (opBook)
                {
                    case "1":
                        MenuCenters();
                        break;
                    case "2":
                        MenuRoutes();
                        break;
                    case "3":
                        break;
                    
                    default:
                        Console.WriteLine("La opción seleccionada no es valida, vuelve a intentarlo");
                        break;
                }
                Console.WriteLine();
            } while (opBook != "3");
        }
    }
    
}
