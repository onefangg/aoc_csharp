// See https://aka.ms/new-console-template for more information

var data = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "day2.txt"));
var parsedInput = data.Select(x => x.Split().Select(v => Int32.Parse(v)).ToArray()).ToArray();

var safeReports = parsedInput.Where(report =>
{
    var shiftRight = report.Take(report.Length - 1).ToArray();
    var diff = report.Skip(1).Zip(shiftRight, (l, r) => r-l).ToArray();
    return diff.All(x => Math.Abs(x) < 4 && Math.Abs(x) > 0)
            && (diff.All(x => x < 0) || diff.All(x => x > 0));
}).ToArray();

Console.WriteLine($"Part 1 Input is: {safeReports.Length}");

var remainingSafeReports = parsedInput
    .Where(report =>
    {
        var shiftRight = report.Take(report.Length - 1).ToArray();
        var diff = report.Skip(1).Zip(shiftRight, (l, r) => r-l).ToArray();
        return !(diff.All(x => Math.Abs(x) < 4 && Math.Abs(x) > 0)
                 && (diff.All(x => x < 0) || diff.All(x => x > 0)));
    }).Count(report => Enumerable.Range(0, report.Length).Any(idx =>
        {
            var excludeLevels = report.Where((l, i) => idx != i).ToArray();
            var shiftRightAgain = excludeLevels.Take(excludeLevels.Length - 1).ToArray();
            var diffAgain = excludeLevels.Skip(1).Zip(shiftRightAgain, (l, r) => r-l).ToArray();
            return diffAgain.All(x => Math.Abs(x) < 4 && Math.Abs(x) > 0)
                   && (diffAgain.All(x => x < 0) || diffAgain.All(x => x > 0));
        }
    ));

Console.WriteLine($"Part 2 Input is: {remainingSafeReports + safeReports.Length}");