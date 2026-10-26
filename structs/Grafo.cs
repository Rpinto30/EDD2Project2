using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Proyecto2EDD2.Estructuras;

public class Grafo
{
    private class EdgeNode
    {
        public int To;
        public double Time;
        public double Cost;
        public EdgeNode? Next;
        public EdgeNode(int to, double time, double cost) { To = to; Time = time; Cost = cost; }
    }

    private string[] _names = new string[8];
    private EdgeNode?[] _adj = new EdgeNode?[8];
    private int _count;
    private UnionFind _uf = new();

    public int NodeCount => _count;

    // ---------- Construcción ----------
    public void InsertNode(string nameCenter)
    {
        if (string.IsNullOrWhiteSpace(nameCenter))
            throw new ArgumentException("El nombre de la sede no puede estar vacío.");
        if (IndexOf(nameCenter) >= 0)
            throw new ArgumentException("El nombre de la sede ya se encuentra en el registro."); 

        if (_count == _names.Length)
        {
            string[] n = new string[_count * 2];
            EdgeNode?[] a = new EdgeNode?[_count * 2];
            for (int i = 0; i < _count; i++) { n[i] = _names[i]; a[i] = _adj[i]; }
            _names = n;
            _adj = a;
        }

        _names[_count] = nameCenter.Trim();
        _uf.MakeSet(); //unionfind
        _count++;
    }

    /// <summary> default (edge.Bidirectional = 0)</summary>
    public void InsertConnection(string origin, string dest, CenterEdge edge)
    {
        ArgumentNullException.ThrowIfNull(edge);
        if (edge.Time < 0 || edge.Cost < 0)
            throw new ArgumentException("Tiempo y costo no pueden ser negativos.");

        int o = RequireIndex(origin);
        int d = RequireIndex(dest);
        if (o == d) throw new ArgumentException("Origen y destino no pueden ser la misma sede.");

        //if (AlreadyConnect(o, dest)) throw new Exception("El origen ya contiene ese destino");
        //if (AlreadyConnect(d, origin)) throw new Exception("El destino ya contiene ese origen");

        AddDirected(o, d, edge.Time, edge.Cost);
        if (edge.Bidirectional == 1)
            AddDirected(d, o, edge.Time, edge.Cost);

        _uf.Union(o, d);
    }

    public bool IsSameRed(string origin, string dest)
    {
        int o = RequireIndex(origin);
        int d = RequireIndex(dest);
        if (o == d) throw new ArgumentException("Origen y destino son la misma sede.");

        bool connected = _uf.Connected(o, d);
        return connected;
    }

    // ---------- Recorridos ----------
    public void Dfs()
    {
        Console.WriteLine("============== DFS ==============");
        if (_count == 0) { Console.WriteLine("(grafo vacío)"); return; }

        bool[] visited = new bool[_count];
        var sb = new StringBuilder();
        for (int i = 0; i < _count; i++)
            if (!visited[i]) DfsVisit(i, visited, sb);
        Console.WriteLine(sb.ToString());
    }

    private void DfsVisit(int u, bool[] visited, StringBuilder sb)
    {
        visited[u] = true;
        if (sb.Length > 0) sb.Append(" -> ");
        sb.Append(_names[u]);
        for (EdgeNode? e = _adj[u]; e != null; e = e.Next)
            if (!visited[e.To]) DfsVisit(e.To, visited, sb);
    }

    public void Bfs()
    {
        Console.WriteLine("============== BFS ==============");
        if (_count == 0) { Console.WriteLine("(grafo vacío)"); return; }

        bool[] visited = new bool[_count];
        var queue = new Queue<int>();
        var sb = new StringBuilder();

        for (int s = 0; s < _count; s++)
        {
            if (visited[s]) continue;
            visited[s] = true;
            queue.Enqueue(s);
            while (queue.Count > 0)
            {
                int u = queue.Dequeue();
                if (sb.Length > 0) sb.Append(" -> ");
                sb.Append(_names[u]);
                for (EdgeNode? e = _adj[u]; e != null; e = e.Next)
                {
                    if (visited[e.To]) continue;
                    visited[e.To] = true;
                    queue.Enqueue(e.To);
                }
            }
        }
        Console.WriteLine(sb.ToString());
    }

    public string[] GetNames() {
        bool allNull = true;
        for (int i = 0; i < _names.Length; i++)
        {
            if (_names[i] != null)
            {
                allNull = false;
                break; // Encontró un dato, dejamos de buscar
            }
        }
        return (!allNull)?this._names: new string[0];
    }
    

