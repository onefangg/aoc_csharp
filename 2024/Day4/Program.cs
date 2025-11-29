// See https://aka.ms/new-console-template for more information

var data = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "day4.txt"));
var xmasMatrix = data.Select(x => x.ToCharArray()).ToArray();

bool CheckIfSameOrReverseOrderIsSame(string inputString, string matchingString)
{
    return inputString == matchingString || new string(inputString.Reverse().ToArray())== matchingString;
}
List<string> ParsePartOneResults(char[][] matrix)
{
    var results = new List<string>();
    for (var row = 0; row < matrix.Length; row++)
    {
        for (var col = 0; col < matrix[row].Length; col++)
        {
            if (matrix[row][col] != 'X') continue;
            if (col+3 < matrix[row].Length) results.Add(new String(matrix[row][col..(col+4)]));
            if (col-3 >= 0) results.Add(new String(matrix[row][(col - 3)..(col+1)]));
            if (row+3 < matrix.Length) results.Add(new String(matrix[row..(row+4)].Select(r => r[col]).ToArray()));
            if (row-3 >= 0) results.Add(new String(matrix[(row-3)..(row+1)].Select(r => r[col]).ToArray()));
        
            if (row-3 >= 0 && col-3>=0) results.Add(new String(Enumerable.Range(0, 4).Select(i => matrix[row-i][col-i]).ToArray()));
            if (row-3 >= 0 && col+3<matrix[row].Length) results.Add(new String(Enumerable.Range(0, 4).Select(i => matrix[row-i][col+i]).ToArray()));
        
            if (row+3<matrix.Length && col-3>=0) results.Add(new String(Enumerable.Range(0, 4).Select(i => matrix[row+i][col-i]).ToArray()));
            if (row+3<matrix.Length && col+3<matrix[row].Length) results.Add(new String(Enumerable.Range(0, 4).Select(i => matrix[row+i][col+i]).ToArray()));
        }
    }
    return results;
}

var partOneResults = ParsePartOneResults(xmasMatrix);
Console.WriteLine($"Number of XMAS-es: {partOneResults.Count(x => CheckIfSameOrReverseOrderIsSame(x, "XMAS"))}");

int ParsePartTwoResults(char[][] matrix)
{
    var res = 0;
    for (var row = 0; row < matrix.Length; row++)
    {
        for (var col = 0; col < matrix[row].Length; col++)
        {
            if (matrix[row][col] != 'A') continue;
            // if OOB SKIP
            if (row - 1 < 0 || row + 1 >= matrix.Length || col - 1 < 0 || col + 1 >= matrix[row].Length) continue;
            var diagonalOne = new string(new[] { matrix[row - 1][col - 1], matrix[row][col], matrix[row + 1][col + 1] });
            var diagonalTwo = new string(new[] { matrix[row-1][col+1], matrix[row][col], matrix[row+1][col-1] });
            if (CheckIfSameOrReverseOrderIsSame(diagonalOne, "SAM") &&
                CheckIfSameOrReverseOrderIsSame(diagonalTwo, "SAM")) res++;
        }
    }
    return res;
}
Console.WriteLine($"Number of X-MAS-es: {ParsePartTwoResults(xmasMatrix)}");