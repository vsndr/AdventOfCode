using System;
using System.Collections.Generic;

namespace AdventOfCode.shared.utils
{
    public static class ListUtils
    {

        /// <summary>
        /// Removes the first item that meets the given criterium and returns it. If no item was found that meets the given criterium, null is returned.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static T RemoveFirstOrDefaultAndGet<T>(this List<T> list, Func<T, bool> predicate) where T : class
        {
            for (int i=list.Count-1; i>=0; i--)
            {
                if (predicate(list[i]))
                {
                    var item = list[i];
                    list.RemoveAt(i);
                    return item;
                }
            }
            return null;
        }


        /// <summary>
        /// Removes the first item that meets the given criterium and returns it. If no item was found that meets the given criterium, this method throws an exception.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static T RemoveFirstAndGet<T>(this List<T> list, Func<T, bool> predicate) where T : class
        {
            for (int i = list.Count-1; i >= 0; i--)
            {
                if (predicate(list[i]))
                {
                    var item = list[i];
                    list.RemoveAt(i);
                    return item;
                }
            }

            throw new Exception("No item was found that meets the criterium.");
        }
    }
}
