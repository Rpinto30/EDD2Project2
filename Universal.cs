using Proyecto2EDD2.Estructuras;
using System;
using System.Collections.Generic;
using System.Text;

namespace Universal
{
    public class Universal
    {
        public static Grafo graph = new Grafo();
        public static TablaHash table = new TablaHash(20);
        public static Trie trie = new Trie();

        public static void WaitEntry()
        {
            Console.WriteLine("Presiona la tecla enter para continuar..."); Console.ReadLine();
        }

    }
}