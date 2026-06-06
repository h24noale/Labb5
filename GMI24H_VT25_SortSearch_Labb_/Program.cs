using AlgorithmLib;
using System.Diagnostics;


namespace GMI24H_VT25_SortSearch_Labb_
{
    using AlgorithmLib;
    using System.Diagnostics;

    namespace GMI24H_VT25_SortSearch_Labb_
    {
        internal class Program
        {
            static void Main(string[] args)
            {
                const int numberOfPosts = 100000;
                const int seed = 123;

                var generator = new RandomLogGenerator();
                var logs = generator.GenerateLogs(numberOfPosts, seed).ToList();

                Console.WriteLine("Förhandsvisning av loggdata:");

                foreach (var entry in logs.Take(5))
                {
                    Console.WriteLine(entry);
                }

                // Vi ska sortera strängar (IP-adresser)
                var sorter = new SortingManager<string>();

                // Vi ska söka bland strängar (IP-adresser)
                var searcher = new SearchingManager<string>();

                // Plocka ut IP-adresser ur loggarna
                List<string> ipAddresses =
                    logs.Select(entry => entry.IpAddress).ToList();

                // Skapa en kopia så att originaldatan inte ändras
                List<string> testData =
                    new List<string>(ipAddresses);

                Stopwatch sw = Stopwatch.StartNew();

                // Välj den algoritm du vill testa
                sorter.BubbleSort(testData);

                sorter.InsertionSort(testData);
                // sorter.SelectionSort(testData);
                // sorter.MergeSort(testData);

                sw.Stop();

                TimeSpan elapsedTime = sw.Elapsed;

                Console.WriteLine();
                Console.WriteLine($"Sorteringstid: {elapsedTime.TotalMilliseconds} ms");

                Console.WriteLine();
                Console.WriteLine("Första 10 sorterade IP-adresser:");

                foreach (var ip in testData.Take(10))
                {
                    Console.WriteLine(ip);
                }

                Console.WriteLine();
                Console.WriteLine($"Totalt antal rader inlästa: {logs.Count}");
            }
        }
    }
}
