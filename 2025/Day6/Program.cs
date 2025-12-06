// var inputData = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sample.txt"));
var inputData = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "day6.txt"));

int[,] numbers = new int[inputData[0].Split(Array.Empty<char>(), StringSplitOptions.RemoveEmptyEntries).Length, inputData.Length-1];


for (int r = 0; r < inputData.Length - 1; r++)
{
    var line = inputData[r];
    var nums = line.Split(Array.Empty<char>(), StringSplitOptions.RemoveEmptyEntries);
    for (int j = 0; j < nums.Length ; j++)
    {
        numbers[j, r] = int.Parse(nums[j]);
    }
}

var operators = inputData[^1].Split(Array.Empty<char>(), StringSplitOptions.RemoveEmptyEntries);
var partOne = 0L;
for (int i = 0; i < numbers.GetLength(0); i++)
{
    var op = operators[i];
    var numBase = op == "+" ? 0L : 1L;
    var res = Enumerable.Range(0, numbers.GetLength(1)).Select(x => numbers[i, x]).Aggregate(numBase, (curr, next) =>
    {
        if (op == "*") return curr * next;
        if (op == "+") return curr + next;
        return -1;
    });
    partOne += res;
} 

Console.WriteLine($"Part One is: {partOne}");

// parsing it again for pt 2 >:(

var splitIndices = new int[operators.Length];
var curr = 0;

for (int c = 0; c < inputData[0].Length; c++)
{
    var check = new char[inputData.Length - 1];
    for (int r = 0; r < inputData.Length - 1; r++)
    {
        check[r]  = inputData[r][c];
    }

    if (check.All(x => x == ' '))
    {
        splitIndices[curr] = c;
        curr++;
    }
}
splitIndices[splitIndices.Length - 1] = inputData[0].Length;
var start = 0;


List<List<long>> partTwoHolder = [];
foreach (var nextStop in splitIndices)
{
    List<long> holder = [];
    List<string> strHolder = [];

    for (int row = 0; row < inputData.Length - 1; row++)
    {
        strHolder.Add(string.Join("", Enumerable.Range(start, nextStop - start).Select(x => inputData[row][x])).Replace(" ", "0"));
    }
    
        
    for (int i = strHolder[0].Length-1; i >= 0 ; i--)
    {
        var parsed = long.Parse(string.Join("", strHolder.Select(x => x[i]).Where(x => x != '0')));
        holder.Add(parsed);
    } 
    partTwoHolder.Add(holder);
    start = nextStop + 1;
}

long partTwo = 0;
for (int opIdx = 0; opIdx < partTwoHolder.Count; opIdx++)
{
    var op = operators[opIdx];
    var num = op == "+" ? 0L : 1L;
    partTwo += partTwoHolder[opIdx].Aggregate(num, (curr, next) => op == "*" ? curr * next : curr + next);    
}

Console.WriteLine($"Part Two is: {partTwo}");