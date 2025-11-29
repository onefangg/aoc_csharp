// See https://aka.ms/new-console-template for more information

using System.Data;

var data = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "day7_sample.txt"));

var callibrations = data.Select(line => line.Split(":").ToArray())
    .ToDictionary(k => Int64.Parse(k[0]), v => v[1].Trim().Split().ToArray());


void RecursivePermutations(string current, int length, HashSet<string> results, string[] pattern = null)
{
    if (current.Length == length)
    {
        results.Add(current);
        return;

    }
    RecursivePermutations(current + "+", length, results);
    RecursivePermutations(current + "*", length, results);
}

void RecursivePermutationsForPart2(string current, int length, HashSet<string> results)
{
    if (current.Length == length)
    {
        results.Add(current);
        return;
    }
    RecursivePermutationsForPart2(current + "+", length, results);
    RecursivePermutationsForPart2(current + "*", length, results);
    RecursivePermutationsForPart2(current + "|", length, results);
}

HashSet<string> GetAllOperatorPermutations(int maxNumOfOperators)
{
    var permutations = new HashSet<string>();
    RecursivePermutations(string.Empty, maxNumOfOperators, permutations);
    return permutations;
}

HashSet<string> GetAllOperatorPermutationsForPart2(int maxNumOfOperators)
{
    var permutations = new HashSet<string>();
    RecursivePermutationsForPart2(string.Empty, maxNumOfOperators, permutations);
    return permutations;
}
bool CalculateForPermutations(string[] arr, long value, Func<int, HashSet<string>> getPermFunc)
{
    var opNum = arr.Length - 1;
    var possibleOperators = getPermFunc(opNum).ToArray();
    foreach (var possiblity in possibleOperators)
    {
        var padPossiblity = possiblity.ToCharArray().Select(x => x.ToString()).Concat([string.Empty]);
        var op = arr.Zip(padPossiblity, (a,b) => (a, b)).SelectMany(x => new [] { x.Item1, x.Item2}).Where(x => x != String.Empty).ToArray();

        var runningNumber = Int64.Parse(op[0]);
        var numOp = Operator.Plus;
        foreach (var thing in op.Skip(1))
        {
            switch (thing)
            {
                case "*": numOp = Operator.Multiply; continue;
                case "+": numOp = Operator.Plus; continue;
                case "|": numOp = Operator.Concat; continue;
            }

            switch (numOp)
            {
                case Operator.Plus:
                    runningNumber += Int64.Parse(thing);
                    break;
                case Operator.Multiply:
                    runningNumber *= Int64.Parse(thing);
                    break;
                case Operator.Concat:
                    runningNumber = long.Parse(String.Concat(runningNumber, thing));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
        }

        if (runningNumber == value)
        {
            return true;
        }
    }
    return false;
}

var partOne = callibrations.Where(x => CalculateForPermutations(x.Value, x.Key, GetAllOperatorPermutations)).Select(x => x.Key).Sum();

Console.WriteLine($"Part 1 solution {partOne}");


var partTwo = callibrations.Where(x => CalculateForPermutations(x.Value, x.Key, GetAllOperatorPermutationsForPart2))
    .Select(x => x.Key).Sum();
Console.WriteLine($"Part 2 solution {partTwo}");
enum Operator
{
    Plus,
    Multiply,
    Concat
}