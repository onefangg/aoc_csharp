var data = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "day23.txt"));
var network = data.Select(x=> x.Split('-')).ToList();

var networkMap = new Dictionary<string, HashSet<string>>();

foreach (var node in network)
{
    var (partOne, partTwo) = (node[0], node[1]);
    if (!networkMap.TryAdd(partOne, [partTwo])) 
    {
        networkMap[partOne].Add(partTwo);
    }
    if (!networkMap.TryAdd(partTwo, [partOne])) 
    {
        networkMap[partTwo].Add(partOne);
    }
}

var lookup = new Dictionary<string, int[]>();
foreach (var (node, neighbours) in networkMap)
{
    // if co - de and co - ka and de -ka
    // from the perspective of co, de and ka connecting to each counts them as 2\
    var arr = neighbours.ToArray();
    lookup[node] = new int[arr.Length];
    for (int i = 0; i < arr.Length; i++)
    {
        var anchor = arr[i];
        var otherNeighbours = arr.Where(x => x != anchor).ToArray();
        var anchorNeighbours = networkMap[anchor].Where(x=>x != node).ToArray();
        
        lookup[node][i] = anchorNeighbours.Intersect(otherNeighbours).Count();    
    }
}

var g =  lookup
    .MaxBy(x => {
        var maxV = x.Value.Max();
        return x.Value.Count(y=>y == maxV);    
    }
    ).Key;
var num = lookup[g];
var maxrelation = num.Max();

var relevant = new List<string>([g]);
for (int i = 0; i < networkMap[g].Count; i++)
{
    if (num[i] == maxrelation)
    {
        relevant.Add(networkMap[g].ToArray()[i]);
    }
}

GetPart1Solution(networkMap);
Console.WriteLine($"Part 2: {string.Join(",", relevant.OrderBy(x=>x))}");

void GetPart1Solution(Dictionary<string, HashSet<string>> inputNetworkMap)
{
    var connectedToThree = new List<string[]>();
    foreach (var (node, neighbours) in inputNetworkMap)
    {
        foreach (var n in neighbours)
        {
            if (!inputNetworkMap.ContainsKey(n))
            {
                continue;
            }
            inputNetworkMap.TryGetValue(n, out HashSet<string> indirectNeighbours);
            var sameNeighbours = indirectNeighbours.Intersect(neighbours).ToArray();
            if (sameNeighbours.Any())
            {
                foreach (var n2 in sameNeighbours)
                {
                    string[] constructMap = [node, n, n2];
                    constructMap = constructMap.OrderBy(x => x).ToArray();
                    if (!connectedToThree.Any(x => x.SequenceEqual(constructMap)))
                    {
                        connectedToThree.Add(constructMap);    
                    }    
                }
            }
        }
    }

    Console.WriteLine($"{connectedToThree.Count(x => x.Any(y => y.StartsWith("t")))}");
}

