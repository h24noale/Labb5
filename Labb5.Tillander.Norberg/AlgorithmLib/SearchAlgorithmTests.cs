using System;
using System.Collections.Generic;

namespace AlgorithmLib
{
    /// <summary>
    /// Enkel testklass för att kontrollera att sökalgoritmerna fungerar som de ska.
    /// </summary>
    public static class SearchAlgorithmTests
    {
        public static void RunBasicSearchTests()
        {
            Console.WriteLine("--- SearchAlgorithmTests: Startar enkla testfall ---");

            var intSearcher = new SearchingManager<int>();
            var stringSearcher = new SearchingManager<string>();

            var numbers = new List<int> { 1, 3, 5, 7, 9 };
            var sortedNumbers = new List<int>(numbers);
            sortedNumbers.Sort();

            var words = new List<string> { "alpha", "beta", "gamma", "omega" };
            var sortedWords = new List<string>(words);
            sortedWords.Sort();

            RunTest("LinearSearch find existing int", () => intSearcher.LinearSearch(numbers, 7), 3);
            RunTest("LinearSearch missing int", () => intSearcher.LinearSearch(numbers, 2), -1);

            RunTest("BinarySearch find existing int", () => intSearcher.BinarySearch(sortedNumbers, 5), 2);
            RunTest("BinarySearch missing int", () => intSearcher.BinarySearch(sortedNumbers, 4), -1);

            RunTest("JumpSearch find existing int", () => intSearcher.JumpSearch(sortedNumbers, 3), 1);
            RunTest("JumpSearch missing int", () => intSearcher.JumpSearch(sortedNumbers, 6), -1);

            RunTest("LinearSearch find existing string", () => stringSearcher.LinearSearch(words, "gamma"), 2);
            RunTest("BinarySearch find existing string", () => stringSearcher.BinarySearch(sortedWords, "gamma"), sortedWords.IndexOf("gamma"));
            RunTest("JumpSearch find existing string", () => stringSearcher.JumpSearch(sortedWords, "beta"), sortedWords.IndexOf("beta"));

            Console.WriteLine("--- SearchAlgorithmTests: Klart ---");
        }

        private static void RunTest(string testName, Func<int> action, int expected)
        {
            int result;
            string outcome;

            try
            {
                result = action();
                outcome = result == expected ? "PASS" : "FAIL";
            }
            catch (Exception ex)
            {
                result = int.MinValue;
                outcome = $"ERROR ({ex.GetType().Name}: {ex.Message})";
            }

            Console.WriteLine($"{testName}: förväntat={expected}, fick={result} => {outcome}");
        }
    }
}
