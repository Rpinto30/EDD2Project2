using System;
using System.Globalization;
using System.Xml.Linq;

namespace Proyecto2EDD2.Estructuras;

public class TablaHash
{
    private class Node
    {
        public Book Book;
        public Node? Next;
        public Node(Book book, Node? next) { Book = book; Next = next; }
    }

    private const int Base = 31;
    private const double MaxLoadFactor = 0.75;

    private Node?[] _buckets;
    private int _count;

    public int Count => _count;

    public int prevId = 0;

    public TablaHash(int initialCapacity = 11)
    {
        _buckets = new Node?[NextPrime(Math.Max(initialCapacity, 3))];
    }

    public void Insert(Book book)
    {
        ArgumentNullException.ThrowIfNull(book);

        int index = Hash(book.Id, _buckets.Length);

        for (Node? cur = _buckets[index]; cur != null; cur = cur.Next)
        {
            if (cur.Book.Id == book.Id)
            {
                cur.Book = book;
                return;
            }
        }

        _buckets[index] = new Node(book, _buckets[index]);
        _count++;
        prevId = book.Id;
        if ((double)_count / _buckets.Length > MaxLoadFactor)
            Resize();
    }

    public Book? SearchById(int id)
    {
        int index = Hash(id, _buckets.Length);
        for (Node? cur = _buckets[index]; cur != null; cur = cur.Next)
            if (cur.Book.Id == id) return cur.Book;
        return null;
    }

    public Book? SearchByName(string nameBook)
    {
        for (int i = 0; i < _buckets.Length; i++)
        {
            if (_buckets[i] == null) continue;
            for (Node? cur = _buckets[i]; cur != null; cur = cur.Next)
                if (cur.Book.Title == nameBook) return cur.Book;
        }
        return null;
    }

    public void ShowTable()
    {
        Console.WriteLine($"===== Tabla Hash (capacidad: {_buckets.Length}, elementos: {_count}) =====");
        for (int i = 0; i < _buckets.Length; i++)
        {
            Console.Write($"[{i}] ");
            if (_buckets[i] == null)
            {
                Console.WriteLine("(vacío)");
                continue;
            }
            for (Node? cur = _buckets[i]; cur != null; cur = cur.Next)
            {
                Console.WriteLine($"\t({cur.Book.Id}: [Titulo: \"{cur.Book.Title}\" | Autor: {cur.Book.Author}]) | Disponible en: {cur.Book.Center}");
            }
        }
    }


    public void UpdateBookByID(int id, Book new_book)
    {
        int index = Hash(id, _buckets.Length);
        for (Node? cur = _buckets[index]; cur != null; cur = cur.Next)
        {
            if (cur.Book.Id == id)
            {
                cur.Book = new_book;
                break;
            }
        }
    }

    public int GetAutoIncrement() => this.prevId+1;


    public bool Remove(int id)
    {
        int index = Hash(id, _buckets.Length);
        Node? prev = null;
        for (Node? cur = _buckets[index]; cur != null; prev = cur, cur = cur.Next)
        {
            if (cur.Book.Id != id) continue;

            if (prev == null) _buckets[index] = cur.Next; //head
            else prev.Next = cur.Next; //any else
            _count--;
            return true;
        }
        return false;
    }

    public int DisconnectCenter(string centerName)
    {
        if (string.IsNullOrWhiteSpace(centerName)) return 0;
        string target = centerName.Trim();
        int affected = 0;

        for (int i = 0; i < _buckets.Length; i++)
        {
            for (Node? cur = _buckets[i]; cur != null; cur = cur.Next)
            {
                if (string.Equals(cur.Book.Center?.Trim(), target, StringComparison.OrdinalIgnoreCase))
                {
                    cur.Book = cur.Book with { Center = "N/A" }; 
                    affected++;
                }
            }
        }
        return affected;
    }

    private static int Hash(int id, int capacity)
    {
        string key = id.ToString(CultureInfo.InvariantCulture);
        long h = 0;
        for (int i = 0; i < key.Length; i++)
            h = (h * Base + key[i]) % capacity;
        return (int)h;
    }

    private void Resize()
    {
        Node?[] old = _buckets;
        Node?[] fresh = new Node?[NextPrime(old.Length * 2 + 1)];

        for (int i = 0; i < old.Length; i++)
        {
            Node? cur = old[i];
            while (cur != null)
            {
                Node? next = cur.Next;
                int idx = Hash(cur.Book.Id, fresh.Length);
                cur.Next = fresh[idx];
                fresh[idx] = cur;
                cur = next;
            }
        }
        _buckets = fresh;
    }

    private static int NextPrime(int n)
    {
        if (n <= 2) return 2;
        if (n % 2 == 0) n++;
        while (!IsPrime(n)) n += 2;
        return n;
    }

    private static bool IsPrime(int n)
    {
        if (n < 2) return false;
        if (n % 2 == 0) return n == 2;
        for (int i = 3; (long)i * i <= n; i += 2)
            if (n % i == 0) return false;
        return true;
    }

    public string[][] GetCleanNames()
    {
        string[][] result = new string[_count][];
        int k = 0;

        for (int i = 0; i < _buckets.Length; i++)
        {
            for (Node? cur = _buckets[i]; cur != null; cur = cur.Next)
            {
                result[k++] = new[]
                {
                cur.Book.Id.ToString(CultureInfo.InvariantCulture),
                cur.Book.Title,
                cur.Book.Author,
                cur.Book.Center
            };
            }
        }
        return result;
    }

}
