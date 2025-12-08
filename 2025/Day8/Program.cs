var inputData = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sample.txt"));
// var inputData = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "day8.txt"));

double Calculate3DEuclidean(long x1, long x2, long y1, long y2, long z1, long z2)
{
    return Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2)+ Math.Pow(z1 - z2, 2));
}

var circuits = new int[inputData.Length, inputData.Length];
var junctionBoxes = inputData.Select(x => x.Split(',').Select(long.Parse).ToArray()).ToArray();

for (int i = 0; i < junctionBoxes.Length; i++)
{
    var curr = junctionBoxes[i];
    var nextIdx = i;
    double minDistance = Double.MaxValue;
    
    for (int j = 0; j < junctionBoxes.Length; j++)
    {
        if (i==j)
        {
            continue;
        };
        var next = junctionBoxes[j];
        var distance = Calculate3DEuclidean(curr[0], next[0], 
            curr[1], next[1], curr[2],
        next[2]);
        if (distance < minDistance)
        {
            nextIdx = j;
            minDistance = distance;
        }
    }
    var matchOne = junctionBoxes[i];
    var matchTwo = junctionBoxes[nextIdx];
    Console.WriteLine(String.Join(",", matchOne));
    Console.WriteLine(String.Join(",", matchTwo));
    Console.WriteLine();
    
    circuits[i, nextIdx] = 1;
}

List<int> candidates = new List<int>();
for (int r = 0; r < circuits.GetLength(0); r++)
{
    var rowCount = 0;
    for (int c = 0; c < circuits.GetLength(1); c++)
    {
        if (circuits[r, c] == 1) rowCount++;
    }

    for (int c = 0; c < circuits.GetLength(0); c++)
    {
        if (circuits[c, r] == 1) rowCount++;    
    }
    
    candidates.Add(rowCount);

}

var partOne = candidates.OrderByDescending(x => x).Take(3);
Console.WriteLine($"Part 1: {partOne.Aggregate(1, (a,b) => a*b)}");
