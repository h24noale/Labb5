using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmLib
{
    public class SortingManager<T> : ISortingManager<T> where T : IComparable<T>
    {
        public void BubbleSort(IList<T> collection)
        {
            int n = collection.Count;

            for (int i = 0; i < n - 1; i++)
            {
                bool swapped = false;

                for (int j = 0; j < n - i - 1; j++)
                {
                    if (collection[j].CompareTo(collection[j + 1]) > 0)
                    {
                        T temp = collection[j];
                        collection[j] = collection[j + 1];
                        collection[j + 1] = temp;

                        swapped = true;
                    }
                }

                if (!swapped)
                    break;
            }
        }
      
        public void InsertionSort(IList<T> collection)
        {
            for (int i = 1; i < collection.Count; i++)
            {
                T current = collection[i];
                int j = i - 1;

                while (j >= 0 &&
                       collection[j].CompareTo(current) > 0)
                {
                    collection[j + 1] = collection[j];
                    j--;
                }

                collection[j + 1] = current;
            }
        }
        public void MergeSort(IList<T> collection)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Sorterar listan med Heap Sort-algoritmen.
        /// </summary>
        /// <param name="collection">Listan som ska sorteras.</param>
        public void HeapSort(IList<T> collection)
        {
            throw new NotImplementedException();
        }

        
        /// <summary>
        /// Sorterar listan med Quick Sort-algoritmen.
        /// </summary>
        /// <param name="collection">Listan som ska sorteras.</param>
        public void QuickSort(IList<T> collection)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sorterar listan med Selection Sort-algoritmen.
        /// </summary>
        /// <param name="collection">Listan som ska sorteras.</param>
        public void SelectionSort(IList<T> collection)
        {
            throw new NotImplementedException();
        }
    }
}