    public void ShowMatrix()
    {
        Console.WriteLine("============== Lista de adyacencia ==============");
        for (int i = 0; i < _count; i++)
        {
            Console.Write($"{_names[i]}: ");
            if (_adj[i] == null) { Console.WriteLine("(sin conexiones)"); continue; }
            for (EdgeNode? e = _adj[i]; e != null; e = e.Next)
                Console.Write($"[Destino: {_names[e.To]} | Tiempo: {e.Time}m | Costo: Q{e.Cost:0.##}] ");
            Console.WriteLine();
        }
    }

    // ---------- Dijkstra ----------
    public RouteResult? DijkstraCost(string origin, string dest)
        => RunDijkstra(origin, dest, RouteCriteria.Cost, 0.5);

    public RouteResult? DijkstraTime(string origin, string dest)
        => RunDijkstra(origin, dest, RouteCriteria.Time, 0.5);

    public RouteResult? Dijkstra(string origin, string dest, double timeWeight = 0.5)
    {
        if (timeWeight < 0 || timeWeight > 1)
            throw new ArgumentOutOfRangeException(nameof(timeWeight), "Debe estar entre 0 y 1.");
        return RunDijkstra(origin, dest, RouteCriteria.Combined, timeWeight);
    }

    public bool AreConnected(string a, string b) => _uf.Connected(RequireIndex(a), RequireIndex(b));

    private RouteResult? RunDijkstra(string origin, string dest, RouteCriteria criteria, double timeWeight)
    {
        int s = RequireIndex(origin);
        int t = RequireIndex(dest);

        if (!_uf.Connected(s, t)) return null;

        double maxTime = 1, maxCost = 1;
        if (criteria == RouteCriteria.Combined)
        {
            maxTime = 0; maxCost = 0;
            for (int i = 0; i < _count; i++)
                for (EdgeNode? e = _adj[i]; e != null; e = e.Next)
                {
                    if (e.Time > maxTime) maxTime = e.Time;
                    if (e.Cost > maxCost) maxCost = e.Cost;
                }
            if (maxTime == 0) maxTime = 1;
            if (maxCost == 0) maxCost = 1;
        }

        double[] dist = new double[_count];
        int[] prev = new int[_count];
        double[] totTime = new double[_count];
        double[] totCost = new double[_count];
        bool[] done = new bool[_count];
        for (int i = 0; i < _count; i++) { dist[i] = double.PositiveInfinity; prev[i] = -1; }
        dist[s] = 0;

        for (int iter = 0; iter < _count; iter++)
        {
            int u = -1;
            for (int i = 0; i < _count; i++)
                if (!done[i] && !double.IsPositiveInfinity(dist[i]) && (u == -1 || dist[i] < dist[u]))
                    u = i;
            if (u == -1 || u == t) break;
            done[u] = true;

            for (EdgeNode? e = _adj[u]; e != null; e = e.Next)
            {
                if (done[e.To]) continue;
                double w = criteria switch
                {
                    RouteCriteria.Time => e.Time,
                    RouteCriteria.Cost => e.Cost,
                    _ => timeWeight * (e.Time / maxTime) + (1 - timeWeight) * (e.Cost / maxCost)
                    // w* time + (1- w) * cost
                };
                double nd = dist[u] + w;
                if (nd < dist[e.To])
                {
                    dist[e.To] = nd;
                    prev[e.To] = u;
                    totTime[e.To] = totTime[u] + e.Time;
                    totCost[e.To] = totCost[u] + e.Cost;
                }
            }
        }

        if (double.IsPositiveInfinity(dist[t])) return null;

        int len = 0;
        for (int v = t; v != -1; v = prev[v]) len++;
        string[] path = new string[len];
        int k = len - 1;
        for (int v = t; v != -1; v = prev[v]) path[k--] = _names[v];

        return new RouteResult(path, totTime[t], totCost[t], dist[t], criteria);
    }

    public bool RemoveConnection(string origin, string dest, bool bidirectional = false)
    {
        int o = RequireIndex(origin);
        int d = RequireIndex(dest);

        bool removed = RemoveDirected(o, d);
        if (bidirectional) removed |= RemoveDirected(d, o);

        if (removed) RebuildUnionFind();
        return removed;
    }

