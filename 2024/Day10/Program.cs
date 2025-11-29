
var data = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "day10.txt"));
var parsedData = data.Select(x => x.ToCharArray().Select(x => (int) Char.GetNumericValue(x)).ToArray()).ToArray();

var allTrailHeads = new List<(int,int)>();
for (int i = 0; i < parsedData.Length; i++)
{
    for (int j = 0; j < parsedData[i].Length; j++)
    {
        if (parsedData[i][j] == 0) allTrailHeads.Add((i, j));
    }
}

var res = 0;


foreach (var trailHead in allTrailHeads)
{
    var iteratedPoints = new HashSet<(int, int)>();
    var neighbours = new Queue<(int, int)>();
    neighbours.Enqueue((trailHead.Item1, trailHead.Item2));
    while (neighbours.Count > 0)
    {
        
        var (currRow, currCol) = neighbours.Dequeue();
        // var notIterated = iteratedPoints.Add((currRow, currCol));
        // if (!notIterated)
        // {
        //     continue;
        // }
        
        var currEle = parsedData[currRow][currCol];
        if (currEle == 9)
        {
            // Console.WriteLine($"Found 9: {currRow},{currCol}");
            res++;
            continue;
        };
        
        (int, int)[] neighboursIndex = [
            (currRow - 1, currCol),
            (currRow + 1, currCol),
            (currRow, currCol - 1),
            (currRow, currCol + 1),
        ];
        foreach (var (nRow, nCol) in neighboursIndex)
        {
            try
            {
                var neighbourEle = parsedData[nRow][nCol];
                if ((neighbourEle - currEle) == 1)
                {
                    neighbours.Enqueue((nRow, nCol));
                }
            }
            catch
            {
                // ignored
            }
        }
        
    }

    
}


Console.WriteLine(res);