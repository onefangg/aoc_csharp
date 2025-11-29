// See https://aka.ms/new-console-template for more information

// long n = 123;
// long[] input = [123];
// long[] input = [1, 2, 3, 2024];
// long[] input = [1,10, 100, 2024];
var data = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "day22.txt"));
long[] input = data.Select(x => long.Parse(x.ToString())).ToArray();


// sequence as key
// (init_seed_num, price)
var lookup = new Dictionary<string, List<(long, long)>>();

long s = 0;
foreach (var number in input)
{
    long res;
    long buf = number;
    var iter = 0;
    var prices = new long[2000];
    
    var uniqueSeq = new HashSet<string>();
    while (iter < 2000)
    {
        res = CalculatePseudoRandom(buf);
        buf = res;
        var p = buf % 10;
        prices[iter] = p;

        if (iter >= 4) // assume half search space
        {
            var deltaWin = prices[(iter-3)..(iter+1)].Zip(prices[(iter-4)..(iter)], (x, y) =>x-y).ToArray();
            var deltaSeq =  String.Join(",", deltaWin);
            
            if (uniqueSeq.Add(deltaSeq) && !lookup.TryAdd(deltaSeq, [(number, p)]))
            {
                lookup[deltaSeq].Add((number, p));
            }
        }
        
        // move on to next sequence
        iter++;
    }
    s += buf;
}



var optimise = lookup.OrderByDescending(x =>
{
    return x.Value.Sum(x => x.Item2);
});


Console.WriteLine($"Part 1: {s}");
Console.WriteLine($"Part 2: {optimise.First().Value.Sum(x=>x.Item2)}");


long CalculatePseudoRandom(long num)
{
    var stepOne = ((num * 64) ^ num) % 16777216;
    var stepTwo = (long) Math.Floor((double)stepOne / 32) ^ stepOne % 16777216;
    return ((stepTwo * 2048) ^ stepTwo) % 16777216;
}