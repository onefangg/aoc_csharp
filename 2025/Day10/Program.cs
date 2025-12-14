using System.Text.RegularExpressions;
using Microsoft.Z3;

// var inputData = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sample.txt"));
var inputData = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "day10.txt"));


var machines = new List<Machine>();
for (int i = 0; i < inputData.Length; i++)
{
    var line = Regex.Match(inputData[i], @"(\[.*\])\s(.*)\s(\{.*\})");
    var lights = line.Groups[1].ToString();
    var switches = line.Groups[2].ToString().Split(" ");
    var joltage = line.Groups[3].ToString();


    var parsedLights = new int[lights.Length - 2];
    for (int j = 1; j < lights.Length - 1; j++)
    {
        parsedLights[j - 1] = lights[j] == '.' ? 0 : 1;
    }

    var parsedSwitches = new List<List<int>>();
    foreach (var t in switches)
    {
        var sw = t[1..^1].Split(",").Select(int.Parse).ToList();
        parsedSwitches.Add(sw);
    }

    machines.Add(new Machine
    {
        FinalLights = parsedLights, Switches = parsedSwitches,
        Joltage = joltage[1..^1].Split(',').Select(int.Parse).ToArray()
    });
}


// THIS IS SO SLOW >:(
// var partOne = 0;
// for (int i = 0; i < machines.Count; i++)
// {
//     Console.WriteLine($"machine {i}: {machines[i]}");
//     partOne += SolveMachine(machines[i]);
// }

// Console.WriteLine($"Part 1: {machines.Select(SolveMachine).Sum()}");

int partTwo = 0;
for (int i = 0; i < machines.Count; i++)
{
    var result = SolveSwitchesForMachine(machines[i]);
    partTwo += result;
}
Console.WriteLine($"Part 2: {partTwo}");
int SolveSwitchesForMachine(Machine machine)
{
    using var context = new Context();
    Optimize optimizer = context.MkOptimize();

    var rowX = machine.Switches.Select((x, idx) => context.MkIntConst($"x{idx}")).ToArray();
    var minObj = context.MkAdd(rowX);
    
    optimizer.MkMinimize(minObj);
    var bulbs = machine.Joltage.Length;
    var cntMatrix = new int[bulbs][];
    for (int r = 0; r < bulbs; r++)
    {
        cntMatrix[r] = new int[machine.Switches.Count];
    }
    
    for (int c = 0; c < machine.Switches.Count; c++)
    {
        var switches = machine.Switches[c];
        foreach (var sw in switches)
        {
            cntMatrix[sw][c] = 1;
        }
    }

    
    foreach (var t in rowX)
    {
        optimizer.Add(context.MkGe(t, context.MkInt(0)));
    }
    
    for (int r = 0; r < cntMatrix.Length; r++)
    {
        var pivots = cntMatrix[r].Select((x, i) => context.MkMul(context.MkInt(x),rowX[i])).ToArray();
        var equation = context.MkEq(
            context.MkAdd(
                pivots
            ),
            context.MkInt(machine.Joltage[r]));
        optimizer.Add(equation);
    }

    if (optimizer.Check() == Status.SATISFIABLE)
    {
        Model model = optimizer.Model;

        int result = 0;
        foreach (FuncDecl d in model.ConstDecls)
        {

            var val = (IntNum)optimizer.Model.ConstInterp(d);
            result += val.Int;
        }

        return result;
    }
    
    return -1;

}


