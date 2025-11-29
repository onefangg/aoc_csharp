// See https://aka.ms/new-console-template for more information

var data  = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "day24.txt"));
var splitAtIdx = Array.FindIndex(data, string.IsNullOrEmpty);
var wires = data[..splitAtIdx];
var gates = data[(splitAtIdx + 1)..];

var lookup = new Dictionary<string, bool>();

foreach (var w in wires)
{
    var (bitName, bit) = (w.Split(": ")[0], w.Split(": ")[1]);
    lookup[bitName] = !bit.Equals("0");
}

var q = new Queue<string>(gates);

while (q.Count > 0)
{
    var g = q.Dequeue();
    var parse = g.Split(" ");
    var op = parse[1];
    var (a, b) = (parse[0], parse[2]);
    if (!(lookup.ContainsKey(a) && lookup.ContainsKey(b)))
    {
        q.Enqueue(g);
        continue;
    }
    
    var c = parse[^1];

    switch (op)
    {
        case "AND":
            lookup[c] = lookup[a] && lookup[b];
            break;
        case "OR":
            lookup[c] = lookup[a] || lookup[b];
            break;
        case "XOR":
            lookup[c] = lookup[a] ^ lookup[b];
            break;
    }
}

var xWires = Convert.ToInt64(string.Join("", wires.Where(x => x.StartsWith("x")).OrderByDescending(x => x.Split(": ")[0]).Select(x=>Convert.ToInt32(x.Split(": ")[1] == "1"))), 2);  
var yWires = Convert.ToInt64(string.Join("", wires.Where(x => x.StartsWith("y")).OrderByDescending(x => x.Split(": ")[0]).Select(x=>Convert.ToInt32(x.Split(": ")[1] == "1"))), 2);
var zWires = Convert.ToString(xWires + yWires, 2);

var baseString = string.Join("", lookup.Where(x=>x.Key.StartsWith("z"))
    .OrderByDescending(x=>x.Key)
    .Select(x=> Convert.ToInt32(x.Value))).ToString();
Console.WriteLine($"Part 1: {Convert.ToInt64(baseString, 2)}");
var padBaseString = String.Join("", baseString.PadRight(zWires.Length, '0').Reverse().ToArray());


var constructPath = new Dictionary<string, List<(string, string, string)>>();
foreach (var g in gates)
{
    var parse = g.Split(" ");
    var op = parse[1];
    var (a, b) = (parse[0], parse[2]);
    var c = parse[^1];
    if (!constructPath.TryAdd(c, [(a, op, b)]))
    {
        constructPath[c].Add((a, op, b));
    }
}

var secondLookup = new Dictionary<string, HashSet<(string, string, string)>>();
foreach (var (c, v) in constructPath)
{
    secondLookup[c] = FindAncestors(constructPath, c);
}


var outputs = new List<(string, string, string, string)>();
foreach (var g in gates)
{
    var parse = g.Split(" ");
    var op = parse[1];
    var (a, b) = (parse[0], parse[2]);
    var c = parse[^1];
    if (c.StartsWith("z") && op != "XOR")
    {
        Console.WriteLine($"{a} {op} {b}: {c}");
    } else if (!c.StartsWith("z") && !(a.StartsWith("x") || a.StartsWith("y")) && op == "XOR")
    {
        Console.WriteLine($"{a} {op} {b}: {c}");
    }
    outputs.Add((a,op,b,c));
}

Console.WriteLine($"---");
foreach (var o in outputs)
{
    var (a, op, b, c) = o;
    if ((a.StartsWith("x") || a.StartsWith("y")) && op == "XOR")
    {
        if (!outputs.Any(x => (x.Item1 == c || x.Item3 == c)&& x.Item2 == op))
        {
            Console.WriteLine($"{a} {op} {b}: {c}");
        } 
    }
    
    if (op == "AND")
    {
        if (!outputs.Any(x => (x.Item1 == c || x.Item3 == c) && x.Item2 == "OR"))
        {
            Console.WriteLine($"{a} {op} {b}: {c}");
        } 
    }
}

Console.WriteLine(zWires);
Console.WriteLine(padBaseString);

HashSet<(string, string, string)> FindAncestors(Dictionary<string, List<(string, string, string)>> relations, string parent)
{
    var result = new HashSet<(string, string, string)>();
    Recurse(parent, relations, result);
    void Recurse(string children, Dictionary<string, List<(string, string, string)>> relations, HashSet<(string, string, string)> unique)
    {
        if (!relations.TryGetValue(children, out var relation))
        {
            return;
        }
        foreach (var child in relation)
        {
            if (!unique.Add(child))
            {
                continue;
            };
            
            Recurse(child.Item1, relations, result);
            Recurse(child.Item3, relations, result);
        }   
    }

    return result;
}





