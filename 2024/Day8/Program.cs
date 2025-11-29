// See https://aka.ms/new-console-template for more information

var data = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "day8.txt"));
var gridData = data.Select(x => x.ToCharArray()).ToArray();
var deriveAntennaCoords = new List<(int, int)>();
var deriveAntennaLookup = new Dictionary<char, List<(int, int)>>();
var width = gridData[0].Length;
var height = gridData.Length;

// find the coords of all the antennas
for (var row = 0; row < gridData.Length; row++)
{
    for (var col = 0; col < gridData[row].Length; col++)
    {
        var element = gridData[row][col];
        if (element != '.')
        {
            deriveAntennaCoords.Add((row, col));
            if (!deriveAntennaLookup.ContainsKey(element))
            {
                deriveAntennaLookup[element] = [(row, col)];
            }

            deriveAntennaLookup[element].Add((row, col));
        }
    }
}


int GetAntinodeSpawn(char[][] grid, List<(int, int)> antennaCoords, Dictionary<char, List<(int, int)>> antennaLookup,
    bool isForPartTwo = false)
{
    var antinodes = new HashSet<(int, int)>();
    while (antennaCoords.Count > 0)
    {
        var (currRow, currCol) = antennaCoords[0];
        var currentAntenna = grid[currRow][currCol];
        antennaCoords.RemoveAt(0);
        var antinodesSpawn = antennaLookup[currentAntenna]
            .SelectMany<(int, int), (int, int)>(coord =>
            {
                var (row, col) = coord;
                if (row == currRow && col == currCol) return Array.Empty<(int, int)>();
                var offsetRow = row - currRow;
                var offsetCol = col - currCol;
                var addToList = new List<(int, int)>();

                var maxfreq = isForPartTwo ? Math.Abs(height / offsetRow) : 1;
                if (isForPartTwo)
                {
                    addToList.Add(coord);
                    addToList.Add((currRow, currCol));
                }
                // doesn't matter because height == width, and you'll filter it out anyhow
                for (int f = 1; f <= maxfreq; f++)
                {
                    addToList.AddRange([
                        (row + f * offsetRow, col + f * offsetCol), (currRow + f * -offsetRow, currCol + f * -offsetCol)
                    ]);
                }

                return addToList;
            }).Where(coord => (coord.Item1 >= 0 && coord.Item1 < height) && (coord.Item2 >= 0 && coord.Item2 < width))
            .ToArray();
        antennaLookup[currentAntenna].Remove((currRow, currCol));
        Array.ForEach(antinodesSpawn, antinode => { antinodes.Add(antinode); });
    }

    return antinodes.Count;
}

// var partOneSolution = GetAntinodeSpawn(gridData, deriveAntennaCoords, deriveAntennaLookup);
var partTwoSolution = GetAntinodeSpawn(gridData,deriveAntennaCoords , deriveAntennaLookup, true);
// Console.WriteLine($"Part 1: {partOneSolution}");
Console.WriteLine($"Part 2: {partTwoSolution}");