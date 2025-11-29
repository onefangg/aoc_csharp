// See https://aka.ms/new-console-template for more information

using System.Text.RegularExpressions;

var data = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "day3.txt"))[0];
var partOneRes = Regex.Matches(data, @"mul\((\d+)\,(\d+)\)").Select(x => 
    Int32.Parse(x.Groups[1].ToString()) * Int32.Parse(x.Groups[2].ToString())).Sum();
    
Console.WriteLine($"Part 1 Result: {partOneRes}");

var partTwoRes = Regex.Matches(data, @"(mul\((\d+)\,(\d+)\))|do\(\)|don't\(\)")
    .Select(x => x.Groups[0].ToString())
    .ToArray();

var shouldMul = true; // default
var runningRes = 0;
foreach (var parse in partTwoRes)
{
    if (parse.StartsWith("mul") && shouldMul)
    {
        var parseToMul = Regex.Match(parse, @"mul\((\d+)\,(\d+)\)");
        runningRes += Int32.Parse(parseToMul.Groups[1].ToString()) * Int32.Parse(parseToMul.Groups[2].ToString());
        continue;
    }
    if (parse == "do()") shouldMul = true;
    if (parse == "don't()") shouldMul = false;
}

Console.WriteLine($"{runningRes}");