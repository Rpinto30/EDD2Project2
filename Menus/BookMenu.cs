using Proyecto2EDD2.Estructuras;
using System;
using System.Collections.Generic;
using System.Text;
using Universal;

namespace Proyecto2EDD2.Menus
{
    public class MenuBook
    {

        static string SolCenter()
        {
            string[] centers = Universal.Universal.graph.GetNames();
            if (centers.Length == 0)
            {
                Console.WriteLine("No hay sedes registradas, antes de registrar un libro debes registrar una sede");
                return string.Empty;
            }
            int center = -1;
            do
            {
                for (int i = 1; i <= centers.Length; i++)
                {
                    if (centers[i - 1] == null || centers[i - 1] == string.Empty) break;
                    Console.WriteLine($"{i}) {centers[i - 1]}");
                }

                Console.Write("> Ingresa el ID de la sede del libro: ");
                if (!int.TryParse(Console.ReadLine(), out center))
                {
                    center = -1;
                }
            } while (center < 1 || center > centers.Length);
            return centers[center - 1];
        }

        public static void CreateBook()
        {
            
            int id = Universal.Universal.table.GetAutoIncrement();


            Console.Write("> Ingresa el TITULO del libro: ");
            string tittle = Console.ReadLine() ?? "";
            Console.Write("> Ingresa el NOMBRE DEL AUTOR del libro: ");
            string autor = Console.ReadLine() ?? "";

            string center = SolCenter();

            if (center == string.Empty) return;

            Console.Clear();
            Book new_book = new Book(id, tittle.ToLower(), autor, center);
            Universal.Universal.table.Insert(new_book);
            Universal.Universal.trie.Insert(tittle.ToLower());
            Console.WriteLine($"El libro {tittle} ha sido registrado");
        }

        static Book? SearchIDBook()
        {
            Console.Write("> Ingresa el ID del libro: ");
            int id;
            if (!int.TryParse(Console.ReadLine(), out id))
            {
                id = -1;
            }
            Console.Clear();
            Book? book = Universal.Universal.table.SearchById(id);
            return book;
        }

        static Book? SearchNameBook()
        {
            Console.Write("> Ingresa el TITULO del libro: ");
            string tittle = Console.ReadLine() ?? "";
            string[] coincidences = Universal.Universal.trie.GetCoincidences(tittle);

            Console.Clear();
            if (coincidences.Length > 0)
            {
                int id = -1;
                do
                {
                    for (int i = 0; i < coincidences.Length; i++)
                    {
                        Console.WriteLine($"\t{i}) {coincidences[i]}");
                    }
                    Console.Write("> Ingresa el numero de coincidencia del libro (-1 para ninguno): ");
                    if (!int.TryParse(Console.ReadLine(), out id))
                    {
                        id = -2;
                    }

                } while (id+1 < -1 || id+1 > coincidences.Length);

                Console.Clear();

                if (id != -1)
                {
                    Book? book = Universal.Universal.table.SearchByName(coincidences[id]);

                    return book;
                }

            }
            else
            {
                Console.WriteLine($"No hay ninguna coincidencia con: {tittle}");
                return null;
            }
            return null;
        }


        static void PrintBook(Book? book)
        {
            if (book != null)
            {
                Console.WriteLine($"===== ID: {book.Id} | Titulo: {book.Title} =====");
                Console.WriteLine($"      Autor: {book.Author} | Sede: {book.Center}");
            }
            else
            {
                Console.WriteLine($"No encontramos ese libro...");
            }
        }

        static void ShowBookByID()
        {
            Book? book = SearchIDBook();
            PrintBook(book);
        }

        static void ShowBookByName()
        {
            Book? book = SearchNameBook();
            if (book != null)
            PrintBook(book);
        }

        static void ShowTable()
        {
            Universal.Universal.table.ShowTable();
        }


