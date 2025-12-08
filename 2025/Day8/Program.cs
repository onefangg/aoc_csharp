// var inputData = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sample.txt"));
var inputData = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "day8.txt"));

double Calculate3DEuclidean(long x1, long x2, long y1, long y2, long z1, long z2)
{
    return Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2)+ Math.Pow(z1 - z2, 2));
}

var junctionBoxes = inputData.Select(x => x.Split(',').Select(long.Parse).ToArray()).ToArray();

var (distancesMatrix, circuitsConnection) = GetDistanceMatrixAndConnectedGraph(junctionBoxes, inputData.Length);
var partOne = SearchConnectedComponentsForPartOne(circuitsConnection, distancesMatrix, 1000);
Console.WriteLine($"Part 1: {partOne}");

(double[,], int[,]) GetDistanceMatrixAndConnectedGraph(long[][] inputJunctionBoxes, int n)
{
    var distances = new double[n, n];
    var circuits = new int[n, n];

    for (int i = 0; i < inputJunctionBoxes.Length; i++)
    {
        var curr = inputJunctionBoxes[i];
        for (int j = 0; j < inputJunctionBoxes.Length; j++)
        {
            if (i == j)
            {
                distances[i, j] = -1;
                circuits[i, j] = 0;
                continue;
            }
            var next = inputJunctionBoxes[j];
            var distance = Calculate3DEuclidean(curr[0], next[0], curr[1], next[1], curr[2], next[2]);
            distances[i, j] = distance;
        }
    }

    return (distances, circuits);
}


int SearchConnectedComponentsForPartOne(int[,] inputCircuits, double[,] distanceMatrix, int iteration = 10)
{
    int[,] circuits = (int[,])inputCircuits.Clone();
    double[,] distances = (double[,])distanceMatrix.Clone();
    
    while (iteration > 0)
    {
        iteration--;
        var minDistance = double.MaxValue;
        var conn = (-1, -1);
        for (int i = 0; i < inputData.Length; i++)
        {
            for (int j = 0; j < inputData.Length; j++)
            {
                if (i==j) continue;
                var currDistance = distances[i, j];
                if (currDistance < minDistance)
                {
                    conn = (i, j);
                    minDistance = currDistance;
                }
            }
        }

    
        if (conn.Item1 == -1 || conn.Item2 == -1) break;
    
        circuits[conn.Item1, conn.Item2] = 1;
        circuits[conn.Item2, conn.Item1] = 1;
        distances[conn.Item1, conn.Item2] = double.MaxValue;
        distances[conn.Item2, conn.Item1] = double.MaxValue;
    }

    List<int> sizes = [];
    int[,]  visited = new int[circuits.GetLength(0), circuits.GetLength(1)];
    
    foreach (var node in Enumerable.Range(0, circuits.GetLength(0)))
    {
        HashSet<int> connectedComponents = new HashSet<int>();
        Queue<int> queue = [];
        queue.Enqueue(node);
        
        while (queue.Count > 0)
        {
            var curr = queue.Dequeue();
            
            connectedComponents.Add(curr);
            var getNeighbors = Enumerable.Range(0, circuits.GetLength(1)).
                Where(x=>circuits[curr, x] == 1).Select(x => x).ToArray();
            foreach (var neighbor in getNeighbors) 
            {
                if (visited[curr, neighbor] == 1) continue; 
                visited[curr, neighbor] = 1;
                visited[neighbor, curr] = 1;
                queue.Enqueue(neighbor);
            }
        }
        sizes.Add(connectedComponents.Count);
    }
    return sizes.OrderDescending().Take(3).Aggregate(1, (a, b) => a * b);
}