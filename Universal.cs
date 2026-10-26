using Proyecto2EDD2.Estructuras;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Universal
{
    public class Universal
    {
        public static Grafo graph = new Grafo();
        public static TablaHash table = new TablaHash(20);
        public static Trie trie = new Trie();
        public static Trie trieCentres = new Trie();

        public static void WaitEntry()
        {
            Console.WriteLine("Presiona la tecla enter para continuar..."); Console.ReadLine();
        }

        public static string RemoverTildes(string texto)
        {
            var normalizedString = texto.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

    }


}