using AdventOfCode.shared.dataStructures;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.year2021.day19
{
    public class ScannerGroup
    {
        private readonly List<Scanner> scanners;

        public ScannerGroup(List<Scanner> scanners)
        {
            this.scanners = scanners;
        }

        public HashSet<Vector3> GetAllBeacons()
        {
            this.MatchScanners();
            return this.scanners.First().GetBeacons();
        }

        public List<Scanner> GetMatchedScanners()
        {
            this.MatchScanners();
            return this.scanners;
        }

        private void MatchScanners()
        {
            //Keep adding Scanners to the first scanner until no scanners remain
            var scannersCopy = new List<Scanner>(this.scanners);
            var firstScanner = scannersCopy[0];
            scannersCopy.RemoveAt(0);
            while (scannersCopy.Any())
            {
                for (int i = 0; i < scannersCopy.Count; i++)
                {
                    var scannerToMatch = scannersCopy[i];
                    if (firstScanner.TryAddScanner(scannerToMatch))
                    {
                        scannersCopy.RemoveAt(i);
                        break;
                    }
                }
            }
        }


    }
}
