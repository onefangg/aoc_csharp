// See https://aka.ms/new-console-template for more information

var data = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "day20.txt"));
var gridData = data.Select(r => r.ToCharArray()).ToArray();
(int,int)[] directions = [(-1, 0), (0, -1), (1, 0), (0, 1)];
(int, int) start = (0,0);
(int, int) end  = (0,0);
int height = gridData.Length;
int width = gridData[0].Length;
bool[][] visited = new bool[gridData.Length][];

for (int i = 0; i < gridData.Length; i++)
{
    var visitedRow = new bool[gridData[i].Length];
    for (int j = 0; j < gridData[i].Length; j++)
    {
        if (gridData[i][j] == 'S')
        {
            start = (i, j);
        } else if (gridData[i][j] == 'E')
        {
            end = (i, j);
        }
        visitedRow[j]= false;
    }
    visited[i] = visitedRow;
}

var orderedPath = new List<(int, int)>();
var p1Shortcuts = new HashSet<((int, int),  (int, int))>();
var q = new Queue<(int,int)>([start]);

while (q.Count > 0)
{
    var (r, c) = q.Dequeue();
    visited[r][c] = true;
    foreach (var d in directions)
    {
        var (dr, dc) = d;
        var rr = r + dr;
        var cc = c + dc;
        if (visited[rr][cc] == false && gridData[rr][cc] != '#')
        {
            q.Enqueue((rr, cc));
            orderedPath.Add((rr, cc));
        } else if (gridData[rr][cc] == '#')
        {
            var drr = rr + dr;
            var dcc = cc + dc;
            if (drr >= 0 && drr < height && dcc >= 0 && dcc < width && gridData[drr][dcc] != '#' && !visited[drr][dcc])
            {
                p1Shortcuts.Add(((r,c),(drr,dcc)));
            }
        } 
    }
}
var p1SavingsLookup = GetCheatSavings(orderedPath, p1Shortcuts);
var res= p1SavingsLookup.Where(x => x.Key >= 100).Sum(x => x.Value);
Console.WriteLine($"Part 1: {res}");

// part 2 - idea from: https://publish.reddit.com/embed?url=https://www.reddit.com/r/adventofcode/comments/1hicdtb/comment/m2xzyfs/ 
var p2Q = new Queue<((int, int),int)>([(start, 0)]);
var distanceMap = new Dictionary<(int, int), int>();
while (p2Q.Count > 0)
{
    var (curr, n) = p2Q.Dequeue();
    if (!distanceMap.TryAdd(curr, n))
    {
        continue;
    }
    var (r, c) = curr;
    foreach (var dir in directions)
    {
        var (dr, dc) = dir;
        var (rr, cc) = (r + dr, dc + c);
        if (gridData[rr][cc] != '#')
        {
            p2Q.Enqueue(((rr,cc), n+1));
        }
    }
}

var p2Res = 0;
var flattenMap = distanceMap.Select(x => (x.Key.Item1, x.Key.Item2, x.Value)).ToArray();

for (int i = 0; i < flattenMap.Length-1; i++)
{
    for (int j = i + 1; j < flattenMap.Length; j++)
    {
        var (r1, c1, d1) = (flattenMap[i].Item1, flattenMap[i].Item2, flattenMap[i].Item3);
        var (r2, c2, d2) = (flattenMap[j].Item1, flattenMap[j].Item2, flattenMap[j].Item3);
        
        var cheatPath = Math.Abs(r1 - r2) + Math.Abs(c1 - c2);
        var path = Math.Abs(d2 - d1);
        if (cheatPath <= 20 && cheatPath + 100 <= path)
        {
            p2Res++;
        }
    }
}


Console.WriteLine($"Part 2: {p2Res}");

Dictionary<int, int> GetCheatSavings(List<(int, int)> path, HashSet<((int, int), (int, int))> cheats)
{
    var counts = new Dictionary<int, int>();

    foreach (var c in cheats)
    {
        var (anchorStart,  anchorEnd) = c;
        var firstStep = path.FindIndex(x => x == anchorStart) ;
        var reenterStep = path.FindIndex(x => x == anchorEnd);
        var savings = reenterStep - firstStep - 2;
        if (!counts.TryAdd(savings, 1))
        {
            counts[savings]++;
        }
    }
    return counts;
}

