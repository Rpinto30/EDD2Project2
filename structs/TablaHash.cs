using System;
using System.Globalization;

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

    public void ShowTable()
    {
        Console.WriteLine($"=== Tabla Hash (capacidad: {_buckets.Length}, elementos: {_count}) ===");
        for (int i = 0; i < _buckets.Length; i++)
        {
            Console.Write($"[{i}] ");
            if (_buckets[i] == null)
            {
                Console.WriteLine("(vacío)");
                continue;
            }
            for (Node? cur = _buckets[i]; cur != null; cur = cur.Next)
                Console.Write($"({cur.Book.Id}: \"{cur.Book.Title}\" - {cur.Book.Author}) -> ");
            Console.WriteLine("null");
        }
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
}