// Console.WriteLine($"Part 2: {SolveSwitchesForMachine(machines[0])}");
// int SolveSwitchesForMachine(Machine machine)
// {
//     var bulbs = machine.Joltage.Length;
//     var cntMatrix = new int[bulbs][];
//     for (int r = 0; r < bulbs; r++)
//     {
//         cntMatrix[r] = new int[machine.Switches.Count];
//     }
//     
//
//     for (int c = 0; c < machine.Switches.Count; c++)
//     {
//         var switches = machine.Switches[c];
//         foreach (var sw in switches)
//         {
//             cntMatrix[sw][c] = 1;
//         }
//     }
//     
//     
//     
//     return GaussianJordanElimination(cntMatrix, machine.Joltage, machine);
// }
//
//
//
// void GaussianElimination(int[][] matrix, int[] v)
// {
//     var rows = matrix.Length;
//     var cols = matrix[0].Length;
//     for (int idx = 0; idx < rows - 1; idx++)
//     {
//         var pivot = -1;
//         for (int c = 0; c < cols; c++)
//         {
//             for (int r = idx; r < rows; r++)
//             {
//                 if (matrix[r][c] == 1)
//                 {
//                     pivot = c;
//                     break;
//                 }
//             }
//
//             if (pivot != -1) break;
//         }
//         if (pivot == -1) continue; // ie. it's already in reduced-form
//         var fromIdx = Enumerable.Range(idx, rows-idx).FirstOrDefault(i => matrix[i][pivot] == 0);
//         var toIdx = Enumerable.Range(idx, rows-idx).FirstOrDefault(i => matrix[i][pivot] == 1);
//         if (fromIdx == null || toIdx == null || toIdx < fromIdx) continue; // no need, proceed 
//         
//         (matrix[fromIdx], matrix[toIdx]) = (matrix[toIdx], matrix[fromIdx]);
//         (v[fromIdx], v[toIdx]) = (v[toIdx], v[fromIdx]);
//         // for (int r = toIdx + 1; r < rows; r++)
//         // {
//         //     if (matrix[r][c] != 0)
//         //     {
//         //         matrix[r] = matrix[r].Select((val, i) => val - matrix[toIdx][i]).ToArray();
//         //         v[r] -= v[toIdx];
//         //     }
//         //
//         // }
//     }
//
// }
//
// int GaussianJordanElimination(int[][] matrix, int[] v, Machine machine)
// {
//     GaussianElimination(matrix, v);
//     var rows = matrix.Length;
//     var cols = matrix[0].Length;
//     // for (int i = 0; i < rows; i++)
//     // {
//     //     for (int j = 0; j < cols; j++)
//     //     {
//     //         Console.Write(matrix[i][j]);
//     //     }
//     //     Console.WriteLine();
//     // }
//     // Console.WriteLine("====");
//
//     for (int r = rows - 1; r > 0; r--)
//     {
//         // working backwards, we start from the last pivot column
//         int pivot = -1;
//         for (int c = 0; c< cols; c++)
//         {
//             if (matrix[r][c] == 1)
//             {
//                 pivot = c;
//                 break;
//             }
//         }
//
//         if (pivot == -1) continue;
//
//         for (int idx = r - 1; idx >= 0; idx--)
//         {
//             if (matrix[idx][pivot] == 0) continue;
//             matrix[idx] = matrix[idx].Select((x, i) => x - matrix[r][i]).ToArray();
//             v[idx] -= v[r];
//         }
//     }
//     var freeVariables = new List<int>();
//     for (int c = 0; c < cols; c++)
//     {
//         if (Enumerable.Range(0, rows).Count(x => matrix[x][c] != 0) > 1)
//         {
//             freeVariables.Add(c);
//         }  
//     }
//
//     var maxIter = machine.Joltage.Max();
//     
//     
//     // generate a bunch of free variables in a [col x 1] convention
//     var allPossibleV = Generate(cols, freeVariables.ToArray(), 0, maxIter);
//     
//     // do matrix multiplcation
//     
//     // for (int i = 0; i <= maxIter; i++)
//     // {
//     //     for (int r = 0; r < rows; r++)
//     //     {
//     //         
//     //         foreach (var freeIdx in freeVariables)
//     //         {
//     //             scale[freeIdx] = i;
//     //         }
//     //         
//     //     }
//     //
//     // }
//     // for (int i = 0; i < rows; i++)
//     // {
//     //     for (int j = 0; j < cols; j++)
//     //     {
//     //         Console.Write(matrix[i][j]);
//     //     }
//     //     Console.WriteLine();
//     // }
//     //
//     // for (int i = 0; i < v.Length; i++)
//     // {
//     //     Console.Write($"{v[i]} ,");
//     // }
//
//
//     int[] result = [];
//     foreach (var scale in allPossibleV)
//     {
//         
//         result = new int[rows];
//         for (int r = 0; r < rows; r++)
//         {
//             result[r] = matrix[r].Select((r, i) => r * scale[i]).Sum();
//             if (result[r] != v[r]) break;  // doesn't match
//         }
//     }
//     
//     if (result.Length >0 ) return result.Sum();
//     throw new Exception("Unable to solve");
//     
//
// }   
// IEnumerable<int[]> Generate(
//     int length,
//     int[] indices,
//     int n,
//     int m,
//     int defaultValue = 0)
// {
//     int k = indices.Length;
//     int range = m - n + 1;
//
//     int total = (int)Math.Pow(range, k);
//
//     for (int mask = 0; mask < total; mask++)
//     {
//         var arr = Enumerable.Repeat(defaultValue, length).ToArray();
//
//         int x = mask;
//         for (int i = 0; i < k; i++)
//         {
//             int value = n + (x % range);
//             arr[indices[i]] = value;
//             x /= range;
//         }
//
//         yield return arr;
//     }
// }
int SolveMachine(Machine machine)
{
    // from, to
    HashSet<Lights> steps = []; // maybe this should be a list
    var initialState = new int[machine.FinalLights.Length];
    var initialLightState = new LightState { State = initialState };
    var allSwitches = machine.Switches.Select(x => x.ToArray()).ToArray();

    HashSet<LightState> states = [initialLightState];

    while (true)
    {
        bool shouldBreak = false;
        foreach (var sw in allSwitches)
        {
            var newStates = new HashSet<LightState>();
            foreach (var state in states)
            {
                int[] nextState = state.State.Select(x => x).ToArray();
                foreach (var pos in sw)
                {
                    nextState[pos] ^= 1;
                }


                var nextLightState = new LightState { State = nextState.ToArray() };
                var light = new Lights
                {
                    From = state,
                    To = nextLightState
                };
                steps.Add(light);
                newStates.Add(nextLightState);
            }

            if (newStates.Select(x => states.Add(x)).All(x => x == false))
            {
                shouldBreak = true;
                break;
            }
        }

        if (shouldBreak) break;
    }

    var pq = new Queue<LightState>();
    var visited = new HashSet<LightState>();
    Dictionary<LightState, int> distance = [];
    var finalState = new LightState() { State = machine.FinalLights };

    pq.Enqueue(initialLightState);
    distance[initialLightState] = 0;
    distance[finalState] = int.MaxValue;
    while (pq.Count > 0)
    {
        LightState state = pq.Dequeue();
        var transitions = steps.Where(x => x.From == state).ToArray();
        if (!visited.Add(state) || state == finalState)
        {
            continue;
        }

        var neighbours = transitions.Select(x => x.To).ToArray();
        foreach (var n in neighbours)
        {
            // Console.WriteLine($"From {state}");
            // Console.WriteLine($"To {n}");
            if (n == finalState)
            {
                distance[finalState] = distance[finalState] < distance[state] + 1
                    ? distance[finalState]
                    : distance[state] + 1;
                continue;
            }

            distance[n] = distance[state] + 1;
            pq.Enqueue(n);
        }
    }


    return distance[finalState];
}

record LightState
{
    public int[] State { get; set; }

    public override string ToString()
    {
        return String.Join(",", State);
    }

    public virtual bool Equals(LightState? other)
    {
        return other?.State != null && State.SequenceEqual(other?.State);
    }

    public override int GetHashCode()
    {
        var hash = new HashCode();
        foreach (var x in State) hash.Add(x);
        return hash.ToHashCode();
    }
}

record Lights
{
    public LightState From { get; set; }
    public LightState To { get; set; }

    public virtual bool Equals(Lights? other)
    {
        return From.Equals(other?.From) && To.Equals(other?.To);
    }

    public override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(From);
        hash.Add(To);
        return hash.ToHashCode();
    }
}


record Machine
{
    public int[] FinalLights { get; set; }
    public List<List<int>> Switches { get; set; }
    public int[] Joltage { get; set; }
}