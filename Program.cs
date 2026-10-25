using Proyecto2EDD2.Estructuras;
using Proyecto2EDD2.Files;
using Universal;
using Proyecto2EDD2.Menus;


Universal.Universal.graph.InsertNode("A");
Universal.Universal.graph.InsertNode("B");
Universal.Universal.graph.InsertNode("C");
Universal.Universal.graph.InsertNode("D");
Universal.Universal.graph.InsertNode("E");
Universal.Universal.graph.InsertNode("F");
Universal.Universal.graph.InsertNode("G");
Universal.Universal.graph.InsertNode("H");
Universal.Universal.graph.InsertNode("I");

Universal.Universal.graph.InsertConnection("A", "B", new CenterEdge(2, 3, 1));
Universal.Universal.graph.InsertConnection("A", "C", new CenterEdge(2, 2, 1));
Universal.Universal.graph.InsertConnection("B", "D", new CenterEdge(6, 1, 1));
Universal.Universal.graph.InsertConnection("B", "E", new CenterEdge(2, 3, 1));
Universal.Universal.graph.InsertConnection("C", "E", new CenterEdge(2, 3, 1));
Universal.Universal.graph.InsertConnection("C", "F", new CenterEdge(4, 2, 1));
Universal.Universal.graph.InsertConnection("D", "G", new CenterEdge(1, 3, 1));
Universal.Universal.graph.InsertConnection("E", "G", new CenterEdge(2, 1, 1));
Universal.Universal.graph.InsertConnection("E", "H", new CenterEdge(3, 2, 1));
Universal.Universal.graph.InsertConnection("F", "H", new CenterEdge(1, 2, 1));
Universal.Universal.graph.InsertConnection("G", "I", new CenterEdge(3, 3, 1));
Universal.Universal.graph.InsertConnection("H", "I", new CenterEdge(3, 1, 1));


Universal.Universal.table.Insert(new Book(1, "LIBRO A", "a", "A"));
Universal.Universal.trie.Insert("LIBRO A".ToLower());
Universal.Universal.table.Insert(new Book(2, "LIBRO B", "b", "C"));
Universal.Universal.trie.Insert("LIBRO B".ToLower());
Universal.Universal.table.Insert(new Book(3, "LIBRO C", "c", "C"));
Universal.Universal.trie.Insert("LIBRO C".ToLower());
Universal.Universal.table.Insert(new Book(4, "LIBRO D", "d", "B"));
Universal.Universal.trie.Insert("LIBRO D".ToLower());
Universal.Universal.table.Insert(new Book(5, "LIBRO E", "e", "D"));
Universal.Universal.trie.Insert("LIBRO E".ToLower());
Universal.Universal.table.Insert(new Book(6, "LIBRO F", "f", "F"));
Universal.Universal.trie.Insert("LIBRO F".ToLower());
Universal.Universal.table.Insert(new Book(7, "LIBRO G", "g", "F"));
Universal.Universal.trie.Insert("LIBRO G".ToLower());
Universal.Universal.table.Insert(new Book(8, "LIBRO H", "h", "A"));
Universal.Universal.trie.Insert("LIBRO H".ToLower());
Universal.Universal.table.Insert(new Book(9, "LIBRO I", "i", "G"));
Universal.Universal.trie.Insert("LIBRO I".ToLower());
Universal.Universal.table.Insert(new Book(10, "LIBRO J", "j", "A"));
Universal.Universal.trie.Insert("LIBRO J".ToLower());
Universal.Universal.table.Insert(new Book(11, "LIBRO K", "k", "I"));
Universal.Universal.trie.Insert("LIBRO k".ToLower());

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
            break;
    }
} while (op != "3");