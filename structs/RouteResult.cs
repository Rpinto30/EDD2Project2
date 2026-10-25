using System.Globalization;
using System.Text;

namespace Proyecto2EDD2.Estructuras;

public enum RouteCriteria { Cost, Time, Combined }


public record RouteResult(string[] Path, double TotalTime, double TotalCost, double Weight, RouteCriteria Criteria)
{
    public override string ToString()
    {
        var sb = new StringBuilder();
        for (int i = 0; i < Path.Length; i++)
        {
            if (i > 0) sb.Append(" -> ");
            sb.Append(Path[i]);
        }

        string r = string.Empty;
        switch (Criteria)
        {
            case RouteCriteria.Cost:
                r = "MÁS BARATA";
                break;
            case RouteCriteria.Time:
                r = "MÁS RÁPIDA";
                break;
            case RouteCriteria.Combined:
                r = "MÁS BARATA Y RÁPIDA";
                break;
            default:
                r = "N/A";
                break;
        }

        return $"[{r}] {sb} | Tiempo: {TotalTime}s | Costo: Q{TotalCost.ToString("0.##", CultureInfo.InvariantCulture)}";
    }
}
