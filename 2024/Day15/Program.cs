// See https://aka.ms/new-console-template for more information

var data = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "day15.txt"));
int splitAt = -1;
for (int i = 0; i < data.Length; i++)
{
    if (data[i] == string.Empty)
    {
        splitAt = i;
        break;
    }
}

var grid = data[..splitAt].Select(x => x.ToCharArray()).ToArray();
var directions = data[(splitAt + 1)..].SelectMany(x => x).ToArray();
var wideGrid = TranslateToWideGrid(grid);

var currRobotPosition = FindRobotPosition(grid);
foreach (var dire in directions)
{
    if (dire == '<')
    {
        var newPos = ShiftRobotPosition(grid, (0,-1), currRobotPosition);
        currRobotPosition = newPos;
    } else if (dire == '^')
    {
        var newPos = ShiftRobotPosition(grid, (-1,0), currRobotPosition);
        currRobotPosition = newPos;
    } else if (dire == '>')
    {
        var newPos = ShiftRobotPosition(grid, (0,1), currRobotPosition);
        currRobotPosition = newPos;
    } else if (dire == 'v')
    {
        var newPos = ShiftRobotPosition(grid, (1,0), currRobotPosition);
        currRobotPosition = newPos;
    }
}

// VisualiseRobots(grid);
Console.WriteLine($"Part 1: {CalculateBoxCoordinations(grid)}");

var iteration = 1;

var currRobotPositionForPartTwo = FindRobotPosition(wideGrid);

foreach (var dire in directions)
{
    (int, int) offset = (0, 0); // whack
    if (dire == '<')
    {
        offset = (0,-1);
    } else if (dire == '^')
    {
        offset = (-1,0);
    } else if (dire == '>')
    {
        offset = (0,1);
    } else if (dire == 'v')
    {
        offset = (1,0);
    }
    
    var newPos = ShiftRobotPositionForPartTwo(wideGrid, offset, currRobotPositionForPartTwo);
    currRobotPositionForPartTwo = newPos;
    
    // Console.WriteLine($"Iteration {iteration} and movement applied: {dire}");
    // VisualiseRobots(wideGrid);
    iteration++;
}

Console.WriteLine($"Part 2: {CalculateBoxCoordinations(wideGrid)}");
void VisualiseRobots(char[][] gridData)
{
    for (int i = 0; i < gridData.Length; i++)
    {
        for (int j = 0; j < gridData[i].Length; j++)
        {
            Console.Write(gridData[i][j]);
        }
        Console.WriteLine();
    }
}

long CalculateBoxCoordinations(char[][] gridData)
{
    long res = 0;

    for (int i = 1; i < gridData.Length - 1; i++)
    {
        for (int j = 1; j < gridData[i].Length-1; j++)
        {
            if (gridData[i][j] == 'O' || gridData[i][j] == '[')
            {
                res += 100 * i + j;
            }
        }
    }

    return res;
}

char[][] TranslateToWideGrid(char[][] gridData)
{
    var matrix = new List<char[]>();
    for (int i = 0; i < gridData.Length; i++)
    {
        var row = new char[gridData[i].Length * 2];
        for (int j = 0; j < gridData[i].Length; j++)
        {
            if (gridData[i][j] == '#')
            {
                row[j*2] = '#';    
                row[2*j+1] = '#';    
            } else if (gridData[i][j] == '.')
            {
                row[j*2] = '.';    
                row[2*j+1] = '.';
            } else if (gridData[i][j] == 'O')
            {
                row[j*2] = '[';    
                row[2*j+1] = ']';
            }
            else if (gridData[i][j] == '@')
            {
                row[j*2] = '@';    
                row[2*j+1] = '.';
            }
        }
        matrix.Add(row.ToArray());
    }
    return matrix.ToArray();
}

(int, int) ShiftRobotPosition(char[][] gridData, (int, int) directionOffset, (int, int) robotPosition)
{
    var (x, y) = robotPosition;
    var (dx, dy) = directionOffset; 
    var (shiftedX, shiftedY) = (x+dx, y+dy);
    if (gridData[shiftedX][shiftedY] == '#')
    {
        return (x, y);
    } else if (gridData[shiftedX][shiftedY] == '.')
    {
        gridData[x][y] = '.';
        gridData[shiftedX][shiftedY] = '@';
        return (shiftedX, shiftedY);
    } else if (gridData[shiftedX][shiftedY] == 'O')
    {
        var (nextFreeX, nextFreeY) = (shiftedX + dx, shiftedY + dy);
        while (true)
        {
            if (gridData[nextFreeX][nextFreeY] == '.') break;
            else if (gridData[nextFreeX][nextFreeY] == '#')
            {
                nextFreeX = shiftedX;
                nextFreeY = shiftedY;
                break;
            }
            nextFreeX += dx;
            nextFreeY += dy;
        }

        if (shiftedX == nextFreeX && shiftedY == nextFreeY)
        {
            return (x, y);
        }
        gridData[x][y] = '.';
        gridData[shiftedX][shiftedY] = '@';
        gridData[nextFreeX][nextFreeY] = 'O';
        return (shiftedX, shiftedY);
    }
    throw new Exception("I don't think this will ever happen");

}

