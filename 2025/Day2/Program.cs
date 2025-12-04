var inputData = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "day2.txt"))[0];


bool CheckInvalidSequence(long number)
{
   var digitNum = Math.Floor(Math.Log10(number)) + 1;
   var divisor = Math.Pow(10, digitNum / 2);
   var firstHalf = Math.Floor(number / divisor);
   var secondHalf = number  % divisor;
   return Math.Abs(firstHalf - secondHalf) < 10e-5;
}

bool CheckInvalidSequencePartTwo(long number, long divisor)
{
    if (divisor == 0) return false;
    var digitNum = Math.Floor(Math.Log10(number)) + 1;
    var expectedParts = (int)Math.Floor(digitNum / divisor);
    var curr = number;   

    
    var div = (int)Math.Pow(10, divisor);
    List<double> parts = new List<double>();
    while (curr != 0)
    {
        var pt = curr % div;
        parts.Add(pt);
        curr =(long) Math.Floor((decimal)curr/div) ;
    }

    if (parts.Count == 0) return false;
    var ele = parts.First();
    
    if (parts.Count() == expectedParts && parts.All(x => Math.Abs(x - ele) < 10e-5)) return true;
    

    return CheckInvalidSequencePartTwo(number, divisor - 1);
}

var ranges = inputData.Split(",").SelectMany(x =>
{
    var splitted = x.Split("-");
    var (min, max) = (long.Parse(splitted[0]), long.Parse(splitted[1]));
    var val = new List<long>();
    for (long i = min; i <= max; i++)
    {
        val.Add(i);
    }

    return val;
});

var partOne = ranges.Where(CheckInvalidSequence).Sum();
var partTwo = ranges.Where(x => CheckInvalidSequencePartTwo(x, (int)((Math.Floor(Math.Log10(x))) + 1) / 2)).ToArray();

foreach (var w in partTwo)
{
    Console.WriteLine(w);
}

// Console.WriteLine(CheckInvalidSequencePartTwo(10101, 2));
// Console.WriteLine(CheckInvalidSequencePartTwo(222222, 3));
Console.WriteLine($"Part 1: {partOne}");
Console.WriteLine($"Part 2: {partTwo.Sum()}");