        static void TranslateProcess(Book? book)
        {
            if (book != null)
            {
                string bookCenter = book.Center;
                if (bookCenter == "N/A")
                {
                    Console.Write("Advertencia: El libro no tiene una sede actual, deseas agregar una? (Y/n): ");
                    string y = Console.ReadLine() ?? "n";
                    if (y == "Y")
                    {
                        string a = SolCenter();
                        Console.Clear();
                        if (a == string.Empty) return;

                        bookCenter = a;
                        Book update = new Book(
                            book.Id, book.Title, book.Author, a
                            );
                        Universal.Universal.table.UpdateBookByID(book.Id, update);
                        Console.WriteLine($"El libro \"{book.Title}\" ha sido re-asignado a: {a}");
                        Console.Write("Deseas continuar con el proceso de traslación? (Y/n): ");
                        string y2 = Console.ReadLine() ?? "n";
                        if (y2 != "Y") return;
                    }
                }

                Console.WriteLine($"============== SELECCIONA NUEVA SEDE PARA \"{book.Title}\" (SEDE ACTUAL: {bookCenter}) ==============");
                string b = SolCenter();
                Console.Clear();
                if (b == string.Empty) return;
                else if (b == bookCenter)
                {
                    Console.WriteLine("No puedes trasladar un libro a su sede actual");
                    return;
                }

                RouteResult? r = CenterMenu.CalcBoth(bookCenter, b);
                
                if (r != null)
                {
                    CenterMenu.PrintRoute(r);

                    Console.WriteLine();

                    Console.Write("Deseas confirmar el traslado (considera el costo y tiempo) (Y/n): ");
                    string op = Console.ReadLine() ?? "n";
                    Console.Clear();

                    if (op == "Y")
                    {
                        Book update = new Book(
                            book.Id, book.Title, book.Author, b
                            );
                        Universal.Universal.table.UpdateBookByID(book.Id, update);
                        Console.WriteLine($"El libro \"{book.Title}\" ha sido trasladado: {bookCenter} -> {b}");
                    }
                }
                else
                {
                    Console.WriteLine("Lo sentimos, no existe ninguna ruta disponible que conecte ambas redes de distribución");
                    Console.WriteLine("Parece que no se pudo completar el proceso de traslación...");
                }
            }
            else
            {
                Console.WriteLine("No podemos encontrar ese libro...");
            }
        }

        static void TranslateBook()
        {
            string op = string.Empty;
            Book? book = null;
            do
            {
                Console.WriteLine("============== TRASLADO DE LIBROS ==============");
                Console.WriteLine("1) Buscar libro por ID\n" +
                                "2) Buscar libro por nombre\n" +
                                "3) Cancelar");
                Console.Write("Ingresa una opción para buscar el libro: ");
                op = Console.ReadLine() ?? "";
                Console.Clear();
                switch (op)
                {
                    case "1":
                        Console.WriteLine("============== TRASLADO DE LIBROS ==============");
                        book = SearchIDBook();
                        TranslateProcess(book);
                        return;
                    case "2":
                        Console.WriteLine("============== TRASLADO DE LIBROS ==============");
                        book = SearchNameBook();
                        TranslateProcess(book);
                        return;
                    case "3":
                        return;
                }
            } while (op != "2" || op != "1");
        }


        static void RemoveBook(Book? book)
        {
            Console.Write($"> Estas seguro en eliminar el libro: {book.Title}? (Y/n): ");
            string y = Console.ReadLine() ?? "n";
            Console.Clear();
            if (y == "Y")
            {

                if (book != null)
                {
                    Universal.Universal.table.Remove(book.Id);
                    Universal.Universal.trie.Remove(book.Title);
                    Console.WriteLine("El libro ha sido removido del registro");
                }
                else
                {
                    Console.WriteLine("No se ha encontrado ese libro");
                }
            }
        }

        static void DeleteBook()
        {
            string op = string.Empty;
            Book? book = null;
            do
            {
                Console.WriteLine("============== REMOVER LIBRO ==============");
                Console.WriteLine("1) Buscar libro por ID\n" +
                                "2) Buscar libro por nombre\n" +
                                "3) Cancelar");
                Console.Write("Ingresa una opción para buscar el libro: ");
                op = Console.ReadLine() ?? "";
                Console.Clear();
                switch (op)
                {
                    case "1":
                        Console.WriteLine("============== REMOVER LIBRO ==============");
                        book = SearchIDBook();
                        RemoveBook(book);

                        return;
                    case "2":
                        Console.WriteLine("============== REMOVER LIBRO ==============");
                        book = SearchNameBook();
                        RemoveBook(book);
                        return;
                    case "3":
                        return;
                }
            } while (op != "2" || op != "1");
        }

        public static void SetMenuBooks()
        {
            string opBook = string.Empty;

            do
            {
                Console.WriteLine("============== GESTION DE LIBROS ==============");
                Console.WriteLine("1) Registrar un libro\n" +
                                    "2) Buscar libro por ID\n" +
                                    "3) Buscar libro por nombre\n" +
                                    "4) Ver todo el registro\n" +
                                    "5) Eliminar registro de un libro\n" +
                                    "6) Trasladar libro\n" +
                                    "7) Salir");
                Console.Write("> Ingresa una opción: ");
                opBook = Console.ReadLine() ?? "";
                Console.Clear();
                switch (opBook)
                {
                    case "1":
                        CreateBook();
                        break;
                    case "2":
                        ShowBookByID();
                        break;
                    case "3":
                        ShowBookByName();
                        break;
                    case "4":
                        ShowTable();
                        break;
                    case "5":
                        DeleteBook();
                        break;
                    case "6":
                        TranslateBook();
                        break;
                    case "7": break;
                    default:
                        Console.WriteLine("La opción seleccionada no es valida, vuelve a intentarlo");
                        Universal.Universal.WaitEntry();
                        break;
                }
                Console.WriteLine();
            } while (opBook != "7");
        }
    }
}