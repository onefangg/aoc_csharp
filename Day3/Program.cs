var inputData = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sample.txt"));
// var inputData = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "day3.txt"));


int SolveForTwoBatteries(string[] data)
{
    var runningSum = 0;
    foreach (var line in data)
    {
        var x = int.Parse(line[0].ToString()) * 10;
        int y = 0;
    
        for (int i = 1; i < line.Length; i++)
        {
            var curr = x + y;
            var now = int.Parse(line[i].ToString());

            if (now * 10 > curr && i != line.Length - 1)
            {
                x = now * 10;
                y = 0;
                continue;
            }
            if (x + now > curr)
            {
                y = now;
            }
        }

        runningSum += x + y;
    }
    return runningSum;
}


long FindLargestNumberForBase(string num, int searchPath = 12)
{
    var maxLen = num.Length;
    List<int> digits = [];

    var l = 0;
    var r = maxLen - searchPath - 1;
    for (int i = 0; i < searchPath; i++)
    {
        var curr = int.Parse(num[l].ToString());
        var currIdx = l;

        for (int j = l; j <= r; j++)
        {
            var parsed = int.Parse(num[j].ToString());
            if (parsed > curr)
            {
                curr = parsed;
                currIdx = j;
            }
        }
        // Console.WriteLine($"Yoinked index: {currIdx} for value {curr}");

        // Console.WriteLine($"{l} to {r}");        
        digits.Add(curr);
        l = currIdx + 1;
        r = maxLen - 1 - (searchPath -  digits.Count -1);

    }


    return long.Parse(String.Join("", digits));
}


long SolveForTwelveBatteries(string[] data)
{
    long runningSum = 0;
    foreach (var line in data)
    {
        var calc = FindLargestNumberForBase(line);
        runningSum += calc;
    }
    return runningSum;
}

Console.WriteLine($"Part 1: {SolveForTwoBatteries(inputData)}");
Console.WriteLine($"Part 2: {SolveForTwelveBatteries(inputData)}");
Console.WriteLine($"Part 2: {SolveForTwelveBatteries(["318919111111192345"])}");

