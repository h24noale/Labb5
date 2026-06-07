using AlgorithmLib;
using System;
using System.Diagnostics;
using System.Linq;

namespace GMI24H_VT25_SortSearch_Labb_
{
    internal class Program
    {
        // Antal gånger varje test körs för att få ett stabilt medelvärde
        private const int RepeatCount = 100;

        static void Main(string[] args)
        {
            // Antal loggposter som genereras
            const int numberOfPosts = 5000;
            const int seed = 123;

            // Genererar reproducerbar testdata (loggar)
            var generator = new RandomLogGenerator();
            var logs = generator.GenerateLogs(numberOfPosts, seed).ToList();

            // Kör enkla tester för att verifiera att sökalgoritmer fungerar
            SearchAlgorithmTests.RunBasicSearchTests();

            Console.WriteLine($"Totalt antal rader inlästa: {logs.Count}");

            // Visar de första 5 loggposterna för kontroll
            Console.WriteLine("Första 5 loggposter:");
            foreach (var entry in logs.Take(5))
            {
                Console.WriteLine(entry);
            }

            // Skapar instanser av sök- och sorteringshanterare
            var ipSearchManager = new SearchingManager<string>();
            var intSearchManager = new SearchingManager<int>();
            var logSearchManager = new SearchingManager<LogEntry>();
            var sortingManager = new SortingManager<LogEntry>();

            // Plockar ut specifika fält för sökning/sortering
            var ipAddresses = logs.Select(entry => entry.IpAddress).ToList();
            var statusCodes = logs.Select(entry => entry.StatusCode).ToList();

            // Sorterar loggar efter tid för tidsbaserade sökningar
            var logsByTime = logs.OrderBy(entry => entry.Timestamp).ToList();

            // Krävs för binärsökning (listor måste vara sorterade)
            ipAddresses.Sort();
            statusCodes.Sort();

            // SEARCH TESTS

            string targetIp = "192.168.1.10";

            // Tester för IP-adress
            RunSearchCase("IP-adress", "LinearSearch", RepeatCount,
                () => ipSearchManager.LinearSearch(ipAddresses, targetIp));

            RunSearchCase("IP-adress", "BinarySearch", RepeatCount,
                () => ipSearchManager.BinarySearch(ipAddresses, targetIp));

            RunSearchCase("IP-adress", "JumpSearch", RepeatCount,
                () => ipSearchManager.JumpSearch(ipAddresses, targetIp));

            // Tester för statuskoder
            foreach (int statusCode in new[] { 401, 403, 500 })
            {
                RunSearchCase($"Statuskod {statusCode}", "BinarySearch", RepeatCount,
                    () => intSearchManager.BinarySearch(statusCodes, statusCode));
            }

            // INTERVALL SÖKNING

            DateTime intervalStart = logsByTime[numberOfPosts / 4].Timestamp;
            DateTime intervalEnd = logsByTime[numberOfPosts / 2].Timestamp;

            Console.WriteLine();
            Console.WriteLine($"Söker loggposter i tidsintervallet {intervalStart:O} - {intervalEnd:O}");

            var intervalStartEntry = new LogEntry { Timestamp = intervalStart };
            var intervalEndEntry = new LogEntry { Timestamp = intervalEnd };

            var intervalSearchResults = new[]
            {
                new
                {
                    Name = "BinarySearch",
                    StartIndex = logSearchManager.BinarySearch(logsByTime, intervalStartEntry),
                    EndIndex = logSearchManager.BinarySearch(logsByTime, intervalEndEntry),
                    Timing = MeasureAverage(RepeatCount,
                        () => logSearchManager.BinarySearch(logsByTime, intervalStartEntry)
                    ).averageMilliseconds
                },
                new
                {
                    Name = "JumpSearch",
                    StartIndex = logSearchManager.JumpSearch(logsByTime, intervalStartEntry),
                    EndIndex = logSearchManager.JumpSearch(logsByTime, intervalEndEntry),
                    Timing = MeasureAverage(RepeatCount,
                        () => logSearchManager.JumpSearch(logsByTime, intervalStartEntry)
                    ).averageMilliseconds
                }
            };

            foreach (var result in intervalSearchResults)
            {
                if (result.StartIndex >= 0 && result.EndIndex >= 0 && result.EndIndex >= result.StartIndex)
                {
                    int count = result.EndIndex - result.StartIndex + 1;
                    Console.WriteLine($"{result.Name}: startIndex={result.StartIndex}, endIndex={result.EndIndex}, träffar={count}, medelms={result.Timing:F4}");
                }
                else
                {
                    Console.WriteLine($"{result.Name}: hittade inte båda gränserna");
                }
            }

            // SORT TESTS

            Console.WriteLine();
            Console.WriteLine("----- SORTERINGSTESTER -----");

            var originalLogs = logs.ToList();

            RunSortCase("BubbleSort (Timestamp)", RepeatCount, () =>
            {
                var copy = originalLogs.ToList();
                sortingManager.BubbleSort(copy);
            });

            RunSortCase("InsertionSort (Timestamp)", RepeatCount, () =>
            {
                var copy = originalLogs.ToList();
                sortingManager.InsertionSort(copy);
            });

            RunSortCase("MergeSort (Timestamp)", RepeatCount, () =>
            {
                var copy = originalLogs.ToList();
                sortingManager.MergeSort(copy);
            });

            RunSortCase("QuickSort (Timestamp)", RepeatCount, () =>
            {
                var copy = originalLogs.ToList();
                sortingManager.QuickSort(copy);
            });
            
            RunSortCase("HeapSort (Timestamp)", RepeatCount, () =>
            {
                var copy = originalLogs.ToList();
                sortingManager.HeapSort(copy);
            });

            RunSortCase("SelectionSort (Timestamp)", RepeatCount, () =>
            {
                var copy = originalLogs.ToList();
                sortingManager.SelectionSort(copy);
            });

            Console.WriteLine();
            Console.WriteLine("Körningen är klar.");
        }

