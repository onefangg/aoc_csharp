using System.Text.RegularExpressions;

var inputData = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sample.txt"));
// var inputData = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "day10.txt"));


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


var partOne = 0;
// THIS IS SO SLOW >:(
for (int i = 0; i < machines.Count; i++)
{
    Console.WriteLine($"machine {i}: {machines[i]}");
    partOne += SolveMachine(machines[i]);
}

Console.WriteLine($"Part 1: {machines.Select(SolveMachine).Sum()}");

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
    Dictionary<LightState, int> distance =  [];
    var finalState = new LightState() { State = machine.FinalLights };
    
    pq.Enqueue(initialLightState);
    distance[initialLightState] = 0;
    distance[finalState] = int.MaxValue;
    while (pq.Count > 0)
    {
        LightState state = pq.Dequeue();
        var transitions = steps.Where(x => x.From == state).ToArray();
        if (!visited.Add(state) || state == finalState) { continue; }
        
        var neighbours = transitions.Select(x=>x.To).ToArray();
        foreach (var n in neighbours)
        {
            // Console.WriteLine($"From {state}");
            // Console.WriteLine($"To {n}");
            if (n == finalState)
            {
                distance[finalState] = distance[finalState] < distance[state] + 1 ? distance[finalState]: distance[state] + 1;
                continue;
            }
            distance[n] =  distance[state] + 1;
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