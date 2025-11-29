// See https://aka.ms/new-console-template for more information

var data = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "day1_sample.txt"));
var (left, right) = ParsePuzzleInput(data);
(int[], int[]) ParsePuzzleInput(string[] inputData)
{
    var firstPass = inputData.Select(line =>
    {
        var leftAndRight = line.Split().Where(x => x != String.Empty).ToArray();
        return (Int32.Parse(leftAndRight[0].Trim()), Int32.Parse(leftAndRight[1].Trim()));
    }).ToArray();
    return (firstPass.Select(x => x.Item1).ToArray(),
        firstPass.Select(x => x.Item2).ToArray());
}

Console.WriteLine($"Part 1 solution: {left.Order().Zip(right.Order(), (l,r) => Math.Abs(l - r)).Sum()}");

var lookUp = right.Select(n => (n, 1)).GroupBy(x => x.Item1).ToDictionary(k => k.Key, v => v.Select(x=>x.Item2).Sum());

Console.WriteLine($"Part 2 solution: {left.Select(x => x * (lookUp.GetValueOrDefault(x,0))).Sum()}");
