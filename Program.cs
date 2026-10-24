using Proyecto2EDD2.Estructuras;
using Proyecto2EDD2.Files;
using Universal;
using Proyecto2EDD2.Menus;

string op = string.Empty;

do
{
    Console.WriteLine("============== CATALOGO DE LIBROS, BIENVENIDO ==============");
    Console.WriteLine("1) Gestionar libros\n" +
                      "2) Gestionar rutas\n" +
                      "3) Salir");
    Console.Write("> Ingresa una opción: ");
    op = Console.ReadLine()??"";
    Console.Clear();
    switch (op)
    {
        case "1":
            MenuBook.SetMenuBooks();
            break;
        case "2":
            CenterMenu.SetMenuCenter();
            break;
        case "3":
            Console.WriteLine("Adios");
            break;
        default:
            Console.WriteLine("La opción seleccionada no es valida, vuelve a intentarlo");
            Universal.Universal.WaitEntry();
            break;
    }
} while (op != "3");