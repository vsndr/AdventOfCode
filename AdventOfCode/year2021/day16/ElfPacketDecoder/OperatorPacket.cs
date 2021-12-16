using AdventOfCode.year2021.day16.ElfPacketDecoder.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.year2021.day16.ElfPacketDecoder
{
    public class OperatorPacket : PacketBody
    {

        private List<ElfPacket> subPackets;
        private readonly int packetType;

        public OperatorPacket(ref string binaryString, int packetType) : base(ref binaryString)
        {
            this.packetType = packetType;
            this.Parse(ref binaryString);
        }


        private void Parse(ref string binaryString)
        {
            var lengthType = this.ParseLengthType(ref binaryString);
            if (lengthType == 0)
            {
                this.ParseSubPacketsByLength(ref binaryString);
            }
            else
            {
                this.ParseSubPacketsImmediate(ref binaryString);
            }
        }

        private int ParseLengthType(ref string binaryString)
        {
            const int lengthTypeLength = 1;
            var packetType = binaryString.Substring(0, lengthTypeLength);
            binaryString = binaryString.Substring(lengthTypeLength);
            return (int) ElfPacketUtils.BinaryToNumber(packetType);
        }

        private void ParseSubPacketsImmediate(ref string binaryString)
        {
            var immediateSubPackets = ParseSubPacketImmediateLength(ref binaryString);

            this.subPackets = new List<ElfPacket>();
            for (int i=0; i<immediateSubPackets; i++)
            {
                this.subPackets.Add(new ElfPacket(ref binaryString));
            }
        }

        private int ParseSubPacketImmediateLength(ref string binaryString)
        {
            const int subPacketLength = 11;
            var immediateLengthBinary = binaryString.Substring(0, subPacketLength);
            binaryString = binaryString.Substring(subPacketLength);
            return (int) ElfPacketUtils.BinaryToNumber(immediateLengthBinary);
        }


        private void ParseSubPacketsByLength(ref string binaryString)
        {
            var subPacketLength = this.ParseSubPacketLength(ref binaryString);
            var subPacketString = binaryString.Substring(0, subPacketLength);
            binaryString = binaryString.Substring(subPacketLength);

            this.subPackets = new List<ElfPacket>();
            while (subPacketString.Any())
            {
                this.subPackets.Add(new ElfPacket(ref subPacketString));
            }
        }

        private int ParseSubPacketLength(ref string binaryString)
        {
            const int subPacketLength = 15;
            var subPacketLengthBinary = binaryString.Substring(0, subPacketLength);
            binaryString = binaryString.Substring(subPacketLength);
            return (int) ElfPacketUtils.BinaryToNumber(subPacketLengthBinary);
        }

        public override long GetValue()
        {
            return this.ApplyOperator();
        }

        private long ApplyOperator()
        {
            switch(this.packetType)
            {
                case 0:
                    return this.ApplySum();
                case 1:
                    return this.ApplyProduct();
                case 2:
                    return this.ApplyMinimum();
                case 3:
                    return this.ApplyMaximum();
                case 5:
                    return this.ApplyGreaterThan();
                case 6:
                    return this.ApplyLessThan();
                case 7:
                    return this.ApplyEqualTo();
                default:
                    throw new Exception("Woops!");
            }
        }

        private long ApplySum()
        {
            return this.subPackets.Sum(x => x.GetValue());
        }

        private long ApplyProduct()
        {
            var result = this.subPackets[0].GetValue();
            for (int i= 1; i < this.subPackets.Count; i++)
            {
                result *= this.subPackets[i].GetValue();
            }
            return result;
        }
        private long ApplyMinimum()
        {
            return this.subPackets.Min(x => x.GetValue());
        }

        private long ApplyMaximum()
        {
            return this.subPackets.Max(x => x.GetValue());
        }

        private long ApplyGreaterThan()
        {
            if (this.subPackets.Count != 2)
            {
                throw new Exception("Woops!");
            }

            return this.subPackets[0].GetValue() > this.subPackets[1].GetValue() ? 1 : 0;
        }

        private long ApplyLessThan()
        {
            if (this.subPackets.Count != 2)
            {
                throw new Exception("Woops!");
            }

            return this.subPackets[0].GetValue() < this.subPackets[1].GetValue() ? 1 : 0;
        }

        private long ApplyEqualTo()
        {
            if (this.subPackets.Count != 2)
            {
                throw new Exception("Woops!");
            }

            return this.subPackets[0].GetValue() == this.subPackets[1].GetValue() ? 1 : 0;
        }

        public override List<int> GetAllVersionNumbers()
        {
            return this.subPackets.SelectMany(x => x.GetAllVersionNumbers()).ToList();
        }


    }
}
