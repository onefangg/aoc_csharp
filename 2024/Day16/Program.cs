var data = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "day16_sample.txt"));
var gridData = data.Select(x => x.ToCharArray()).ToArray();

var distance = InitDistanceGrid(gridData);
var distanceMap = new Dictionary<Vertex, int>();
var visited = InitVisitedGrid(gridData);
var (startX, startY) = FindPos(gridData, 'S');
var (endX, endY) = FindPos(gridData, 'E');
var linkVertices = new Dictionary<Vertex, List<Vertex>>();
var priorityQueue = new SortedSet<Vertex>();
var minCost = int.MaxValue;
var startVertex = new Vertex()
{
    Row = startX,
    Col = startY,
    Dr = 0,
    Dc = 1,
    Cost = 0
};
priorityQueue.Add(startVertex);
distance[startX][startY] = 0;
distanceMap[startVertex] = 0;

while (priorityQueue.Any())
{
    var curr = priorityQueue.Min()!;
    priorityQueue.Remove(curr);
    if (visited[curr.Row][curr.Col]) continue;
    visited[curr.Row][curr.Col] = true;
    
    // VisualiseGrid(gridData, visited);
    (int,int)[] directions = [
        (curr.Dr, curr.Dc),
        curr.TurnLeft(),
        curr.TurnRight(),
    ];

    if (gridData[curr.Row][curr.Col] == 'E')
    {
        if (distance[curr.Row][curr.Col] <= minCost)
        {
            minCost = distance[curr.Row][curr.Col];    
        }
    }
    
    for (var idx = 0; idx < directions.Length; idx++)
    {
        var (dr, dc) = directions[idx];
        var nextRow = curr.Row + dr;
        var nextCol = curr.Col + dc;
        if (gridData[nextRow][nextCol] != '#')
        {
            var newDist = curr.Cost + (idx == 0 ? 1 : 1001);
            var linkedChild = new Vertex()
            {
                Row = nextRow,
                Col = nextCol,
                Dr = dr,
                Dc = dc,
                Cost = newDist
            };
            distanceMap[linkedChild] = newDist;
            linkVertices[linkedChild] = [curr];
            if (newDist < distance[nextRow][nextCol])
            {
                distance[nextRow][nextCol] = newDist; 
                priorityQueue.Add(linkedChild);    
            }
        }

    }
}

Console.WriteLine($"Part 1: {minCost}");

var endNodes = linkVertices.Where(x => x.Key.Row == endX && x.Key.Col == endY)
    .Select(x=>x.Key with { Dr = -1* x.Key.Dr, Dc = -1*x.Key.Dc}).ToArray();
var invertQueue = new Queue<Vertex>(endNodes);
var paths = InitFalseyGrid(gridData);

var visitedAgain = new HashSet<Vertex>();

while (invertQueue.Any())
{
    var curr = invertQueue.Dequeue();
    paths[curr.Row][curr.Col] = true;
    if (!visitedAgain.Add(curr) || curr.Cost<0)
    {
        continue;
    }
    (int,int)[] directions = [
        (curr.Dr, curr.Dc),
        curr.TurnLeft(),
        curr.TurnRight(),
    ];

    for (var i = 0; i < directions.Length; i++)
    {
        var nextRow = curr.Row + directions[i].Item1;
        var nextCol = curr.Col + directions[i].Item2;
        var reachable = distanceMap.Where(x => x.Key.Row == nextRow && x.Key.Col == nextCol).Select(x => (x.Key, x.Value)).ToArray();
        foreach (var (distanceNode, dist) in reachable)
        {
            if (dist == curr.Cost - 1 || dist == curr.Cost - 1001 )
            {
                invertQueue.Enqueue(new Vertex()
                {
                    Row = nextRow,
                    Col = nextCol,
                    Dr = -1*distanceNode.Dr,
                    Dc = -1*distanceNode.Dc,
                    Cost = dist
                });
                
            }
        }
    }
    
}

Console.WriteLine($"Part 2: {paths.SelectMany(x => x.ToArray()).Count(x=>x)}");

bool[][] InitFalseyGrid(char[][] grid)
{
    var visited = new bool[grid.Length][];
    for (int r = 0; r < grid.Length; r++)
    {
        var row = new bool[grid[r].Length];
        for (int c = 0; c < grid[r].Length; c++)
        {
            row[c] = false;
        }
        visited[r] = row;
    }
    return visited;
}
void VisualiseGrid(char[][] grid, bool[][] visited)
{
    for (int i = 0; i < grid.Length; i++)
    {
        for (int j = 0; j < grid[i].Length; j++)
        {
            if (grid[i][j] == '#')
            {
                Console.Write(grid[i][j]);
            } else if (visited[i][j])
            {
                Console.Write('O');
            }
            else
            {
                Console.Write('.');
            }
        }
        Console.WriteLine();
    }
}

int[][] InitDistanceGrid(char[][] grid)
{
    var distance = new int[grid.Length][];
    for (int r = 0; r < grid.Length; r++)
    {
        
        var row = new int[grid[r].Length];
        for (int c = 0; c < grid[r].Length; c++)
        {
            row[c] = int.MaxValue;
        }
        distance[r] = row;
    }
    return distance;
}

(int, int) FindPos(char[][] grid, char ele)
{
    for (int r = 0; r < grid.Length; r++)
    {
        for (int c = 0; c < grid[r].Length; c++)
        {
            if (grid[r][c] == ele) return (r, c);
        }
    }
    return (-1, -1);
}

bool[][] InitVisitedGrid(char[][] grid)
{
    var visited = new bool[grid.Length][];
    for (int r = 0; r < grid.Length; r++)
    {
        
        var row = new bool[grid[r].Length];
        for (int c = 0; c < grid[r].Length; c++)
        {
            if (grid[r][c] == '#')
            {
                row[c] = false;
            }
            else
            {
                row[c] = false;
            }
        }
        visited[r] = row;
    }
    return visited;
}

record Vertex : IComparable<Vertex>
{
    public int Row { get; set; }
    public int Col { get; set; }
    public int Dr { get; set; }
    public int Dc { get; set; }
    public int Cost { get; set; }

    public (int, int) TurnLeft()
    {
        return (-1 * Dc, Dr);
    }
    public (int, int) TurnRight()
    {
        return (Dc, Dr * -1);
    }

    public int CompareTo(Vertex other)
    {
        if (Cost == other.Cost)
        {
            if (Row == other.Row)
            {
                if (Col == other.Col)
                {
                    if (Dr == other.Dr)
                    {
                        return Dc.CompareTo(other.Dc);
                    }           
                    return Dr.CompareTo(other.Dr);
                }
                return Col.CompareTo(other.Col);
            }
            return Row.CompareTo(other.Row);
        }
        return Cost.CompareTo(other.Cost);
    }
    
}

