var inputData = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sample.txt"));
// var inputData = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "day9.txt"));

var points = inputData.Select(line => line.Split(",").Select(long.Parse).ToArray()).ToArray();


Dictionary< (long, long), (long, long)> coord =  [];

long maxArea = 0;
for (int i = 0; i < points.Length; i++)
{
    for (int j = 0; j < points.Length; j++)
    {
        if (i == j) continue;

        var (xCoord, yCoord) = (points[i][0], points[i][1]);
        var (xCoord2, yCoord2) = (points[j][0], points[j][1]);
        
        // var (pointX, pointY) = xCoord > xCoord2 && yCoord > yCoord2 ? (xCoord, yCoord) : (xCoord2, yCoord2);
        //
        // var magnitude = Math.Sqrt(Math.Pow(pointX, 2) + Math.Pow(pointY, 2));
        // var direction = Math.Atan2(pointX, pointY);

        var isHorizontal = xCoord - xCoord2 == 0;
        var isVertical = yCoord - yCoord2 == 0;
        if (isHorizontal || isVertical)
        {
            var conflict = coord.TryAdd((xCoord, yCoord), (xCoord2, yCoord2));
            // if (!conflict) throw new Exception("Shouldnt happen");
        }
        
        long area = (Math.Abs(xCoord - xCoord2 + 1) * (Math.Abs(yCoord - yCoord2) + 1));
        if (area > maxArea)
        {
            maxArea = area;
        }
        
    }
}
Console.WriteLine($"Part 1: {maxArea}");


long maxAreaPartTwo = 0;
for (int i = 0; i < points.Length; i++)
{
    for (int j = 0; j < points.Length; j++)
    {
        if (i == j) continue;
        var (xCoord, yCoord) = (points[i][0], points[i][1]);
        var (xCoord2, yCoord2) = (points[j][0], points[j][1]);

        var corner1 = (xCoord, yCoord2);
        var corner2 = (xCoord2, yCoord);

        if (coord.ContainsKey(corner1) || coord.ContainsKey(corner2))
        {
            long area = (Math.Abs(xCoord - xCoord2 + 1) * (Math.Abs(yCoord - yCoord2) + 1));
            if (area > maxAreaPartTwo)
            {
                Console.WriteLine("Found area");
                Console.WriteLine($"{xCoord}, {yCoord}");
                Console.WriteLine($"{xCoord2}, {yCoord2}");
                maxAreaPartTwo = area;
            }
        }
    }
}

Console.WriteLine($"Part 2: {maxAreaPartTwo}");

