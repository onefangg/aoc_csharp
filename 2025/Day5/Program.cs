// var inputData = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sample.txt")).ToList();
var inputData = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "day5.txt")).ToList();

var splitIndex = inputData.FindIndex(x => x == "");
var parseRanges = inputData.Take(splitIndex)
    .Select(x =>
    {
        var split = x.Split("-");
        var from = long.Parse(split[0]);
        var to = long.Parse(split[1]);
        return (from, to);
    }).ToArray();
var parsedIngredients = inputData.Skip(splitIndex + 1).Select(long.Parse).ToArray();


long NaiveCountFreshIngredients(long[] ingredients, (long, long)[] ranges)
{
    long running = 0;
    foreach (var ingre in ingredients)
    {
        foreach (var (s, e) in ranges)
        {
            if (ingre >= s && ingre <= e)
            {
                running++;
                break;
            }
        }
    }

    return running;
}

long CountFresh((long, long)[] ranges)
{
    
    // thank you geeks for geeks >:)
    List<(long,long)> diffSets = ranges.OrderBy(x => x.Item1).ToList();
    List<(long,long)> disjointSets = [];

    for (int i = 0; i < diffSets.Count; i++)
    {
        var (s, e) = diffSets[i];

        if (disjointSets.Count > 0 && disjointSets[^1].Item2 >= e) continue;
        for (int j = i + 1; j < diffSets.Count; j++)
        {
            var (ds, de) = diffSets[j];
            if (ds <= e) e = Math.Max(e, de);
        }
        disjointSets.Add((s, e));
    }
    
    
    
    return disjointSets.Select(a => a.Item2 - a.Item1 +1).Sum();
}


Console.WriteLine($"Part 1: {NaiveCountFreshIngredients(parsedIngredients, parseRanges)}");
Console.WriteLine($"Part 2: {CountFresh(parseRanges)}");