(int, int) ShiftRobotPositionForPartTwo(char[][] gridData, (int, int) directionOffset, (int, int) robotPosition)
{
    var (x, y) = robotPosition;
    var (dx, dy) = directionOffset; 
    var (shiftedX, shiftedY) = (x+dx, y+dy);
    if (gridData[shiftedX][shiftedY] == '#')
    {
        return (x, y);
    } else if (gridData[shiftedX][shiftedY] == '.')
    {
        gridData[x][y] = '.';
        gridData[shiftedX][shiftedY] = '@';
        return (shiftedX, shiftedY);
    } else if (gridData[shiftedX][shiftedY] == '[' || gridData[shiftedX][shiftedY] == ']')
    {
        // shift left or right
        if (dx == 0)
        {
            var (nextFreeX, nextFreeY) = (shiftedX + dx, shiftedY + dy);
            while (true)
            {
                if (gridData[nextFreeX][nextFreeY] == '.') break;
                else if (gridData[nextFreeX][nextFreeY] == '#')
                {
                    nextFreeX = shiftedX;
                    nextFreeY = shiftedY;
                    break;
                }
                nextFreeX += dx;
                nextFreeY += dy;
            }

            if (shiftedX == nextFreeX && shiftedY == nextFreeY)
            {
                return (x, y);
            }
            

            if (dy == -1)
            {
                // shift everything left
                gridData[nextFreeX][nextFreeY] = '[';
                for (int shiftLeft = nextFreeY + 1; shiftLeft < y; shiftLeft++)
                {
                    if (gridData[nextFreeX][shiftLeft] == ']')
                    {
                        gridData[nextFreeX][shiftLeft] = '[';
                    }
                    else
                    {
                        gridData[nextFreeX][shiftLeft] = ']';
                    }
                }
            }
            else
            {
                // shift everything right
                gridData[nextFreeX][nextFreeY] = ']';
                for (int shiftRight = nextFreeY - 1; shiftRight > y; shiftRight--)
                {
                    if (gridData[nextFreeX][shiftRight] == ']')
                    {
                        gridData[nextFreeX][shiftRight] = '[';
                    }
                    else
                    {
                        gridData[nextFreeX][shiftRight] = ']';
                    }
                }
            }
            gridData[x][y] = '.';
            gridData[shiftedX][shiftedY] = '@';
            return (shiftedX, shiftedY);
        }
        // up or down direction
        else
        {
            var box = new Box()
            {
                Lhs = gridData[shiftedX][shiftedY] == '[' ? (shiftedX, shiftedY) : (shiftedX, shiftedY -1),
                Rhs = gridData[shiftedX][shiftedY] == ']' ? (shiftedX, shiftedY) : (shiftedX, shiftedY +1),
            };
            var toShiftBoxes = new List<Box>() { box };
            var (nextFreeX, nextFreeY) = (shiftedX + dx, shiftedY + dy);

            while (true)
            {
                if (toShiftBoxes.Where(b=>b.Lhs.Item1 == nextFreeX - dx).All(b => b.DoesItClear(dx, gridData)))
                {
                    break;
                }

                if (toShiftBoxes.Where(b => b.Lhs.Item1 == nextFreeX - dx).Any(b => b.DidItHitObstacle(dx, gridData)))
                {
                    return (x, y);
                }
                
                
                toShiftBoxes.AddRange(GetConnectedBoxes(gridData, toShiftBoxes, nextFreeX, dx));
                nextFreeX += dx;
                
            }
            
            var orderBoxes = dx == 1 ? toShiftBoxes.OrderByDescending(b => b.Lhs.Item1) : 
                toShiftBoxes.OrderBy(b => b.Lhs.Item1);
            
            foreach (var singleBox in orderBoxes)
            {
                var (lx, ly) = singleBox.Lhs;
                gridData[lx+dx][ly] = '[';
                var (rx, ry) = singleBox.Rhs;
                gridData[rx+dx][ry] = ']';
                
                gridData[lx][ly] = '.';
                gridData[rx][ry] = '.';
            }
            
            gridData[x][y] = '.';
            gridData[shiftedX][shiftedY] = '@';
            return (shiftedX, shiftedY);
        }
        
    }
    throw new Exception("I don't think this will ever happen");
}

List<Box> GetConnectedBoxes(char[][] gridData, List<Box> inputBox, int anchorX, int offsetX)
{
    int anchorRow = anchorX;
    int[] yCoords = inputBox.Where(x=> (x.Lhs.Item1 == anchorX - offsetX) )
        .Select(b => (int[])[b.Lhs.Item2, b.Rhs.Item2]).SelectMany(x=>x).ToArray();
    var connectedBoxes = new HashSet<Box>();
    foreach (var y in yCoords)
    {
        if (gridData[anchorRow][y] == '[')
        {
            connectedBoxes.Add(new Box()
            {
                Lhs = (anchorRow, y),
                Rhs = (anchorRow, y+1),
            });
        }
        else if (gridData[anchorRow][y] == ']')
        {
            connectedBoxes.Add(new Box()
            {
                Lhs = (anchorRow, y-1),
                Rhs = (anchorRow, y),
            });
        }
    }

    return connectedBoxes.ToList();
}

(int, int) FindRobotPosition(char[][] gridData)
{
    for (int i = 0; i < gridData.Length; i++)
    {
        for (int j = 0; j < gridData[i].Length; j++)
        {
            if (gridData[i][j] == '@')
            {
                return (i, j);
            }
        }
    }
    return (-1, -1);
}

public record struct Box
{
    public(int, int) Lhs { get; set; }
    public(int, int) Rhs { get; set; }


    public bool DoesItClear(int offset, char[][] gridData)
    {
        foreach (var (boxSideX, boxSideY) in ((int, int)[])[Lhs, Rhs])
        {
            if (gridData[boxSideX+offset][boxSideY] != '.') return false;
        }
        return true;
    }
    
    public bool DidItHitObstacle(int offset, char[][] gridData)
    {
        foreach (var (boxSideX, boxSideY) in ((int, int)[])[Lhs, Rhs])
        {
            if (gridData[boxSideX+offset][boxSideY] == '#') return true;
        }
        return false;
    }
}