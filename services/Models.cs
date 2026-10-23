namespace Estructuras;

public record Book(int Id, string Title, string Author, string Center);
public record Center(string CenterName);
public record Connection(int BookId, string CenterName);

public record CenterEdge(int Time, double Cost, int Bidirectional = 0);
