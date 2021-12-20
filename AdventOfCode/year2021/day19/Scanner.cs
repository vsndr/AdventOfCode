using AdventOfCode.shared.dataStructures;
using AdventOfCode.shared.utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.year2021.day19
{
    public class Scanner
    {
        private const int MATCH_MINIMUM = 12;

        private HashSet<Vector3> visibleBeacons;
        private Dictionary<Vector3, List<Vector3>> beaconDistances;     //Maps each beacon location to the distances with all other beacons
        private Vector3 scannerPosition;

        public Scanner(HashSet<Vector3> visibleBeacons)
        {
            this.visibleBeacons = visibleBeacons;
            this.beaconDistances = CalculateBeaconDistances(visibleBeacons);
        }

        private Dictionary<Vector3, List<Vector3>> CalculateBeaconDistances(HashSet<Vector3> beacons)
        {
            var result = new Dictionary<Vector3, List<Vector3>>();
            foreach (var beaconFrom in beacons)
            {
                result[beaconFrom] = new List<Vector3>();
                foreach (var beaconTo in beacons)
                {
                    if (beaconFrom != beaconTo)  //Don't calculate the distance to itself
                    {
                        result[beaconFrom].Add(beaconFrom - beaconTo);
                    }
                }
            }
            return result;
        }

        public Vector3 GetScannerPosition()
        {
            return this.scannerPosition;
        }

        public void SetScannerPosition(Vector3 position)
        {
            this.scannerPosition = position;
        }

        public void AddBeacons(IEnumerable<Vector3> beaconsToAdd)
        {
            this.visibleBeacons = this.visibleBeacons.Union(beaconsToAdd).ToHashSet();
            //Recreate a map of beacon distances
            this.beaconDistances = this.CalculateBeaconDistances(this.visibleBeacons);
        }

        public Dictionary<Vector3, List<Vector3>> GetBeaconDistances()
        {
            return this.beaconDistances;
        }

        public HashSet<Vector3> GetBeacons()
        {
            return this.visibleBeacons;
        }
        
        public bool TryAddScanner(Scanner other)
        {
            //Check if the other scanner can be matched with this scanner
            if (this.TryMatchScanner(other, out var translation))
            {
                //If the scanners can match, translate all beacons from the other scanner and add them to this scanner
                var beaconsToAdd = other.GetBeacons();
                var translatedBeacons = beaconsToAdd.Select(x => translation.rotationTransformation(x) + translation.positionTransformation).ToHashSet();
                this.AddBeacons(translatedBeacons);
                other.SetScannerPosition(translation.positionTransformation);
                return true;
            }
            return false;
        }


        /// <summary>
        /// Tries to match this scanner with the other scanner. This method returns true if scanners can be matched, false otherwise.
        /// If the scanners can be matched, the output translation contains the transformations that are required to translate the beacons of the other scanner to the same Euclidean space as this scanner.
        /// </summary>
        /// <param name="other"></param>
        /// <param name="translation"></param>
        /// <returns></returns>
        private bool TryMatchScanner(Scanner other, out (Func<Vector3, Vector3> rotationTransformation, Vector3 positionTransformation) translation)
        {
            var otherBeaconDistances = other.GetBeaconDistances();
            var matchedBeacons = new List<(Vector3 ownBeacon, Vector3 otherBeacon)>();
            Func<Vector3, Vector3> rotationFunction = null;

            //For each beacon between 2 scanners, check if they can be matched with each other
            foreach (var beaconOwn in this.beaconDistances.Keys)
            {
                var distancesOwn = beaconDistances[beaconOwn];

                foreach (var beaconOther in otherBeaconDistances.Keys)
                {
                    var distancesOther = otherBeaconDistances[beaconOther];
                    if (TryMatchBeaconDistances(distancesOwn, distancesOther, out var func))
                    {
                        matchedBeacons.Add((beaconOwn, beaconOther));
                        rotationFunction = func;
                    }
                }
            }

            //Check if there are at least MATCH_MINIMUM beacons that have been matched
            if (matchedBeacons.Count >= MATCH_MINIMUM)
            {
                var beaconPair = matchedBeacons[0];       //Just get a beacon from the list (doesn't matter which one)
                //Calculate the difference in distance between the beacon as seen by this scanner and the same beacon as seen by the other scanner
                var distance = beaconPair.ownBeacon - rotationFunction(beaconPair.otherBeacon);
                translation = (rotationFunction, distance);
                return true;
            }

            translation = (null, new Vector3());
            return false;
        }

        private bool TryMatchBeaconDistances(List<Vector3> beaconDistances1, List<Vector3> beaconDistances2, out Func<Vector3, Vector3> rotationFunction)
        {
            //First check if the 2 beacons can match at all based on their distances
            if (CanMatchByDistances(beaconDistances1, beaconDistances2))
            {
                //Try to find the rotation with which the 2 beacon distances can match
                var rotationFunctions = this.GetRotationTransformations();
                foreach (var func in rotationFunctions)
                {
                    //Rotate the distances of the 2nd beacon by the rotation function
                    var beaconDistances2Rotated = beaconDistances2.Select(x => func(x)).ToList();
                    var numMatchingDistances = beaconDistances1.Supersect(beaconDistances2Rotated).Count();
                    if (numMatchingDistances >= MATCH_MINIMUM - 1)
                    {
                        rotationFunction = func;
                        return true;
                    }
                }
            }
            rotationFunction = null;
            return false;
        }


        private bool CanMatchByDistances(List<Vector3> beaconDistances1, List<Vector3> beaconDistances2)
        {
            //A beacon that's seen by 2 different scanners can only be matched if they have at least MATCH_MINIMUM - 1 of the same distances to other beacons
            var absoluteDistances1 = beaconDistances1.Select(x => (int)x.GetMagnitude()).ToList();
            var absoluteDistances2 = beaconDistances2.Select(x => (int)x.GetMagnitude()).ToList();
            //Supersect is almost the same as intersect, except it also counts duplicates
            return absoluteDistances1.Supersect(absoluteDistances2).Count() >= MATCH_MINIMUM - 1;
        }

        private IEnumerable<Func<Vector3, Vector3>> GetRotationTransformations()
        {
            yield return (Vector3 v) => v;
            yield return (Vector3 v) => new Vector3(v.x, -v.y, -v.z);
            yield return (Vector3 v) => new Vector3(v.x, v.z, -v.y);
            yield return (Vector3 v) => new Vector3(v.x, -v.z, v.y);

            yield return (Vector3 v) => new Vector3(-v.x, v.z, v.y);
            yield return (Vector3 v) => new Vector3(-v.x, -v.y, v.z);
            yield return (Vector3 v) => new Vector3(-v.x, v.y, -v.z);
            yield return (Vector3 v) => new Vector3(-v.x, -v.z, -v.y);

            yield return (Vector3 v) => new Vector3(v.y, v.z, v.x);
            yield return (Vector3 v) => new Vector3(v.y, -v.x, v.z);
            yield return (Vector3 v) => new Vector3(v.y, v.x, -v.z);
            yield return (Vector3 v) => new Vector3(v.y, -v.z, -v.x);

            yield return (Vector3 v) => new Vector3(-v.y, v.x, v.z);
            yield return (Vector3 v) => new Vector3(-v.y, -v.z, v.x);
            yield return (Vector3 v) => new Vector3(-v.y, v.z, -v.x);
            yield return (Vector3 v) => new Vector3(-v.y, -v.x, -v.z);

            yield return (Vector3 v) => new Vector3(v.z, v.x, v.y);
            yield return (Vector3 v) => new Vector3(v.z, -v.y, v.x);
            yield return (Vector3 v) => new Vector3(v.z, v.y, -v.x);
            yield return (Vector3 v) => new Vector3(v.z, -v.x, -v.y);

            yield return (Vector3 v) => new Vector3(-v.z, v.y, v.x);
            yield return (Vector3 v) => new Vector3(-v.z, -v.x, v.y);
            yield return (Vector3 v) => new Vector3(-v.z, v.x, -v.y);
            yield return (Vector3 v) => new Vector3(-v.z, -v.y, -v.x);
        }

    }
}
