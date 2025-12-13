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

var endIdx = vertices.Length - 1;

List<List<int>> paths = [];
List<List<int>> partTwoPath = [];
Queue<List<int>> queue = [];
    
var startIdx = Array.FindIndex(vertices, v => v == "svr"); // flipped to this for Pt2
var dacIdx = Array.FindIndex(vertices, v => v == "dac");
var fftIdx = Array.FindIndex(vertices, v => v == "fft");
queue.Enqueue([startIdx]);

while (queue.Count > 0)
{
    var path = queue.Dequeue();
    var currentIdx = path[^1];

    if (currentIdx == endIdx)
    {
        Console.WriteLine("Reached");
        paths.Add(path);
        if (path.Contains(dacIdx) && path.Contains(fftIdx))
        {
            partTwoPath.Add(path);
        }
    }


    List<int> neighbours = [];
    for (int c = 0; c < graph.GetLength(1); c++)
    {
        if (graph[currentIdx, c] == 1)
        {
            neighbours.Add(c);
        }
    }

    foreach (var idx in neighbours)
    {
        
        if (path.Contains(idx)) continue;
        List<int> newPath =
        [
            ..path,
            idx
        ];
        queue.Enqueue(newPath);
    }
}


Console.WriteLine($"Part 1:  {paths.Count}");
Console.WriteLine($"Part 2:  {partTwoPath.Count}");
Console.WriteLine($"Part 2:  {paths.Count(x=>x.Contains(dacIdx) && x.Contains(fftIdx))}");

// for (int i = 0; i < graph.GetLength(0); i++)
// {
//     for (int j = 0; j < graph.GetLength(1); j++)
//     {
//         Console.Write(graph[i, j] + " ");
//     }   
//     Console.WriteLine();
// }