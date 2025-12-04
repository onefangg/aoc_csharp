// var inputData = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sample.txt"));
var inputData = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "day4.txt"));


(int, int)[] directions = [
    (-1,  -1), // top diagonal left
    (-1,  0), // top
    (-1,  1), // top diagonal right
    (0, -1), // left
    (0, 1), // right
    (1, -1), // btm diagonal left
    (1, 0), // btm
    (1, 1), // btm diagonal right
];

int GetPartOne(string[] data)
{
    var iter = 0;
    for (int r = 0; r < data.Length; r++)
    {
        for (int c = 0; c < data[r].Length; c++)
        {
            if  (data[r][c] != '@') continue; 
        
            var cnt = 0;
            foreach (var (dr, dc) in directions)
            {
                var (rr, cc) = (r + dr, c +dc);
                if (rr < 0 || rr >= data.Length || cc < 0 || cc >= data[c].Length) continue;
                if (data[rr][cc] == '@') cnt++;
            }
            if (cnt < 4) iter++;
        }
    }

    return iter;
}


int GetPartTwo(string[] data)
{
    var iter = 0;
    var clone = data.Select(a => a.ToArray()).ToArray();

    while (true)
    {
        var loop = iter;
        for (int r = 0; r < clone.Length; r++)
        {
            for (int c = 0; c < clone[r].Length; c++)
            {
                if (clone[r][c] != '@') continue;

                var cnt = 0;
                foreach (var (dr, dc) in directions)
                {
                    var (rr, cc) = (r + dr, c + dc);
                    if (rr < 0 || rr >= clone.Length || cc < 0 || cc >= clone[c].Length) continue;
                    if (clone[rr][cc] == '@') cnt++;
                }

                if (cnt < 4)
                {
                    clone[r][c] = '.';
                    iter++;
                }
            }
        }
        if (loop == iter) break;
    }

    return iter;
}


var partOne = GetPartOne(inputData);
var partTwo = GetPartTwo(inputData);
Console.WriteLine($"Part One: {partOne}");
Console.WriteLine($"Part Two: {partTwo}");