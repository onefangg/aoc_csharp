// See https://aka.ms/new-console-template for more information

// actual input
var inputString = @"
Register A: 216584205979245
Register B: 0
Register C: 0

Program: 2,4,1,3,7,5,1,5,0,3,4,2,5,5,3,0";

// var inputString = @"
// Register A: 117440
// Register B: 0
// Register C: 0
//
// Program: 0,3,5,4,3,0";

var parsingInput = inputString.Split("\r\n").Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
var (parsingRegisters, parsingProgram) = (parsingInput[..3], parsingInput[3]);
var registers = parsingRegisters.Select(x =>
{
    var splitRegister = x.Split(": ");
    return long.Parse(splitRegister[1]);
}).ToArray();

var inputProgram = parsingProgram.Split(": ")[^1].Split(",").Select(long.Parse).ToArray();
var outputProgramString = RunProgram(registers[0],registers[1], registers[2], inputProgram);
Console.WriteLine($"Part 1: {outputProgramString}");

ReverseEngineerProgram(0, 0, inputProgram);

// neurons connected thanks to this: https://publish.reddit.com/embed?url=https://www.reddit.com/r/adventofcode/comments/1hhtc6g/comment/m2u1fjh/
void ReverseEngineerProgram(long a, long iter, long[] outputProgram)
{
    if (RunProgram(a, 0, 0, outputProgram) == string.Join(",", outputProgram))
    {
        Console.WriteLine($"Part 2: a is {a}");
        return;
    } 
    var xIterOutput = string.Join(",",outputProgram[Convert.ToInt32(outputProgram.Length -  iter)..]);
    var progOutput = RunProgram(a, 0, 0, outputProgram);
    
    if (progOutput == xIterOutput || iter == 0)
    {
        for (int i =0;i < 8; i++)
        {
            ReverseEngineerProgram(a * 8 + i, iter+1, outputProgram);
        }
    } 
}

string RunProgram(long a, long b, long c, long[] program)
{
    var output = new List<char>();
    long instructionPointer = 0;
    while (instructionPointer < program.Length)
    {
        var opCode = program[instructionPointer];
        var operand = GetValueFromOperand(program[instructionPointer + 1], a, b,c);
        
        // Console.WriteLine($"value of opcode: {opCode}");
        // Console.WriteLine($"value of a: {a}");
        // Console.WriteLine($"value of a in binary: {Convert.ToString(a, 2)}");
        // Console.WriteLine($"value of b: {b}");
        // Console.WriteLine($"value of c: {c}");
        if (opCode == 0) a = (long)Math.Truncate(a / Math.Pow(2, operand));
        else if (opCode == 1) b^= program[instructionPointer + 1];
        else if (opCode == 2) b = operand % 8;
        else if (opCode == 3 && a != 0)
        {
            instructionPointer = program[instructionPointer + 1];
            continue;
        }
        else if (opCode == 4) b ^= c;
        else if (opCode == 5) output.Add(char.Parse((operand % 8).ToString()));
        else if (opCode == 6) b = (long)Math.Truncate(a / Math.Pow(2, operand));
        else if (opCode == 7) c = (long)Math.Truncate(a / Math.Pow(2, operand));
        instructionPointer += 2;
    }
    return String.Join(",", output);
}

long GetValueFromOperand(long operand, long a, long b, long c)
{
    switch (operand)
    {
        case 0:
        case 1:
        case 2:
        case 3:
            return long.Parse(operand.ToString());
        case 4: return a;
        case 5: return b;
        case 6: return c;
    }
    return -1;
}