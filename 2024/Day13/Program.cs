// See https://aka.ms/new-console-template for more information

using System.Text.RegularExpressions;

var data = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "day13.txt"));

var parsedData = data.Where(x => x != "").Chunk(3).ToArray();



long FindFewestTokens(string[][] input, long offset = 0)
{
    long fewestTokens = 0;
    foreach (var single in parsedData)
    {
        var buttonA = single[0];
        var buttonB = single[1];
        var prize = single[2];

        var parsingButtonA = Regex.Matches(buttonA, @".*: X\+(\d+), Y\+(\d+)").First();
        var (aX, aY) = (long.Parse(parsingButtonA.Groups[1].ToString()), long.Parse(parsingButtonA.Groups[2].ToString()));
        var parsingButtonB = Regex.Matches(buttonB, @".*: X\+(\d+), Y\+(\d+)").First();
        var (bX, bY) = (long.Parse(parsingButtonB.Groups[1].ToString()), long.Parse(parsingButtonB.Groups[2].ToString()));
        var parsingPrize = Regex.Matches(prize, @".*: X=(\d+), Y=(\d+)").First();
        var (pX, pY) = (long.Parse(parsingPrize.Groups[1].ToString()) + offset, long.Parse(parsingPrize.Groups[2].ToString()) + offset);

        long[] buttonPressMat = [aX, bX, aY, bY];
        var factor = Math.Pow((buttonPressMat[0] * buttonPressMat[3]) - (buttonPressMat[1] * buttonPressMat[2]), -1);
        long[] inverseMat = [bY, -bX, -aY, aX];
        var inverseButtonPress = inverseMat.Select(x => factor * x).ToArray();
        var buttonAPresses = inverseButtonPress[0] * pX + inverseButtonPress[1] * pY;
        var buttonBPresses = inverseButtonPress[2] * pX + inverseButtonPress[3] * pY;

        if (Math.Abs(Math.Round(buttonAPresses, 1) - buttonAPresses) < 0.001 && Math.Abs(Math.Round(buttonBPresses, 1) - buttonBPresses) < 0.001)
        {
            var cost = 3*buttonAPresses + buttonBPresses;
            fewestTokens += (long)Math.Round(cost, 0);
        }
    }
    return fewestTokens;
}


Console.WriteLine($"Part 1: {FindFewestTokens(parsedData)}");
Console.WriteLine($"Part 2: {FindFewestTokens(parsedData, 10000000000000)}");