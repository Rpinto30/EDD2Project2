using System;

namespace Proyecto2EDD2.Estructuras;


public class UnionFind
{
    private int[] _parent;
    private int[] _rank;

    public int Count { get; private set; }
    public int Components { get; private set; }

    public UnionFind(int initialCapacity = 8)
    {
        int cap = Math.Max(initialCapacity, 1);
        _parent = new int[cap];
        _rank = new int[cap];
    }

    /// <summary>Crea un nuevo conjunto unitario y devuelve su índice.</summary>
    public int MakeSet()
    {
        if (Count == _parent.Length)
        {
            int[] p = new int[_parent.Length * 2];
            int[] r = new int[_rank.Length * 2];
            for (int i = 0; i < Count; i++) { p[i] = _parent[i]; r[i] = _rank[i]; }
            _parent = p;
            _rank = r;
        }
        _parent[Count] = Count;
        _rank[Count] = 0;
        Components++;
        return Count++;
    }

    public int Find(int x)
    {
        Validate(x);
        int root = x;
        while (_parent[root] != root) root = _parent[root];


        while (_parent[x] != root)
        {
            int next = _parent[x];
            _parent[x] = root;
            x = next;
        }
        return root;
    }

    /// <summary>Une los conjuntos de a y b. Retorna false si ya estaban unidos.</summary>
    public bool Union(int a, int b)
    {
        int ra = Find(a), rb = Find(b);
        if (ra == rb) return false;

        if (_rank[ra] < _rank[rb]) _parent[ra] = rb;
        else if (_rank[ra] > _rank[rb]) _parent[rb] = ra;
        else { _parent[rb] = ra; _rank[ra]++; }

        Components--;
        return true;
    }

    public bool Connected(int a, int b) => Find(a) == Find(b);

    private void Validate(int x)
    {
        if (x < 0 || x >= Count) throw new ArgumentOutOfRangeException(nameof(x));
    }
}