        // SEARCH HELPERS

        // Kör en sökning flera gånger och mäter snittid
        private static void RunSearchCase(string caseDescription, string algorithmName, int repeats, Func<int> searchAction)
        {
            var (index, avgMs) = MeasureAverage(repeats, searchAction);
            string result = index >= 0 ? $"träff vid index {index}" : "ingen träff";

            Console.WriteLine($"{caseDescription} med {algorithmName}: {result}, medelvärde över {repeats} upprepningar = {avgMs:F4} ms");
        }

        // Mäta genomsnittlig exekveringstid för sökning
        private static (int index, double averageMilliseconds) MeasureAverage(int repeats, Func<int> action)
        {
            long totalTicks = 0;
            int lastIndex = -1;
            var sw = new Stopwatch();

            for (int i = 0; i < repeats; i++)
            {
                sw.Restart();
                lastIndex = action();
                sw.Stop();
                totalTicks += sw.ElapsedTicks;
            }

            double averageMs = totalTicks * 1000.0 / repeats / Stopwatch.Frequency;
            return (lastIndex, averageMs);
        }

        // SORT HELPERS

        // Kör sortering flera gånger och mäter snittid
        private static void RunSortCase(string name, int repeats, Action action)
        {
            double avgMs = MeasureSortAverage(repeats, action);
            Console.WriteLine($"{name}: {avgMs:F4} ms");
        }

        // Mäta genomsnittlig exekveringstid för sortering
        private static double MeasureSortAverage(int repeats, Action action)
        {
            long totalTicks = 0;
            var sw = new Stopwatch();

            for (int i = 0; i < repeats; i++)
            {
                sw.Restart();
                action();
                sw.Stop();
                totalTicks += sw.ElapsedTicks;
            }

            return totalTicks * 1000.0 / repeats / Stopwatch.Frequency;
        }
    }
}
