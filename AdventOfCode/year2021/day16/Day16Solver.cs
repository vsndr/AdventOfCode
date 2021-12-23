using AdventOfCode.year2021.day16.ElfPacketDecoder;
using AdventOfCode.year2021.day16.ElfPacketDecoder.Utils;
using System.Linq;

public class Day16Solver
{
    private const string inputLocation = "D:/CSharpProjects/AdventOfCode/AdventOfCode/AdventOfCode/year2021/day16/day16Input.txt";

    public int SolvePart1()
    {
        var hexString = ParseFromFile(inputLocation);
        var binaryString = ElfPacketUtils.HexToBinary(hexString);
        var elfPacket = new ElfPacket(ref binaryString);
        return elfPacket.GetAllVersionNumbers().Sum();
    }

    public long SolvePart2()
    {
        var hexString = ParseFromFile(inputLocation);
        var binaryString = ElfPacketUtils.HexToBinary(hexString);
        var elfPacket = new ElfPacket(ref binaryString);
        return elfPacket.GetValue();
    }

    private string ParseFromFile(string fileLocation)
    {
        return System.IO.File.ReadAllText(fileLocation);
    }

}
