using AdventOfCode.year2021.day16.ElfPacketDecoder.Utils;
using System.Collections.Generic;

namespace AdventOfCode.year2021.day16.ElfPacketDecoder
{
    public class ElfPacket
    {

        private int versionNumber;
        private int packetType;
        private PacketBody packetBody;

        public ElfPacket(ref string binaryString)
        {
            this.Parse(ref binaryString);
        }


        private void Parse(ref string binaryString)
        {
            ParseVersionNumber(ref binaryString);
            ParsePacketType(ref binaryString);

            if (this.packetType == 4)
            {
                this.packetBody = new LiteralPacket(ref binaryString);
            }
            else
            {
                this.packetBody = new OperatorPacket(ref binaryString, this.packetType);
            }
        }

        private void ParseVersionNumber(ref string binaryString)
        {
            const int versionLength = 3;
            var version = binaryString.Substring(0, versionLength);
            binaryString = binaryString.Substring(versionLength);
            this.versionNumber = (int) ElfPacketUtils.BinaryToNumber(version);
        }

        private void ParsePacketType(ref string binaryString)
        {
            const int packetTypeLength = 3;
            var packetType = binaryString.Substring(0, packetTypeLength);
            binaryString = binaryString.Substring(packetTypeLength);
            this.packetType = (int) ElfPacketUtils.BinaryToNumber(packetType);
        }

        public long GetValue()
        {
            return this.packetBody.GetValue();
        }


        public List<int> GetAllVersionNumbers()
        {
            var allNumbers = new List<int> { versionNumber };
            allNumbers.AddRange(this.packetBody.GetAllVersionNumbers());
            return allNumbers;
        }

    }
}
