var inputData = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sample.txt"));
// var inputData = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "day9.txt"));

var points = inputData.Select(line => line.Split(",")).Select(x
    => new Point { x = long.Parse(x[0]), y = long.Parse(x[1]) }).ToArray();

List<Line> lines = [];

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
    lines.Add(new Line { PointOne = currPoint, PointTwo = closestHorizontal});
    lines.Add(new Line { PointOne = currPoint, PointTwo = closestVertical});
}

Console.WriteLine($"Part 1: {maxArea}");


long maxAreaForPartTwo = 0;
for (int i = 0; i < points.Length; i++)
{
    var currPoint = points[i];
    for (int j = 0; j < points.Length; j++)
    {
        if (i == j) continue;
        var toPoint = points[j];
        var rect = new Rectangle(currPoint, toPoint);
        bool doesIntersect = false;
        foreach (var line in lines)
        {
            if (rect.DoesExactIntersect(line)) continue;

            if (rect.DoesIntersect(line) || line.DoesIntersect(rect.DiagonalLine)) ;
            {
                doesIntersect = true;
                break;
            }
        }
        if (doesIntersect) continue;
        
        long area = (Math.Abs(toPoint.x - currPoint.x + 1) * (Math.Abs(toPoint.y - currPoint.y) + 1));
        if (area > maxAreaForPartTwo)
        {
            maxAreaForPartTwo = area;
        }
    }
}


Console.WriteLine($"Part 2: {maxAreaForPartTwo}");


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
    
    public bool IsHorizontal => (PointOne.y == PointTwo.y); 
    public bool IsVertical => (PointOne.x == PointTwo.x); 
    
    public long LowerY => PointOne.y > PointTwo.y ? PointTwo.y : PointOne.y;
    public long UpperY => PointOne.y <= PointTwo.y ? PointTwo.y : PointOne.y;
    public long LowerX => PointOne.x > PointTwo.x ? PointTwo.x : PointOne.x;
    public long UpperX => PointOne.x <= PointTwo.x ? PointTwo.x : PointOne.x;
        
    public double Gradient  =>  (PointOne.x == PointTwo.x) 
        ? 0 
        : (double)(PointOne.y - PointTwo.y) / (PointOne.x - PointTwo.x);
    
    // y - PointOne.y = Gradient * (x - PointOne.x)
    // y = Gradient * x - PointOne.x * Gradient + PointOne.y
    
    // Gradient * x - Point.x * Gradient + PointOne.y = Gradient * x - Point.x * Gradient + PointOne.y
    public bool DoesIntersect(Line other)
    {
        var intersectionX  =
            (PointOne.x * Gradient + PointOne.y - (other.PointOne.x * other.Gradient + other.PointOne.y))/ (Gradient - other.Gradient);
        var intersectionY  = Gradient * intersectionX - PointOne.x * Gradient + PointOne.y;
        
         
        var otherIntersectionY = intersectionX * other.Gradient - other.PointOne.x * other.Gradient + other.PointOne.y;
        return Math.Abs(otherIntersectionY - intersectionY) < 10e-5;
    }


    public bool DoesAxisIntersect(Line other)
    {
        if (CoordinatesMatching(other)) return false;
        if (IsHorizontal && other.IsHorizontal) return false;
        if (IsVertical && other.IsVertical) return false;

        if (IsVertical)
        {
            return other.PointOne.y > LowerY && other.PointOne.y < UpperY;
            // && (UpperX != other.PointOne.x && UpperX != other.PointTwo.x);
        }

        return other.PointOne.x > LowerX && other.PointOne.x < UpperX;
        // && (UpperY != other.PointOne.y && UpperY != other.PointTwo.y);
    }
    
    public bool CoordinatesMatching(Line other)
    {
        return PointOne == other.PointOne || PointOne == other.PointTwo  || PointTwo == other.PointOne|| PointTwo == other.PointTwo;
    }
}


public record Rectangle
{
    // assumes diagonally opposite
    public Rectangle(Point pointOne, Point pointTwo)
    {
        var otherCorner = new Point { x = pointOne.x, y = pointTwo.y };
        var otherCornerTwo = new Point { x = pointTwo.x, y = pointOne.y };
        
        Lines = [
            new Line{ PointOne = pointOne, PointTwo = otherCorner},
            new Line{ PointOne = otherCorner, PointTwo = pointTwo},
            new Line{ PointOne = pointTwo, PointTwo = otherCornerTwo},
            new Line{ PointOne = otherCornerTwo, PointTwo = pointOne},
        ];
        Corners = [pointOne, otherCorner, pointTwo, otherCornerTwo];
        DiagonalLine = new Line { PointOne = pointOne, PointTwo = pointTwo };
    }
    
    public Line DiagonalLine { get; set; }
    public Line[] Lines { get; set; }
    public Point[] Corners { get; set; }

    public bool DoesIntersect(Line line)
    {
        return Lines.Any(x => x.DoesAxisIntersect(line));
    }

    public bool DoesExactIntersect(Line line)
    {
        return Lines.Any(x => x.CoordinatesMatching(line));
    }
    
}
