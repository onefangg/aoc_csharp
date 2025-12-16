// var inputData = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sample.txt"));
// var inputData = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "samplePt2.txt"));
var inputData = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "day11.txt"));

var vertices = new string[inputData.Length+1];
for (int i = 0; i < inputData.Length; i++)
{
    var line = inputData[i];
    var from  = line.Split(": ")[0];
    vertices[i] = from;
}
vertices[^1] = "out";

var graph = new int[vertices.Length, vertices.Length];
foreach (var line in inputData)
{
    var parts = line.Split(": ");
    var fromIdx  = Array.FindIndex(vertices, v => v == parts[0]);
    var toIndices = parts[1].Split(" ").Select(x => Array.FindIndex(vertices, y=>y==x)).ToArray();
    foreach (var idxE in toIndices)
    {
        graph[fromIdx, idxE] = 1;
    }
}



long Dfs(int[,] inputGraph,  long[] results, int startIdx, int endIdx)
{
    if (startIdx == endIdx) return 1;
    if (results[startIdx] == long.MaxValue)
    {
        
        List<int> neighbours = [];
        for (int c = 0; c < inputGraph.GetLength(1); c++)
        {
            if (graph[startIdx, c] == 1)
            {
                neighbours.Add(c);
            }
        }

        var result = neighbours.Select(x => Dfs(inputGraph, results, x, endIdx)).Sum();
        results[startIdx] =  result;
        return result;
    }
    
    return results[startIdx];
    
}


long FindPaths(string[] v, string start, string end)
{
    var visited = new long[v.Length];
    Array.Fill(visited, long.MaxValue);
    var startIdx = Array.FindIndex(v, x => x == start);
    var endIdx = Array.FindIndex(v, x => x == end);
    
    return Dfs(graph, visited, startIdx, endIdx);
}


var possiblePath = FindPaths(vertices, "svr", "fft") * FindPaths(vertices, "fft", "dac") *
                   FindPaths(vertices, "dac", "out")
                   + FindPaths(vertices, "svr", "dac") * FindPaths(vertices, "dac", "fft") *
                   FindPaths(vertices, "fft", "out");
// Console.WriteLine($"Part 1:  {FindPaths(vertices, "you", "out")}");
Console.WriteLine($"Part 2:  {possiblePath}");
