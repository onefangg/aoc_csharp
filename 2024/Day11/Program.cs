// See https://aka.ms/new-console-template for more information

var data = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "day11.txt"))[0];
var stones = data.Split().Select(long.Parse).ToList();

long[] BlinkStone(long stone)
{
    if (stone == 0) return [1];
    var parseToStoneStr = stone.ToString();
    if (parseToStoneStr.Length % 2 == 0) return [long.Parse(parseToStoneStr[..(parseToStoneStr.Length / 2)]),long.Parse(parseToStoneStr[(parseToStoneStr.Length / 2)..])];
    return [stone * 2024];
}

var stoneLookUp = new Dictionary<(long, long), long>();
long RecursionBlinkStone(long stone, long iterations)
{
    
    if (iterations <= 0) return 1;
    if (stoneLookUp.TryGetValue((stone,iterations), out var blinkStone)) return blinkStone;
    if (stone == 0) return RecursionBlinkStone(1, iterations-1);
    
    var nDigits = Math.Floor(Math.Log10(stone)) + 1;
    if (nDigits % 2 == 0)
    {
        
        var res = RecursionBlinkStone( (long)Math.Floor(stone / Math.Pow(10, nDigits/2)), iterations-1) +
                   RecursionBlinkStone((long)Math.Floor(stone % Math.Pow(10, nDigits/2)), iterations-1);
        stoneLookUp[(stone,iterations)] = res;
        return res;
    }

    return RecursionBlinkStone(stone*2024, iterations-1);
}

var applyCountsToStones = stones.Select(x => RecursionBlinkStone(x, 75)).Sum();
Console.WriteLine(applyCountsToStones);