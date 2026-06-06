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
            // Sortera genom att plocka frÃ¥n heapen 
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



        public void QuickSort(IList<T> collection)
        {
            QuickSortRecursive(collection, 0, collection.Count - 1);
        }

        private void QuickSortRecursive(IList<T> collection, int low, int high)
        {
            if (low >= high)
                return;

            int pivotIndex = Partition(collection, low, high);

            QuickSortRecursive(collection, low, pivotIndex - 1);
            QuickSortRecursive(collection, pivotIndex + 1, high);
        }

        private int Partition(IList<T> collection, int low, int high)
        {
            T pivot = collection[high];
            int i = low - 1;

            for (int j = low; j < high; j++)
            {
                if (((IComparable<T>)collection[j]).CompareTo(pivot) <= 0)
                {
                    i++;
                    T temp = collection[i];
                    collection[i] = collection[j];
                    collection[j] = temp;
                }
            }

            T temp2 = collection[i + 1];
            collection[i + 1] = collection[high];
            collection[high] = temp2;

            return i + 1;
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