// See https://aka.ms/new-console-template for more information

var data = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "day5.txt"));

var separator = Array.IndexOf(data, "");

var (ruleSets, updatesList) = (data[..separator]
        .Select(x => x.Split("|")).Select(x => (Int32.Parse(x[0]), Int32.Parse(x[1])))
        .GroupBy(x => x.Item1, x => x.Item2).ToDictionary(x => x.Key, v => v.ToArray())
    , data[(separator+1)..].Select(x => x.Split(",").Select(Int32.Parse).ToArray()).ToArray());

(List<int[]>, List<int>) FindCorrectUpdates(int[][] updates, Dictionary<int, int[]> rules)
{
    List<int[]> correctUpdates = new List<int[]>();
    List<int> correctUpdatesIndices = new List<int>();
    
    foreach (var (update, updateIdx) in updates.Select((x, i) =>(x, i)))
    {
        var isUpdateGood = true;
        for (var idx = 0; idx < update.Length-1; idx++)
        {
            var pagesAfter = update[(idx + 1)..].ToArray();

            if (!rules.ContainsKey(update[idx]))
            {
                isUpdateGood = false;
                break;
            }
            var rulesAtIdx = rules[update[idx]];
            if (!pagesAfter.All(x => rulesAtIdx.Contains(x)))
            {
                isUpdateGood = false;
                break;
            }
        }
        if (isUpdateGood){
            correctUpdates.Add(update);
            correctUpdatesIndices.Add(updateIdx);
        }
    }
    return (correctUpdates, correctUpdatesIndices);
}

var (correctUpdateList, correctIndicies) = FindCorrectUpdates(updatesList, ruleSets);
Console.WriteLine($"Part 1 solution: {correctUpdateList.Select(x=>x[x.Length/2]).Sum()}");

var remainingIncorrectUpdates = updatesList.Where((v, i) => ! correctIndicies.Contains(i)).ToArray();


var correctedUpdateList = new List<int[]>();
foreach(var update in remainingIncorrectUpdates)
{
    var correctedUpdate = new List<int>() {update[0]};
    for (var idx = 1; idx < update.Length; idx++)
    {
        if (!ruleSets.ContainsKey(update[idx]))
        {
            // if no ordering rule, easiest to put it at the back
            correctedUpdate.Add(update[idx]);
            continue;
        }
        var pagesToBeBefore = ruleSets[update[idx]];
        var insertedAtRightPlace = false;
        for (var correctingIdx = 0; correctingIdx < correctedUpdate.Count; correctingIdx++)
        {
            if (pagesToBeBefore.Contains(correctedUpdate[correctingIdx]))
            {
                correctedUpdate.Insert(correctingIdx, update[idx]);
                insertedAtRightPlace = true;
                break;
            }
        }
        if (!insertedAtRightPlace) correctedUpdate.Add(update[idx]);
    }
    correctedUpdateList.Add(correctedUpdate.ToArray());
}

Console.WriteLine($"Part 2 {correctedUpdateList.Select(x => x[x.Length/2]).Sum()}");
