using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmLib
{
    /// <summary>
    /// Implementation av olika sökalgoritmer för generiska listor.
    /// </summary>
    /// <typeparam name="T">Typen på elementen som ska sökas i. Måste implementera IComparable<T>.</typeparam>

    public class SearchingManager<T> : ISearchingManager<T> where T : IComparable<T>
    {
        /// <summary>
        /// Utför binär sökning i en sorterad lista.
        /// </summary>
        /// <param name="collection">Sorterad lista att söka i.</param>
        /// <param name="target">Värdet som söks.</param>
        /// <returns>Index för träff eller -1 om inget hittas.</returns>
        public int BinarySearch(IList<T> collection, T target)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            return BinarySearchInternal(collection, target, 0, collection.Count - 1);
        }

        private int BinarySearchInternal(IList<T> collection, T target, int low, int high)
        {
            while (low <= high)
            {
                int mid = low + ((high - low) >> 1);
                int comparison = collection[mid].CompareTo(target);

                if (comparison == 0)
                {
                    return mid;
                }

                if (comparison < 0)
                {
                    low = mid + 1;
                }
                else
                {
                    high = mid - 1;
                }
            }

            return -1;
        }

        /// <summary>
        /// Utför jump search i en sorterad lista.
        /// </summary>
        /// </summary>
        /// <param name="collection">Sorterad lista att söka i.</param>
        /// <param name="target">Värdet som söks.</param>
        /// <returns>Index för träff eller -1 om inget hittas.</returns>
        public int JumpSearch(IList<T> collection, T target)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            int count = collection.Count;
            if (count == 0) return -1;

            int step = (int)Math.Floor(Math.Sqrt(count));
            int prev = 0;

            while (prev < count && collection[Math.Min(count - 1, prev + step)].CompareTo(target) < 0)
            {
                prev += step;
            }

            int end = Math.Min(count - 1, prev + step);
            for (int i = prev; i <= end; i++)
            {
                if (collection[i].CompareTo(target) == 0)
                {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// Utför linjär sökning i en lista.
        /// </summary>
        /// <param name="collection">Listan att söka i.</param>
        /// <param name="target">Värdet som söks.</param>
        /// <returns>Index för träff eller -1 om inget hittas.</returns>
        public int LinearSearch(IList<T> collection, T target)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));

            for (int i = 0; i < collection.Count; i++)
            {
                if (collection[i].CompareTo(target) == 0)
                {
                    return i;
                }
            }

            return -1;
        }

    }
}
