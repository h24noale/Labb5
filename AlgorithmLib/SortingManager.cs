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
            if (collection.Count <= 1)
                return;

            int middle = collection.Count / 2;

            List<T> left = collection.Take(middle).ToList();
            List<T> right = collection.Skip(middle).ToList();

            MergeSort(left);
            MergeSort(right);

            Merge(collection, left, right);
        }

        private void Merge(IList<T> collection, IList<T> left, IList<T> right)
        {
            int i = 0;
            int j = 0;
            int k = 0;

            while (i < left.Count && j < right.Count)
            {
                if (left[i].CompareTo(right[j]) <= 0)
                {
                    collection[k] = left[i];
                    i++;
                }
                else
                {
                    collection[k] = right[j];
                    j++;
                }

                k++;
            }

            while (i < left.Count)
            {
                collection[k] = left[i];
                i++;
                k++;
            }

            while (j < right.Count)
            {
                collection[k] = right[j];
                j++;
                k++;
            }
        }
        public void HeapSort(IList<T> collection)
        {
            int n = collection.Count;
            // Bygg max-heap 
            for (int i = n / 2 - 1; i >= 0; i--)
            {
                int root = i;
                while (true)
                {
                    int left = 2 * root + 1;
                    int right = 2 * root + 2;
                    int largest = root;
                    if (left < n &&
                    ((IComparable<T>)collection[left]).CompareTo(collection[largest]) > 0)
                    {
                        largest = left;
                    }
                    if (right < n &&
                    ((IComparable<T>)collection[right]).CompareTo(collection[largest]) > 0)
                    {
                        largest = right;
                    }
                    if (largest == root)
                        break;
                    T temp = collection[root];
                    collection[root] = collection[largest];
                    collection[largest] = temp;
                    root = largest;
                }
            }
            // Sortera genom att plocka från heapen 
            for (int i = n - 1; i > 0; i--)
            {
                T temp = collection[0];
                collection[0] = collection[i];
                collection[i] = temp;
                int root = 0;
                int heapSize = i;
                while (true)
                {
                    int left = 2 * root + 1;
                    int right = 2 * root + 2;
                    int largest = root;
                    if (left < heapSize &&
                    ((IComparable<T>)collection[left]).CompareTo(collection[largest]) > 0)
                    {
                        largest = left;
                    }
                    if (right < heapSize &&
                    ((IComparable<T>)collection[right]).CompareTo(collection[largest]) > 0)
                    {
                        largest = right;
                    }
                    if (largest == root)
                        break;
                    temp = collection[root];
                    collection[root] = collection[largest];
                    collection[largest] = temp;
                    root = largest;
                }
            }
        }



        /// <summary>
        /// Sorterar listan med Quick Sort-algoritmen.
        /// </summary>
        /// <param name="collection">Listan som ska sorteras.</param>
        public void QuickSort(IList<T> collection)
        {
            throw new NotImplementedException();
        }

        public void SelectionSort(IList<T> collection)
        {
            int n = collection.Count;

            for (int i = 0; i < n - 1; i++)
            {
                int minIndex = i;

                for (int j = i + 1; j < n; j++)
                {
                    if (collection[j].CompareTo(collection[minIndex]) < 0)
                    {
                        minIndex = j;
                    }
                }

                T temp = collection[i];
                collection[i] = collection[minIndex];
                collection[minIndex] = temp;
            }
        }
    }
}
