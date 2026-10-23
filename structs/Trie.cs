using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Estructuras;

/// <summary>
/// Árbol de prefijos para títulos. Todo texto se normaliza: minúsculas, sin tildes,
/// sin espacios ni caracteres especiales (solo a-z y 0-9).
/// </summary>
public class Trie
{
    private const int AlphabetSize = 36; // a-z (26) + 0-9 (10)

    private class TrieNode
    {
        public TrieNode?[] Children = new TrieNode?[AlphabetSize];
        public bool IsEnd;
    }

    // Arreglo dinámico mínimo de strings (sin usar List).
    private class StringBuffer
    {
        private string[] _items = new string[8];
        public int Count { get; private set; }

        public void Add(string s)
        {
            if (Count == _items.Length)
            {
                string[] bigger = new string[_items.Length * 2];
                for (int i = 0; i < Count; i++) bigger[i] = _items[i];
                _items = bigger;
            }
            _items[Count++] = s;
        }

        public string[] ToArray()
        {
            string[] result = new string[Count];
            for (int i = 0; i < Count; i++) result[i] = _items[i];
            return result;
        }
    }

    private readonly TrieNode _root = new();

    // ---------- API pública ----------

    public void Insert(string nameBook)
    {
        string word = Normalize(nameBook);
        if (word.Length == 0) return;

        TrieNode node = _root;
        for (int i = 0; i < word.Length; i++)
        {
            int idx = CharToIndex(word[i]);
            node.Children[idx] ??= new TrieNode();
            node = node.Children[idx]!;
        }
        node.IsEnd = true;
    }

    /// <summary>Imprime las ramas (palabras completas) que parten del prefijo dado.</summary>
    public void ShowCoincidences(string nameBook)
    {
        string prefix = Normalize(nameBook);
        string[] matches = GetCoincidences(nameBook);

        Console.WriteLine($"=== Coincidencias para \"{prefix}\" ===");
        if (matches.Length == 0)
        {
            Console.WriteLine("  (sin coincidencias)");
            return;
        }

        foreach (string word in matches)
        {
            var sb = new StringBuilder("  ");
            for (int i = 0; i < word.Length; i++)
            {
                if (i == prefix.Length) sb.Append(" | "); // separa prefijo del resto
                else if (i > 0) sb.Append('-');
                sb.Append(word[i]);
            }
            Console.WriteLine($"{sb}    => {word}");
        }
    }

    /// <summary>Retorna todas las palabras (normalizadas) que empiezan con el prefijo.</summary>
    public string[] GetCoincidences(string nameBook)
    {
        string prefix = Normalize(nameBook);
        if (prefix.Length == 0) return Array.Empty<string>();

        TrieNode? node = FindNode(prefix);
        if (node == null) return Array.Empty<string>();

        var results = new StringBuffer();
        Collect(node, new StringBuilder(prefix), results);
        return results.ToArray();
    }

    /// <summary>true si existe al menos una palabra que empiece con el prefijo.</summary>
    public bool Exist(string nameBook)
    {
        string prefix = Normalize(nameBook);
        if (prefix.Length == 0) return false;
        return FindNode(prefix) != null;
    }

    // ---------- Internos ----------

    public static string Normalize(string? text)
    {
        if (string.IsNullOrEmpty(text)) return string.Empty;

        // Quita tildes: descompone (é -> e + ´) y elimina las marcas diacríticas.
        string decomposed = text.Normalize(NormalizationForm.FormD);
        var sb = new StringBuilder(decomposed.Length);
        foreach (char c in decomposed)
            if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                sb.Append(c);

        string lower = sb.ToString().ToLowerInvariant();
        // Elimina espacios y cualquier carácter que no sea a-z o 0-9.
        return Regex.Replace(lower, "[^a-z0-9]", "");
    }

    private TrieNode? FindNode(string normalized)
    {
        TrieNode? node = _root;
        for (int i = 0; i < normalized.Length && node != null; i++)
            node = node.Children[CharToIndex(normalized[i])];
        return node;
    }

    private static void Collect(TrieNode node, StringBuilder path, StringBuffer results)
    {
        if (node.IsEnd) results.Add(path.ToString());

        for (int i = 0; i < AlphabetSize; i++)
        {
            TrieNode? child = node.Children[i];
            if (child == null) continue;
            path.Append(IndexToChar(i));
            Collect(child, path, results);
            path.Length--; // backtrack
        }
    }

    private static int CharToIndex(char c) => c <= '9' ? c - '0' + 26 : c - 'a';
    private static char IndexToChar(int i) => i >= 26 ? (char)('0' + (i - 26)) : (char)('a' + i);
}
