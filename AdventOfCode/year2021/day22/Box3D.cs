using System;
using System.Collections.Generic;

namespace AdventOfCode.year2021.day22
{
    public class Box3D
    {
        public readonly (int from, int to) x;
        public readonly (int from, int to) y;
        public readonly (int from, int to) z;

        public Box3D((int from, int to) x, (int from, int to) y, (int from, int to) z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        /// <summary>
        /// Returns a list of cubes that you would get if you were to remove the given cube from this cube.
        /// </summary>
        /// <returns></returns>
        public List<Box3D> RemoveFromBox(Box3D boxToRemove)
        {
            var result = new List<Box3D>();

            if (boxToRemove.x.from <= this.x.from && boxToRemove.x.to >= this.x.to &&
                boxToRemove.y.from <= this.y.from && boxToRemove.y.to >= this.y.to &&
                boxToRemove.z.from <= this.z.from && boxToRemove.z.to >= this.z.to)
            {
                //If this box fits entirely in the box to remove, the result is empty
                return result;
            }
            else if (TryGetOverlappingBox(boxToRemove, out var overlappingBox))
            {
                if (this.z.to > overlappingBox.z.to)
                {
                    //Take a slice from the upper part of the box
                    var slice = new Box3D(this.x, this.y, (overlappingBox.z.to, this.z.to));
                    result.Add(slice);
                }
                if (this.z.from < overlappingBox.z.from)
                {
                    //Take a slice from the lower part of the box
                    var slice = new Box3D(this.x, this.y, (this.z.from, overlappingBox.z.from));
                    result.Add(slice);
                }
                if (this.x.from < overlappingBox.x.from)
                {
                    //Take a slice from the left part of the box
                    var slice = new Box3D((this.x.from, overlappingBox.x.from), this.y, overlappingBox.z);
                    result.Add(slice);
                }
                if (this.x.to > overlappingBox.x.to)
                {
                    //Take a slice from the right part of the box
                    var slice = new Box3D((overlappingBox.x.to, this.x.to), this.y, overlappingBox.z);
                    result.Add(slice);
                }
                if (this.y.from < overlappingBox.y.from)
                {
                    //Take a slice from the front of the box
                    var slice = new Box3D(overlappingBox.x, (this.y.from, overlappingBox.y.from), overlappingBox.z);
                    result.Add(slice);
                }
                if (this.y.to > overlappingBox.y.to)
                {
                    //Take a slice from the back of the box
                    var slice = new Box3D(overlappingBox.x, (overlappingBox.y.to, this.y.to), overlappingBox.z);
                    result.Add(slice);
                }
            }
            else
            {
                //If nothing was removed, the result is just the current box
                result.Add(this);
            }

            return result;
        }

        private bool TryGetOverlappingBox(Box3D other, out Box3D overlappingBox)
        {
            if (TryGetIntersection(this.x, other.x, out var xIntersection) &&
               TryGetIntersection(this.y, other.y, out var yIntersection) &&
               TryGetIntersection(this.z, other.z, out var zIntersection))
            {
                overlappingBox = new Box3D(xIntersection, yIntersection, zIntersection);
                return true;
            }
            overlappingBox = null;
            return false;
        }


        /// <summary>
        /// Finds the intersection between 2 lines
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        private bool TryGetIntersection((int from, int to) v1, (int from, int to) v2, out (int from, int to) intersection)
        {
            //2 lines can overlap in 4 different kind of ways:

            //V1 overlaps with V2 at the left side:
            //V1: ______
            //V2:   _________     
            if (v1.from <= v2.from && v1.to > v2.from)
            {
                intersection = (v2.from, (int)Math.Min(v1.to, v2.to));
                return true;
            }
            //V1 overlaps with V2 at the right side:
            //V1:        ________
            //V2:   _________   
            else if (v2.from <= v1.from && v2.to > v1.from)
            {
                intersection = (v1.from, (int)Math.Min(v2.to, v1.to));
                return true;
            }
            //V1 completely falls in the range of V2:
            //V1:     _______
            //V2:  ______________
            else if (v1.from >= v2.from && v1.to <= v2.to)
            {
                intersection = (v1.from, v1.to);
                return true;
            }
            //V2 completely falls in the range of V1:
            //V1:  ______________
            //V2:     _______
            else if (v2.from >= v1.from && v2.to <= v1.to)
            {
                intersection = (v2.from, v2.to);
                return true;
            }
            //The lines don't overlap at all
            else
            {
                intersection = (0, 0);
                return false;
            }
        }


        public long GetSize()
        {
            return ((long)(x.to - x.from)) * ((long)(y.to - y.from)) * ((long)(z.to - z.from));
        }

        public long GetSize((int from, int to) x, (int from, int to) y, (int from, int to) z)
        {
            //Just create a box from the ranges and calculate the intersection with that box
            var boxRange = new Box3D(x, y, z);
            if (this.TryGetOverlappingBox(boxRange, out var intersectingBox))
            {
                return intersectingBox.GetSize();
            }
            //If nothing is in the given range, the size is 0
            return 0;
        }


    }
}
