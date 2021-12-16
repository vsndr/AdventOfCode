using AdventOfCode.year2021.day16.ElfPacketDecoder.Utils;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode.year2021.day16.ElfPacketDecoder
{
    public class LiteralPacket : PacketBody
    {
        private long literalValue; 

        public LiteralPacket(ref string binaryString) : base(ref binaryString)
        {
            this.Parse(ref binaryString);
        }


        private void Parse(ref string binaryString)
        {
            const int chunkSize = 5;
            var stringBuilder = new StringBuilder();
            var isLast = false;
            while (binaryString.Length >= chunkSize && !isLast)
            {
                isLast = binaryString[0] == '0';
                var chunkValue = binaryString.Substring(1, chunkSize-1);
                binaryString = binaryString.Substring(chunkSize);
                stringBuilder.Append(chunkValue);
            }

            this.literalValue = ElfPacketUtils.BinaryToNumber(stringBuilder.ToString());
        }

        public override List<int> GetAllVersionNumbers()
        {
            return new List<int>();
        }

        public override long GetValue()
        {
            return this.literalValue;
        }
    }
}
