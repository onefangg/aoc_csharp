var data = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "day18.txt"));

var (startX, startY) = (0, 0);
// testing sample
// var (endX, endY) = (6, 6);
// var nIterations = 12;

var (endX, endY) = (70, 70);
var nIterations = 1024;

var fallingBytes = data.Select(x =>x.Split(",").Select(int.Parse).ToArray()).ToArray();
(int, bool) RunDfs(int[][] inputFalling, int widthInclusive, int heightInclusive, int takeUntil)
{
    var gridData = CreateGridAfterN(inputFalling, widthInclusive, heightInclusive, takeUntil);
    var dist = CreatDistanceGrid(widthInclusive, heightInclusive);
    var visited = CreateVisitedGrid(widthInclusive, heightInclusive);
    dist[startX][startY] = 0;
    visited[startX][startY] = true;

    var q = new Queue<(int, int)>([(startX, startY)]);
    while (q.Count > 0)
    {
        var curr = q.Dequeue();
    
        (int, int)[] directions = [(-1, 0), (0, -1), (1,0), (0, 1)];
        var currDist = dist[curr.Item1][curr.Item2];
        if (curr.Item1 == widthInclusive && curr.Item2 == endY)
        {
            break;
        }
    
        foreach (var dir in directions)
        {
            try
            {
                var nextX = curr.Item1 + dir.Item1;
                var nextY = curr.Item2 + dir.Item2;

                if (!visited[nextX][nextY] && gridData[nextX][nextY] == '.')
                {
                    visited[nextX][nextY] = true;
                    dist[nextX][nextY] = currDist + 1;
                    q.Enqueue((nextX, nextY));
                }
            }
            catch
            {
            }
        }
    }
    return (dist[widthInclusive][heightInclusive], visited[widthInclusive][heightInclusive]);
}

Console.WriteLine($"Part 1: {RunDfs(fallingBytes, endX, endY, nIterations).Item1}");
var idx = nIterations + 1;
while (idx < fallingBytes.Length)
{
    var (_,  isNotBlocked) = RunDfs(fallingBytes, endX, endY, idx);

    if (!isNotBlocked)
    {
        Console.WriteLine($"Part 2: {fallingBytes[idx-1][0]},{fallingBytes[idx-1][1]}");
        break;
    }

    idx++;
}

bool[][] CreateVisitedGrid(int widthInclusive, int heightInclusive)
{
    var grid = new bool[heightInclusive+1][];
    for (var i = 0; i <= heightInclusive; i++)
    {
        grid[i] = new bool[widthInclusive+1];
        for (int j = 0; j <= widthInclusive; j++)
        {
            grid[i][j] = false;
        }
    }
    return grid;
}

int[][] CreatDistanceGrid(int widthInclusive, int heightInclusive)
{
    var grid = new int[heightInclusive+1][];
    for (var i = 0; i <= heightInclusive; i++)
    {
        grid[i] = new int[widthInclusive+1];
        for (int j = 0; j <= widthInclusive; j++)
        {
            grid[i][j] = Int32.MaxValue;
        }
    }
    return grid;
}

char[][] CreateGridAfterN(int[][] falling, int widthInclusive, int heightInclusive, int n)
{
    var grid = new char[heightInclusive+1][];
    for (var i = 0; i <= heightInclusive; i++)
    {
        grid[i] = new char[widthInclusive+1];
        for (int j = 0; j <= widthInclusive; j++)
        {
            grid[i][j] = '.';
        }
    }
    var takeForFalling = falling.Take(n).ToArray();

    foreach (var row in takeForFalling)
    {
        var (x, y) = (row[0], row[1]);
        grid[x][y] = '#';
    }
    return grid;
}


