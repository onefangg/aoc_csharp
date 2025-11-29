// See https://aka.ms/new-console-template for more information

var data = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "day12.txt"));
var gridData = data.Select(r => r.ToCharArray()).ToArray();
var visited = new HashSet<(int, int)>();

var res = 0;
var part2res = 0;
for (int r = 0; r < gridData.Length; r++)
{
    for (int c = 0; c < gridData[r].Length; c++)
    {
        var isVisted = visited.Add((r, c));
        if (!isVisted) continue;

        var currEle = gridData[r][c];
        Queue<(int, int)> getCurrEleNeighbours = new Queue<(int, int)>
        (
            [
                (r - 1, c),
                (r + 1, c),
                (r, c - 1),
                (r, c + 1)
            ]
        );
        var samePlot = new List<(int, int)>() { (r, c) };
        while (getCurrEleNeighbours.Count > 0)
        {
            var (popRow, popCol) = getCurrEleNeighbours.Dequeue();
            try
            {
                var getNeighbour = gridData[popRow][popCol];
                if (getNeighbour == currEle && !visited.Contains((popRow, popCol)))
                {
                    visited.Add((popRow, popCol));
                    samePlot.Add((popRow, popCol));
                    (int, int)[] popNeighbours =
                    [
                        (popRow - 1, popCol),
                        (popRow + 1, popCol),
                        (popRow, popCol - 1),
                        (popRow, popCol + 1),
                    ];
                    Array.ForEach(popNeighbours.Where(x => x.Item1 >= 0 && x.Item2 >= 0).ToArray(),
                        e => { getCurrEleNeighbours.Enqueue(e); });
                }
            }
            catch
            {
                // ignored
            }
        }
        part2res += CalculatePricePart2(samePlot);
        res += CalculatePrice(samePlot);
    }
}

Console.WriteLine($"Part 1: {res}");
Console.WriteLine($"Part 2: {part2res}");


int CalculatePricePart2(List<(int, int)> region)
{
    var visitedRegion = new HashSet<(int, int)>(region);
    var vistedPerimeter = new HashSet<(int, int, int, int)>();
    
    foreach (var plot in region)
    {
        (int, int)[] surr =
        [
            (-1, 0), // up
            (1, 0), // down
            (0, -1), // left
            (0, 1), // right
        ];

        foreach (var offset in surr)
        {
            var (dr, dc) = offset;
            var (paddingRow, paddingCol) = plot;
            if (visitedRegion.Contains((paddingRow+dr, paddingCol+dc)))
            {
                continue;
            }
            // code adapted (yoinked) from u/SuperSmurfen https://github.com/AxlLind/AdventOfCode2024/blob/main/src/bin/12.rs
            // because i couldn't figure out how to walk perimeters without overthinking
            // thinking through the code:
            // if offset = (-1, 0) => going up
            // if there is plot above, then no point traversing
            // if there is no plot above, stay at row-axis and move col-axis in direction (left)  
            //      if you hit a plot => that's a corner
            while (visitedRegion.Contains((paddingRow + dc, paddingCol + dr))
                   && !visitedRegion.Contains((paddingRow + dr, paddingCol + dc)) )
            {
                paddingRow += dc;
                paddingCol += dr;
            }

            vistedPerimeter.Add((paddingRow, paddingCol, dr, dc));
        }
    }
    return region.Count * vistedPerimeter.Count;

}

int CalculatePrice(List<(int, int)> neighbours)
{
    int area = neighbours.Count;
    var perimeter = 0;
    foreach (var neighbour in neighbours)
    {
        var (r, c) = neighbour;
        (int, int)[] numberOfNeighbours =
        [
            (r - 1, c),
            (r + 1, c),
            (r, c + 1),
            (r, c - 1),
        ];
        var adjacentCount = neighbours.Intersect(numberOfNeighbours).Count();
        perimeter += 4 - adjacentCount;
    }

    return area * perimeter;
}
