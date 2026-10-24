using System.Globalization;
using System.Text;

namespace Proyecto2EDD2.Estructuras;

public enum RouteCriteria { Cost, Time, Combined }


public record RouteResult(string[] Path, int TotalTime, double TotalCost, double Weight, RouteCriteria Criteria)
{
    public override string ToString()
    {
        var sb = new StringBuilder();
        for (int i = 0; i < Path.Length; i++)
        {
            if (i > 0) sb.Append(" -> ");
            sb.Append(Path[i]);
        }
        return $"[{Criteria}] {sb} | Tiempo: {TotalTime}s | Costo: {TotalCost.ToString("0.##", CultureInfo.InvariantCulture)}";
    }
}
