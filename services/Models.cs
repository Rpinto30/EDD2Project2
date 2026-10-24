namespace Proyecto2EDD2.Estructuras;

public record Book(int Id, string Title, string Author, string Center);
public record Center(string CenterName);
public record Connection(int BookId, string CenterName);

public record CenterEdge(double Time, double Cost, int Bidirectional = 0);
