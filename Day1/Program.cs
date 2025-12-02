var inputData = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "day1.txt"));

var curr = 50;
var count = 0;
var countB = 0;


int EuclidDiv(int a, int b)
{
    int mod = Math.Abs(b);
    int r = (a%mod + mod)%mod;
    int q = (a - r) / b; 
    
    return q;
}
foreach (var data in inputData)
{
    
    var direction = data.Substring(0, 1) == "R" ? 1: -1;
    var distance = Int32.Parse(data.Substring(1));

    if (direction > 0)
    {
        countB += (curr + distance) / 100;
    }
    else
    {
        countB += EuclidDiv(curr - 1, 100) - EuclidDiv(curr - distance- 1, 100);
    }
    
    curr = (curr + direction * distance) % 100;
    if (curr < 0) curr = 100 - Math.Abs(curr); // make it easier to follow along the steps

    if (curr == 0) { count++; } 
}

Console.WriteLine($"Part 1: {count}");
Console.WriteLine($"Part 2: {countB}");




