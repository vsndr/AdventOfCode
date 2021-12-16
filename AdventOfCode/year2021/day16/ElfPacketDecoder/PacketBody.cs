using System.Collections.Generic;

namespace AdventOfCode.year2021.day16.ElfPacketDecoder
{
    public abstract class PacketBody
    {
        public PacketBody(ref string binaryString)
        {
        }

        public abstract List<int> GetAllVersionNumbers();

        public abstract long GetValue();
    }
}
