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

Universal.Universal.graph.InsertNode("M");
Universal.Universal.graph.InsertNode("N");


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



Universal.Universal.graph.InsertConnection("M", "N", new CenterEdge(3, 1, 1));

Universal.Universal.table.Insert(new Book(1, "El quijote".ToLower(), "a", "A"));
Universal.Universal.trie.Insert("El quijote".ToLower());
Universal.Universal.table.Insert(new Book(2, "la metamorfosis".ToLower(), "b", "C"));
Universal.Universal.trie.Insert("la metamorfosis".ToLower());
Universal.Universal.table.Insert(new Book(3, "LIBRO C".ToLower(), "c", "C"));
Universal.Universal.trie.Insert("LIBRO C".ToLower());
Universal.Universal.table.Insert(new Book(4, "harry potter".ToLower(), "d", "B"));
Universal.Universal.trie.Insert("harry potter".ToLower());
Universal.Universal.table.Insert(new Book(5, "el principito".ToLower(), "e", "D"));
Universal.Universal.trie.Insert("rl principito".ToLower());
Universal.Universal.table.Insert(new Book(6, "el extranjero".ToLower(), "f", "F"));
Universal.Universal.trie.Insert("el extranjero".ToLower());
Universal.Universal.table.Insert(new Book(7, "clean code".ToLower(), "g", "F"));
Universal.Universal.trie.Insert("clean code".ToLower());
Universal.Universal.table.Insert(new Book(8, "el hombre de ojalata".ToLower(), "h", "A"));
Universal.Universal.trie.Insert("el hombre de ojalata".ToLower());
Universal.Universal.table.Insert(new Book(9, "LIBRO I".ToLower(), "i", "G"));
Universal.Universal.trie.Insert("LIBRO I".ToLower());
Universal.Universal.table.Insert(new Book(10, "el mago de oz".ToLower(), "j", "A"));
Universal.Universal.trie.Insert("el mago de oz".ToLower());
Universal.Universal.table.Insert(new Book(11, "LIBRO K".ToLower(), "k", "I"));
Universal.Universal.trie.Insert("LIBRO k".ToLower());

//===================MENU
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