// var inputData = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sample.txt"));
var inputData = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "day7.txt"));


var traversedMap = inputData.Select(x => x.ToCharArray()).ToArray();
var partOne = 0;

for (int r = 0; r < traversedMap.Length - 1; r++)
{
    for (int c = 0; c < traversedMap[r].Length; c++)
    {
        if (traversedMap[r][c] == 'S')
        {
            traversedMap[r+1][c] = '|';
        } else if (traversedMap[r][c] == '|' && traversedMap[r+1][c] == '^') 
        {
            traversedMap[r + 1][c-1] = '|';
            traversedMap[r + 1][c+1] = '|';
            partOne++;
        } else if (traversedMap[r][c] == '|' && traversedMap[r+1][c] == '.') 
        {
            traversedMap[r + 1][c] = '|';
        }
    }
}

Console.WriteLine($"Part 1: {partOne}");

// recursive solution doesn't work??
// 48989920237096
// var traverseMapForPartTwo = inputData.Select(x => x.ToCharArray()).ToArray();
// Dictionary<(int, int), long> visitedCounts = [];
// var (startY, startX) = (Enumerable.Range(0, inputData[0].Length).First(x => inputData[0][x] == 'S'), 0);
// long TraverseMap(char[][] inputMap, int posX, int posY)
// {
//     
//     if (visitedCounts.TryGetValue((posX,posY), out var cnt)) return cnt;
//     if (posX == inputMap.Length - 1) return 1L;
//
//     
//     var goDown = inputMap[posX+1][posY];
//     long result = 0;
//     if (goDown == '.')
//     {
//         result = TraverseMap(inputMap, posX + 1, posY);
//     }
//     if (goDown == '^')
//     {
//         result = TraverseMap(inputMap, posX + 1, posY + 1) + TraverseMap(inputMap, posX + 1, posY - 1);
//     }
//
//     visitedCounts[(posX, posY)] = result;
//     return result;
// }
// var partTwo = TraverseMap(traverseMapForPartTwo, startX, startY);
// Console.WriteLine($"Part 2: {partTwo}");

// Adapting this solution because i got mega fustrated that the one above didnt work:
// https://github.com/AxlLind/AdventOfCode/blob/main/2025/src/bin/07.rs
var startY = Enumerable.Range(0, inputData[0].Length).First(x => inputData[0][x] == 'S');
Dictionary<(int,int), long> beamsCount = [];
beamsCount.Add((0, startY), 1);

while (true)
{
    Dictionary<(int, int), long> newBeamSplit = [];
    foreach (var ((r,c), count) in beamsCount)
    {
        if (r == inputData.Length - 1) continue;
        
        var goDown = inputData[r + 1][c];
        if (goDown == '.')
        {
            newBeamSplit[(r + 1, c)] = newBeamSplit.GetValueOrDefault((r + 1, c)) + count;
            continue;
        }

        if (goDown == '^')
        {
            foreach (var cc in (int[])[c - 1, c + 1])
            {
                newBeamSplit[(r + 1, cc)] = newBeamSplit.GetValueOrDefault((r + 1, cc)) + count;
            }
        }
    }
    if (newBeamSplit.Count == 0 ) break;
    beamsCount = newBeamSplit;
}
// 48989920236781
Console.WriteLine($"Part 2: {beamsCount.Values.Sum()}");