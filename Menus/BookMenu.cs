using Proyecto2EDD2.Estructuras;
using System;
using System.Collections.Generic;
using System.Text;
using Universal;

namespace Proyecto2EDD2.Menus
{
    public class MenuBook
    {


        public static void SetMenuBooks()
        {
            string opBook = string.Empty;

            do
            {
                Console.WriteLine("============== GESTION DE LIBROS ==============");
                Console.WriteLine("1) Registrar un libro\n" +
                                    "2) Actualizar libro por ID\n" +
                                    "3) Actualizar libro por nombre\n" +
                                    "4) Salir");
                Console.Write("> Ingresa una opción: ");
                opBook = Console.ReadLine() ?? "";
                Console.Clear();
                switch (opBook)
                {
                    case "1":
                        int id = 0;
                        string tittle = string.Empty;

                        string autor = string.Empty;
                        string center = string.Empty;

                        Book new_book = new Book(id, tittle, autor, center);

                        Universal.Universal.table.Insert(new_book);
                        break;
                    case "2":

                        break;
                    case "3":
                        break;
                    case "4": break;
                    default:
                        Console.WriteLine("La opción seleccionada no es valida, vuelve a intentarlo");
                        Universal.Universal.WaitEntry();
                        break;
                }
            } while (opBook != "4");
        }
    }
}