    public bool RemoveNode(string nameCenter)
    {
        int r = IndexOf(nameCenter);
        if (r < 0) return false;

        // Remove edges
        for (int i = 0; i < _count; i++)
            if (i != r) RemoveDirected(i, r);

        for (int i = r; i < _count - 1; i++)
        {
            _names[i] = _names[i + 1];
            _adj[i] = _adj[i + 1];
        }
        _count--;
        _names[_count] = null!;
        _adj[_count] = null;

        // renum dest
        for (int i = 0; i < _count; i++)
            for (EdgeNode? e = _adj[i]; e != null; e = e.Next)
                if (e.To > r) e.To--;

        RebuildUnionFind();
        return true;
    }

    private bool RemoveDirected(int from, int to)
    {
        EdgeNode? prev = null;
        for (EdgeNode? cur = _adj[from]; cur != null; prev = cur, cur = cur.Next)
        {
            if (cur.To != to) continue;
            if (prev == null) _adj[from] = cur.Next;
            else prev.Next = cur.Next;
            return true;
        }
        return false;
    }

   
    private void RebuildUnionFind()
    {
        _uf = new UnionFind(Math.Max(_count, 1));
        for (int i = 0; i < _count; i++) _uf.MakeSet();
        for (int i = 0; i < _count; i++)
            for (EdgeNode? e = _adj[i]; e != null; e = e.Next)
                _uf.Union(i, e.To);
    }


    // ---------- Internos ----------

    public bool AlreadyConnect(int index, string centerName)
    {
        Console.Write($"{_names[index]}: ");
        if (_adj[index] == null) { return false; }
        for (EdgeNode? e = _adj[index]; e != null; e = e.Next)
        {
            if (_names[e.To] == centerName) return true;
        }
        return false;
    }

    private void AddDirected(int from, int to, double time, double cost)
    {
        EdgeNode? cur = _adj[from], last = null;
        while (cur != null)
        {
            if (cur.To == to) { cur.Time = time; cur.Cost = cost; return; }
            last = cur;
            cur = cur.Next;
        }
        var node = new EdgeNode(to, time, cost);
        if (last == null) _adj[from] = node; else last.Next = node;
    }

    private int IndexOf(string name)
    {
        for (int i = 0; i < _count; i++)
            if (string.Equals(_names[i], name?.Trim(), StringComparison.OrdinalIgnoreCase)) return i;
        return -1;
    }

    private int RequireIndex(string name)
    {
        int i = IndexOf(name);
        if (i < 0) throw new InvalidOperationException($"La sede \"{name}\" no existe en el grafo.");
        return i;
    }

    public bool InGraph(string name)
    {
        try
        {
            RequireIndex(name);
            return true;
        } catch
        {
            return false;
        }
    }

    public string[][] GetCleanNames()
    {
        string[] temp = new string[_names.Length];
        int k = 0;

        for (int i = 0; i < _names.Length; i++)
            if (!string.IsNullOrEmpty(_names[i]))
                temp[k++] = _names[i]!;

        string[][] result = new string[k][];
        for (int i = 0; i < k; i++) 
            result[i] = new string[]{ temp[i] };
        return result;
    }

    public string[][] GetConnections()
    {
        if (_count == 0) return Array.Empty<string[]>();
        int maxRows = 0;
        for (int i = 0; i < _count; i++)
            for (EdgeNode? e = _adj[i]; e != null; e = e.Next) maxRows++;

        string[][] rows = new string[maxRows][];
        int k = 0;

        bool[] visited = new bool[_count];
        var queue = new Queue<int>();

        for (int s = 0; s < _count; s++)
        {
            if (visited[s]) continue;
            visited[s] = true;
            queue.Enqueue(s);

            while (queue.Count > 0)
            {
                int u = queue.Dequeue();
                for (EdgeNode? e = _adj[u]; e != null; e = e.Next)
                {
                    EdgeNode? back = FindEdge(e.To, u);
                    bool isPair = back != null && back.Time == e.Time && back.Cost == e.Cost;
                    if (!isPair || u < e.To)
                    {
                        rows[k++] = new[]
                        {
                            _names[u],
                            _names[e.To],
                            e.Time.ToString(CultureInfo.InvariantCulture),
                            e.Cost.ToString(CultureInfo.InvariantCulture),
                            isPair ? "1" : "0"
                        };
                    }

                    if (visited[e.To]) continue;
                    visited[e.To] = true;
                    queue.Enqueue(e.To);
                }
            }
        }

        Array.Resize(ref rows, k);
        return rows;
    }

    private EdgeNode? FindEdge(int from, int to)
    {
        for (EdgeNode? e = _adj[from]; e != null; e = e.Next)
            if (e.To == to) return e;
        return null;
    }
}
