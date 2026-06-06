using AlgorithmLib;
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
            //Test för sökalgoritmerna innan vi kör på loggdatan. Bra för att verifiera att de fungerar korrekt innan vi mäter prestanda.
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

            var ipAddresses = logs.Select(entry => entry.IpAddress).ToList();
            var statusCodes = logs.Select(entry => entry.StatusCode).ToList();
            var logsByTime = logs.OrderBy(entry => entry.Timestamp).ToList();

            ipAddresses.Sort();
            statusCodes.Sort();

            string targetIp = "192.168.1.10";
            RunSearchCase("IP-adress", "LinearSearch", RepeatCount, () => ipSearchManager.LinearSearch(ipAddresses, targetIp));
            RunSearchCase("IP-adress", "BinarySearch", RepeatCount, () => ipSearchManager.BinarySearch(ipAddresses, targetIp));
            RunSearchCase("IP-adress", "JumpSearch", RepeatCount, () => ipSearchManager.JumpSearch(ipAddresses, targetIp));

            foreach (int statusCode in new[] { 401, 403, 500 })
            {
                RunSearchCase($"Statuskod {statusCode}", "BinarySearch", RepeatCount, () => intSearchManager.BinarySearch(statusCodes, statusCode));
            }

            DateTime intervalStart = logsByTime[numberOfPosts / 4].Timestamp;
            DateTime intervalEnd = logsByTime[numberOfPosts / 2].Timestamp;

            Console.WriteLine();
            Console.WriteLine($"Söker loggposter i tidsintervallet {intervalStart:O} - {intervalEnd:O}");

            var intervalStartEntry = new LogEntry { Timestamp = intervalStart };
            var intervalEndEntry = new LogEntry { Timestamp = intervalEnd };

            var intervalSearchResults = new[]
            {
                new { Name = "BinarySearch", StartIndex = logSearchManager.BinarySearch(logsByTime, intervalStartEntry), EndIndex = logSearchManager.BinarySearch(logsByTime, intervalEndEntry), Timing = MeasureAverage(RepeatCount, () => logSearchManager.BinarySearch(logsByTime, intervalStartEntry)) },
                new { Name = "JumpSearch", StartIndex = logSearchManager.JumpSearch(logsByTime, intervalStartEntry), EndIndex = logSearchManager.JumpSearch(logsByTime, intervalEndEntry), Timing = MeasureAverage(RepeatCount, () => logSearchManager.JumpSearch(logsByTime, intervalStartEntry)) }
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
                    Console.WriteLine($"{result.Name}: hittade inte båda intervallets gränser (startIndex={result.StartIndex}, endIndex={result.EndIndex})");
                }
            }

            Console.WriteLine();
            Console.WriteLine("Körningen är klar. Använd resultaten för att jämföra sökalgoritmernas prestanda.");
        }

        private static void RunSearchCase(string caseDescription, string algorithmName, int repeats, Func<int> searchAction)
        {
            var (index, averageMs) = MeasureAverage(repeats, searchAction);
            string foundText = index >= 0 ? $"träff vid index {index}" : "ingen träff";
            Console.WriteLine($"{caseDescription} med {algorithmName}: {foundText}, medelvärde över {repeats} upprepningar = {averageMs:F4} ms");
        }

        private static (int index, double averageMilliseconds) MeasureAverage(int repeats, Func<int> action)
        {
            if (repeats <= 0) throw new ArgumentOutOfRangeException(nameof(repeats));

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
    }
}
