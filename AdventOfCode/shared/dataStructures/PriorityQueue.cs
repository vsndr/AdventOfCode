using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.shared.dataStructures
{
    public class PriorityQueue<T> where T : struct
    {
        private SortedDictionary<int, List<T>> queue = new SortedDictionary<int, List<T>>();

        public void Add(T item, int priority)
        {
            if (this.queue.TryGetValue(priority, out var currentItems))
            {
                currentItems.Add(item);
            }
            else
            {
                queue.Add(priority, new List<T> { item });
            }
        }

        public T Pop()
        {
            var highestPrioItems = queue.FirstOrDefault();
            if (highestPrioItems.Value != null)
            {
                var firstHighestPrioItem = highestPrioItems.Value.First();
                highestPrioItems.Value.RemoveAt(0);
                if (!highestPrioItems.Value.Any())
                {
                    queue.Remove(highestPrioItems.Key);
                }
                return firstHighestPrioItem;
            }

            throw new System.Exception("No items to pop");
        }


        public bool TryPop(out T? output)
        {
            var highestPrioItems = queue.FirstOrDefault();
            if (highestPrioItems.Value != null)
            {
                output = highestPrioItems.Value.First();
                highestPrioItems.Value.RemoveAt(0);
                if (!highestPrioItems.Value.Any())
                {
                    queue.Remove(highestPrioItems.Key);
                }
                return true;
            }

            output = null;
            return false;
        }

    }
}
