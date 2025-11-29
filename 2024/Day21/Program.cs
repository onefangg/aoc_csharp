// See https://aka.ms/new-console-template for more information

// string[] inputCodes = ["029A", "980A", "179A","456A", "379A"];
string[] inputCodes = ["480A", "143A", "983A","382A", "974A"];
// string[] inputCodes = ["379A"];
// string[] inputCodes = ["179A","456A"];

ButtonPress[] numPads = [
    new('7', 0, 0),
    new('8', 0, 1),
    new('9', 0, 2),
    new('4', 1, 0),
    new('5', 1, 1),
    new('6', 1, 2),
    new('1', 2, 0),
    new('2', 2, 1),
    new('3', 2, 2),
    new('0', 3, 1),
    new('A', 3, 2),
];

ButtonPress[] dirPads = [
    new('^', 0, 1),
    new('A', 0, 2),
    new('<', 1, 0),
    new('v', 1, 1),
    new('>', 1, 2),
];

Direction[] DIRECTIONS = [
    new ('v', 1,0),
    new ('>', 0,1),
    new ('<', 0,-1),
    new ('^', -1,0),
];
// Direction[] DIRECTIONS = [
//     
//     new ('>', 0,1),
//     new ('v', 1,0),
//     new ('^', -1,0),
//     new ('<', 0,-1),
// ];

var shortestNumPads = GetShortestPathsForNum(numPads, new ButtonPress(' ', 3 ,0), 4, 3);
var shortestDirPads = GetShortestPathsForNum(dirPads, new ButtonPress(' ', 0 ,0), 2, 3);

Dictionary<(char, char, int), long> CACHE = new();

// Console.WriteLine(shortestNumPads.Count);
// Console.WriteLine(shortestDirPads.Count);
long p1 = 0;
foreach (var code in inputCodes)
{
    var digits = code;
    var startPress = 'A';
    long cnt = 0;
    for (int i =0; i<digits.Length; i++)
    {
        var d = digits[i];
        var calc = shortestNumPads[(startPress, d)].Select(x=>FindKeyPresses(x, 25)).ToArray();
        cnt += calc.Min();
        startPress = d;
    }

    var numPart = Int32.Parse(digits[..^1]);
    var res = numPart * cnt;
    p1 += res;
    Console.WriteLine($"Sum total for {code}: {res}");
    Console.WriteLine($"Working for {code}: {cnt} * {numPart} ");
}
Console.WriteLine($"Part 1: {p1}");

long FindKeyPresses(string path, int depth)
{
    if (depth == 0)
    {
        return path.Length;
    }
    long cnt = 0;
    path = 'A' + path;
    for (int i = 0; i < path.Length-1; i++)
    {
        if (CACHE.ContainsKey((path[i], path[i + 1], depth)))
        {
            cnt += CACHE[(path[i], path[i + 1], depth)];
        }
        else
        {
            var possiblePresses = shortestDirPads[(path[i], path[i + 1])].Select(x =>
                FindKeyPresses(x, depth - 1)).ToArray();
            CACHE.Add((path[i], path[i + 1], depth), possiblePresses.Min());
            cnt += possiblePresses.Min();
        }
    }
    return cnt;
}


Dictionary<(char, char), List<string>> GetShortestPathsForNum(ButtonPress[] inputNumPad, ButtonPress escape, int maxR = 0, int maxC = 0)
{
    // referenced a smarter way of deriving the paths from https://github.com/maneatingape/advent-of-code-rust/blob/main/src/year2024/day21.rs
    var lookup = new Dictionary<(char, char), List<string>>();
    
    var (escR, escC) = (escape.r, escape.c);

    for (int i = 0; i < inputNumPad.Length; i++)
    {
        for (int j = 0; j < inputNumPad.Length; j++)
        {
            var startPress = inputNumPad[i];
            var endPress = inputNumPad[j];
            var fromToEnd = (startPress.e, endPress.e);
            if (i==j && startPress.e == endPress.e)
            {
                if (lookup.ContainsKey(fromToEnd))
                {
                    lookup[fromToEnd].Add("A");    
                }
                else
                {
                    lookup[fromToEnd] = ["A"];
                }

                continue;
            };
            var (sr, sc) = (startPress.r, startPress.c);
            var (er, ec) = (endPress.r, endPress.c);

            var verticalOffset = Math.Abs(er - sr);
            var verticalPath = er - sr > 0 ? 'v' : '^';
            var horizontalOffset = Math.Abs(ec - sc);
            var horizontalPath = ec - sc > 0 ? '>' : '<';

            var buildMultiplePaths = new List<string>();
            
            if (!(sc == escC && er == escR))
            {
                buildMultiplePaths.Add(string.Join("", Enumerable.Repeat(verticalPath, verticalOffset)
                    .Concat(Enumerable.Repeat(horizontalPath, horizontalOffset))
                    .Concat(['A'])));
            }
            
            if (!(ec == escC && sr == escR))
            {
                buildMultiplePaths.Add(string.Join("", Enumerable.Repeat(horizontalPath, horizontalOffset)
                    .Concat(Enumerable.Repeat(verticalPath, verticalOffset))
                    .Concat(['A'])));
            }
            if (!lookup.TryAdd(fromToEnd, buildMultiplePaths))
            {
                lookup[fromToEnd].AddRange(buildMultiplePaths);
            }
        }
    }
    
    // foreach (var (k, v) in lookup)
    // {
    //     var minLen = v.Min(x => x.Length);
    //     lookup[k] = v.Where(x => x.Length <= minLen).Distinct().ToList();
    // }
    return lookup;
}

struct Direction(char d, int dr, int dc)
{
    public char d { get; set; } = d;
    public int dr { get; set; } = dr;
    public int dc { get; set; } = dc;
}

record struct ButtonPress(char e, int r, int c)
{
    public char e { get; set; } = e;
    public int r { get; set; } = r;
    public int c { get; set; } = c;
    public char prev { get; set; }
}
