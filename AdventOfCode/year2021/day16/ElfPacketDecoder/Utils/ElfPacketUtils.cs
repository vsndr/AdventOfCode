using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode.year2021.day16.ElfPacketDecoder.Utils
{
    public static class ElfPacketUtils
    {

        private static readonly Dictionary<char, string> hexCharacterToBinary = new Dictionary<char, string> {
            { '0', "0000" },
            { '1', "0001" },
            { '2', "0010" },
            { '3', "0011" },
            { '4', "0100" },
            { '5', "0101" },
            { '6', "0110" },
            { '7', "0111" },
            { '8', "1000" },
            { '9', "1001" },
            { 'A', "1010" },
            { 'B', "1011" },
            { 'C', "1100" },
            { 'D', "1101" },
            { 'E', "1110" },
            { 'F', "1111" }
        };


        public static string HexToBinary(string hexString)
        {
            StringBuilder result = new StringBuilder();
            foreach (char c in hexString)
            {
                result.Append(hexCharacterToBinary[c]);
            }
            return result.ToString();
        }

        public static long BinaryToNumber(string binaryString)
        {
            return Convert.ToInt64(binaryString, 2);
        }
    }
}
