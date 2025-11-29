// See https://aka.ms/new-console-template for more information

using System.Globalization;

var data = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "day9.txt"))[0].ToCharArray().Select(x =>x.ToString()).ToArray();
var diskMap = new List<string>();
for (int i = 0; i < data.Length; i++)
{
    if (i % 2 == 0)
    {
        diskMap.AddRange(Enumerable.Repeat(Math.Ceiling((double)i/2).ToString(CultureInfo.InvariantCulture), int.Parse(data[i])));
    }
    else
    {
        diskMap.AddRange(Enumerable.Repeat(".", int.Parse(data[i])));
    }
}


var diskMapArray = diskMap.ToArray();

for (int i = diskMapArray.Length - 1; i >= 0; i--)
{
    if (diskMapArray[i] != ".")
    {
        for (int j = 0; j < i; j++)
        {
            if (diskMapArray[j] == ".")
            {
                diskMapArray[j] = diskMapArray[i];
                diskMapArray[i] = ".";
                break;
            }
        }
    }
}

Console.WriteLine($"Part 1: {GetChecksum(diskMapArray)}");
int GetChecksum(string[] disk)
{
    var checksum = 0;
    for (int i = 0; i < disk.Length; i++)
    {
        if (disk[i] != ".")
        {
            checksum += i * int.Parse(disk[i]);
        }
    }

    return checksum;
}