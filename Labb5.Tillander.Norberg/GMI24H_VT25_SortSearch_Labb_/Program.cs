using AlgorithmLib;
using System;
using System.Diagnostics;
using System.Linq;

namespace GMI24H_VT25_SortSearch_Labb_
{
    internal class Program
    {
        private const int RepeatCount = 100;

        static void Main(string[] args)
        {
            const int numberOfPosts = 500000;
            const int seed = 123;

            var generator = new RandomLogGenerator();
            var logs = generator.GenerateLogs(numberOfPosts, seed).ToList();

            // Test för sökalgoritmerna innan vi kör på loggdatan. Bra för att verifiera att de fungerar korrekt innan vi mäter prestanda.
            SearchAlgorithmTests.RunBasicSearchTests();

            Console.WriteLine($"Totalt antal rader inlästa: {logs.Count}");

            Console.WriteLine("Första 5 loggposter:");
            foreach (var entry in logs.Take(5))
            {
                Console.WriteLine(entry);
            }

            var ipSearchManager = new SearchingManager<string>();
            var intSearchManager = new SearchingManager<int>();
            var logSearchManager = new SearchingManager<LogEntry>();

            var sortingManager = new SortingManager<LogEntry>();

            var ipAddresses = logs.Select(entry => entry.IpAddress).ToList();
            var statusCodes = logs.Select(entry => entry.StatusCode).ToList();
            var logsByTime = logs.OrderBy(entry => entry.Timestamp).ToList();

            ipAddresses.Sort();
            statusCodes.Sort();

            string targetIp = "192.168.1.10";

            RunSearchCase("IP-adress", "LinearSearch", RepeatCount,
                () => ipSearchManager.LinearSearch(ipAddresses, targetIp));

            RunSearchCase("IP-adress", "BinarySearch", RepeatCount,
                () => ipSearchManager.BinarySearch(ipAddresses, targetIp));

            RunSearchCase("IP-adress", "JumpSearch", RepeatCount,
                () => ipSearchManager.JumpSearch(ipAddresses, targetIp));

            foreach (int statusCode in new[] { 401, 403, 500 })
            {
                RunSearchCase($"Statuskod {statusCode}", "BinarySearch", RepeatCount,
                    () => intSearchManager.BinarySearch(statusCodes, statusCode));
            }

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

            // =========================
            // 📊 SORT TESTS
            // =========================

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

            Console.WriteLine();
            Console.WriteLine("Körningen är klar.");
        }

        // =========================
        // 🔍 SEARCH HELPERS
        // =========================

        private static void RunSearchCase(string caseDescription, string algorithmName, int repeats, Func<int> searchAction)
        {
            var (index, avgMs) = MeasureAverage(repeats, searchAction);
            string result = index >= 0 ? $"träff vid index {index}" : "ingen träff";

            Console.WriteLine($"{caseDescription} med {algorithmName}: {result}, medelvärde över {repeats} upprepningar = {avgMs:F4} ms");
        }

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

        // =========================
        // 📊 SORT HELPERS
        // =========================

        private static void RunSortCase(string name, int repeats, Action action)
        {
            double avgMs = MeasureSortAverage(repeats, action);
            Console.WriteLine($"{name}: {avgMs:F4} ms");
        }

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
