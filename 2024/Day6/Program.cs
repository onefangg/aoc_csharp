// See https://aka.ms/new-console-template for more information

var data = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "day6.txt"));
var gridData = data.Select(row => row.ToCharArray()).ToArray();

(int, int) GetCoordinates(char[][] gridData, char ele)
{
    for (int i = 0; i < gridData.Length; i++)
    {
        for (int j = 0; j < gridData[i].Length; j++)
        {
            if (ele == gridData[i][j]) return (i, j);
        }
    }

    // cannot find
    return (-1, -1);
}

Direction Turn(Direction direction)
{
    switch (direction)
    {
        case Direction.Up: return Direction.Right;
        case Direction.Right: return Direction.Down;
        case Direction.Down: return Direction.Left;
        case Direction.Left: return Direction.Up;
    }

    return direction;
}

(HashSet<(int, int)>, bool) Walk(Direction initDirection, (int, int) initCoordinates, char[][] grid)
{
    var (currRow, currCol) = initCoordinates;
    HashSet<(int, int,Direction)> uniqueWalked = new HashSet<(int, int,Direction)>() { (currRow, currCol, initDirection) };
    HashSet<(int, int)> uniqueCoords = new HashSet<(int, int)>() { (currRow, currCol) };
    var currDirection = initDirection;
    var hasCollision = false;
    while ((currRow >= 0 && currRow < grid.Length) || (currCol >= 0 && currCol < grid[0].Length))
    {
        try
        {
            if (currDirection == Direction.Up)
            {
                if (grid[currRow - 1][currCol] != '#')
                {
                    currRow--;
                    uniqueCoords.Add((currRow, currCol));
                    if (uniqueWalked.Contains((currRow, currCol, currDirection)))
                    {
                        hasCollision = true;
                        break;
                    }
                    uniqueWalked.Add((currRow, currCol, currDirection));
                }
                else
                {
                    currDirection = Turn(currDirection);
                }
            }
            else if (currDirection == Direction.Right)
            {
                if (grid[currRow][currCol + 1] != '#')
                {
                    currCol++;
                    uniqueCoords.Add((currRow, currCol));
                    if (uniqueWalked.Contains((currRow, currCol, currDirection)))
                    {
                        hasCollision = true;
                        break;
                    }
                    uniqueWalked.Add((currRow, currCol, currDirection));
                }
                else
                {
                    currDirection = Turn(currDirection);
                }
            }
            else if (currDirection == Direction.Down)
            {
                if (grid[currRow + 1][currCol] != '#')
                {
                    currRow++;
                    uniqueCoords.Add((currRow, currCol));
                    if (uniqueWalked.Contains((currRow, currCol, currDirection)))
                    {
                        hasCollision = true;
                        break;
                    }
                    uniqueWalked.Add((currRow, currCol, currDirection));
                }
                else
                {
                    currDirection = Turn(currDirection);
                }
            }
            else if (currDirection == Direction.Left)
            {
                if (grid[currRow][currCol - 1] != '#')
                {
                    currCol--;
                    uniqueCoords.Add((currRow, currCol));
                    if (uniqueWalked.Contains((currRow, currCol, currDirection)))
                    {
                        hasCollision = true;
                        break;
                    }
                    uniqueWalked.Add((currRow, currCol, currDirection));
                }
                else
                {
                    currDirection = Turn(currDirection);
                }
            }
        }
        catch
        {
            break;
        }
    }

    return (uniqueCoords, hasCollision);
}


var (startRow, startCol) = GetCoordinates(gridData, '^');
var (uniqueCoordsWalked, _) = Walk(Direction.Up, (startRow, startCol), gridData);
Console.WriteLine($"{uniqueCoordsWalked.Count}");

var res = 0;
var uniqueCoordsWalkedIter = uniqueCoordsWalked.Except([(startRow, startCol)]).ToArray();
var collidedCoords = new HashSet<(int, int)>();

for (var idx=0;idx<uniqueCoordsWalkedIter.Length; idx++) {

    var (editRow, editCol) = uniqueCoordsWalkedIter[idx];
    var editGridData = new char[gridData.Length][];
    for (int i = 0; i < gridData.Length; i++)
    {
        
        editGridData[i] = new char[gridData[i].Length];
        for (int j = 0; j < gridData[i].Length; j++)
        {
            if (i == editRow && j == editCol) editGridData[i][j] = '#';
            else editGridData[i][j] = gridData[i][j];
        }
    }

    var (_, collision) = Walk(Direction.Up,(startRow, startCol),  editGridData);
    if (collision) res++;
}
Console.WriteLine($"{res}");

public enum Direction
{
    Up = 0,
    Down = 1,
    Left = 2,
    Right = 3,
}