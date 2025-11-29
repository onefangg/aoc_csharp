// See https://aka.ms/new-console-template for more information

using System.ComponentModel;

var data = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "day14.txt"));
var robots = new List<Robot>();

// var (width, height) = (11, 7); // hotswap this
var (width, height) = (101, 103); // hotswap this
var seconds = 100;

foreach (var line in data)
{
    var splitOne = line.Split(" ");
    var positions = splitOne[0][2..].Split(",").Select(Int32.Parse).ToArray();
    var velocity = splitOne[1][2..].Split(",").Select(Int32.Parse).ToArray();

    robots.Add(new Robot
    {
        PositionX = positions[0],
        PositionY = positions[1],
        VelocityX = velocity[0],
        VelocityY = velocity[1],
    });
}

var newRobots = new List<Robot>();
foreach (var rob in robots)
{
    var newRob = rob.MoveRobotWrap(width, height, seconds);
    newRobots.Add(newRob);
}

// VisualiseRobots(width, height, newRobots);
Console.WriteLine($"Result for part 1: {CalculateSafetyRatio(width, height, newRobots)}");

var numOfSeconds = 1;
var tolerance = 0;
var simulate = new List<Robot>(robots);
while (numOfSeconds <= 10000)
{
    simulate = simulate.Select(x => x.MoveRobotWrap(width,height,1)).ToList();
    
    var simulateDistr = simulate.GroupBy(x => (x.PositionX, x.PositionY)).ToDictionary(g => g.Key, g => g.Count());
    if (simulateDistr.Values.All(x=> x==1))
    {
        
        Console.WriteLine($"Found roughly symmmetrical match at {numOfSeconds}");
        VisualiseRobots(width, height, simulate);
    }

    numOfSeconds++;
}


int CalculateSafetyRatio(int width, int height, List<Robot> inputRobotsData)
{
    var midX = width / 2;
    var midY = height / 2;
    var quadOne = inputRobotsData.Count(x => x.PositionX < midX && x.PositionY < midY);
    var quadTwo = inputRobotsData.Count(x => x.PositionX < midX && x.PositionY > midY);
    var quadThree = inputRobotsData.Count(x => x.PositionX > midX && x.PositionY < midY);
    var quadFour = inputRobotsData.Count(x => x.PositionX > midX && x.PositionY > midY);
    return quadOne *  quadTwo *  quadThree *  quadFour;
}

void VisualiseRobots(int width, int height, List<Robot> robots) {

    var robotPosAndCount = robots.GroupBy(x => (x.PositionX, x.PositionY)).ToDictionary(g => g.Key, g => g.Count());

    for (int r = 0; r < height; r++)
    {
        for (int c = 0; c < width; c++)
        {
            var howManyRobots = robotPosAndCount.TryGetValue((c, r), out var robotsCount);
            var fmtRobat = howManyRobots ? robotsCount.ToString() : ".";
            
            Console.Write(fmtRobat);
        }
        Console.WriteLine();
    }
}
public record Robot
{
    public int PositionX { get; set; }
    public int VelocityX { get; set; }
    public int PositionY { get; set; }
    public int VelocityY { get; set; }

    public Robot MoveRobotWrap(int width, int height, int seconds)
    {
        var moveToX = PositionX + VelocityX * seconds;
        var moveToY = PositionY + VelocityY * seconds;
        while (!(moveToX >= 0 && moveToX < width))
        {
            var direction = moveToX >= width ? -1 : 1;
            moveToX += direction * width; 
        }
        
        while (!(moveToY >= 0 && moveToY < height))
        {
            var direction = moveToY >= height ? -1 : 1;
            moveToY += direction * height; 
        }

        return this with { PositionX = moveToX, PositionY = moveToY };
    }
}