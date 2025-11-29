// See https://aka.ms/new-console-template for more information

var data = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "day19.txt")).Where(s => !string.IsNullOrWhiteSpace(s)).ToArray();

var availTowels = data[0].Split(", ").ToArray();
var desiredPatterns = data[1..];

var initCache = new Dictionary<string, long>();
long cnt = 0;
foreach (var design in desiredPatterns)
{
    cnt += RecurseForPossibleDesign(design, availTowels, initCache);
    
}

Console.WriteLine(cnt);
// var cnt = 0;
// foreach (var design in desiredPatterns)
// {
//     var iterateThrough = new SortedSet<string>(Comparer<string>.Create((a,b)=>a.Length-b.Length)) { design };
//
//     while (iterateThrough.Count > 0)
//     {
//         var matchAgainst = iterateThrough.Min()!;
//         iterateThrough.Remove(matchAgainst);
//         
//         if (matchAgainst == "")
//         {
//             cnt++;
//             break;
//         }
//         foreach (var towel in availTowels)
//         {
//             if (matchAgainst.StartsWith(towel))
//             {
//                 iterateThrough.Add(matchAgainst[towel.Length..]);
//             }
//         }
//
//     }
// }

long RecurseForPossibleDesign(string design, string[] towels, Dictionary<string, long> cache)
{
    if (cache.TryGetValue(design, out var possibleDesign)) return possibleDesign;
    if (design == string.Empty) return 1;

    long sum = 0;
    for (long i = 0; i < towels.Length; i++)
    {
        var towel = towels[i];
        if (towel.Length > design.Length)
        {
            continue;
        }

        if (design.StartsWith(towel))
        {
            sum += RecurseForPossibleDesign(design[towel.Length..], towels, cache);
        }
    }
    cache.Add(design, sum);
    return sum;
}
