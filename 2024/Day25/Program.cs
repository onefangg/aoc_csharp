// See https://aka.ms/new-console-template for more information

var data  = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "day25.txt"));


List<(int[], bool)> keyLocks = [];


var l = new int[data[0].Length];
for (int idx = 0; idx < data.Length; idx++)
{
    var line = data[idx];
    if (line.Length == 0)
    {
        var isLock = data[idx - 1].All(x => x == '#');
        keyLocks.Add((l.Select(x => x - 1).ToArray(), isLock));
        l = new int[data[0].Length];
        continue;
    }
    
    for (int i = 0; i < line.Length; i++)
    {
        if (line[i] == '#')
        {
            l[i] += 1;
        } 
    }
}
keyLocks.Add((l.Select(x => x - 1).ToArray(), data[^1].All(x => x == '#')));

var keys = keyLocks.Where(x =>!x.Item2).Select(x => x.Item1).ToArray();
var locks = keyLocks.Where(x =>x.Item2).Select(x => x.Item1).ToArray();


var cnt = 0;
foreach (var k in keys)
{
    foreach (var t in locks)
    {

        var doesOverlap = k.Zip(t, (a, b) => a + b).Any(x => x>=6);
        if (!doesOverlap) cnt++;

    }
}

Console.WriteLine(cnt);