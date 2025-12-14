// var inputData = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sample.txt"));
var inputData = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "day12.txt"));
var sepIdx = Array.FindLastIndex(inputData, x => x.Length == 0);

var shapes = String.Join("", inputData.Take(sepIdx).Select(x =>
    {
        if (x == "") return "/n";
        return x;
    })).Split("/n")
    .Select(x =>
    {
        var parts = x.Split(':');
        var grid = new char[3][];
        grid[0] = parts[1][..3].ToCharArray();
        grid[1] = parts[1][3..6].ToCharArray();
        grid[2] = parts[1][6..9].ToCharArray();
        
        return new Shape { Index = int.Parse(parts[0]), Grid = grid};

    }).ToArray();
    
var trees = inputData.Skip(sepIdx+1).Take(inputData.Length - sepIdx).Select(x =>
{
    var parts = x.Split(": ");
    return new Tree(parts[0], parts[1]);

}).ToArray(); 

var partOne = 0;
foreach (var t in trees)
{
    // doing the cheese solution
    var area = t.Bound.Item1 * t.Bound.Item2;
    var requiredShapes = t.Regions.Select((r, i) => (r, i)).Where(x => x.Item1 > 0).ToDictionary(
        x=>x.i, x=>x.r);

    var shapeArea = 0;
    foreach (var (idx, cnt) in requiredShapes)
    {
        // shapeArea += cnt * shapes.Single(x => x.Index == idx).OccupiedSpace;
        shapeArea += cnt * 9;
    }

    if (shapeArea <= area) partOne++;
    
}
Console.WriteLine($"Part 1: {partOne}");

record Shape
{
    public int Index { get; set; }
    public char[][] Grid { get; set; }
    public int OccupiedSpace => Grid.SelectMany(x => x).Count(x => x =='#');
}

record Tree
{
    public Tree(string bound, string regions)
    {
        var b = bound.Split("x").Select(int.Parse).ToArray();
        Bound = (b[0], b[1]);
        Regions = regions.Split(" ").Select(int.Parse).ToArray();
    }
    
    public (int, int) Bound { get; set; } 
    public int[] Regions { get; set; } 
}
