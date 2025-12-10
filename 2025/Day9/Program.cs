var inputData = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sample.txt"));
// var inputData = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "day9.txt"));

var points = inputData.Select(line => line.Split(",")).Select(x
    => new Point { x = long.Parse(x[0]), y = long.Parse(x[1]) }).ToArray();

HashSet<Line> lines = [];

long maxArea = 0;
for (int i = 0; i < points.Length; i++)
{
    var currPoint = points[i];
    for (int j = 0; j < points.Length; j++)
    {
        if (i == j) continue;

        var (xCoord, yCoord) = (currPoint.x, currPoint.y);
        var (xCoord2, yCoord2) = (points[j].x, points[j].y);

        long area = (Math.Abs(xCoord - xCoord2 + 1) * (Math.Abs(yCoord - yCoord2) + 1));
        if (area > maxArea)
        {
            maxArea = area;
        }
    }
    
    var closestHorizontal = points.Where(p => p.x == currPoint.x && p.y != currPoint.y)
        .OrderBy(p => Math.Abs(currPoint.y - p.y)).First();
    var closestVertical = points.Where(p => p.y == currPoint.y && p.x != currPoint.x)
        .OrderBy(p => Math.Abs(currPoint.x - p.x)).First();
    
    var conflictHori = lines.Add(new Line { PointOne = currPoint, PointTwo = closestHorizontal});
    var conflictVert = lines.Add(new Line { PointOne = currPoint, PointTwo = closestVertical});
    if (!conflictHori || !conflictVert)
    {
        throw new Exception("cannot happen >:(");
    }
}

Console.WriteLine($"Part 1: {maxArea}");



var pop = lines.ToArray()[0];
foreach (var line in lines)
{
    Console.WriteLine( line.DoesIntersect(pop));   ;
}

public record Point
{
    public long x { get; set; }
    public long y { get; set; }
    public override string ToString() => $"{x}, {y}";
}

public record Line
{
    public Point PointOne { get; set; }
    public Point PointTwo { get; set; }
    
    // y - PointOne.y = Gradient * (x - PointOne.x)
    // y = Gradient * x - PointOne.x * Gradient + PointOne.y
    
    // Gradient * x - Point.x * Gradient + PointOne.y = Gradient * x - Point.x * Gradient + PointOne.y
    
    public double Gradient  => (PointOne.x == PointTwo.x) 
        ? 0 
        : (PointOne.y - PointTwo.y) / (PointOne.x - PointTwo.x);
    public bool DoesIntersect(Line other)
    {
        var intersectionX  =
            (PointOne.x * Gradient + PointOne.y - (other.PointOne.x * other.Gradient + other.PointOne.y))/ (Gradient - other.Gradient);
        var intersectionY  = Gradient * intersectionX - PointOne.x * Gradient + PointOne.y;
        
         
        var otherIntersectionY = intersectionX * other.Gradient - other.PointOne.x * other.Gradient + other.PointOne.y;
        return Math.Abs(otherIntersectionY - intersectionY) < 10e-5;
    }
}