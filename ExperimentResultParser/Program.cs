using Experiments;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ExperimentResultParser
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Starting");
            string json = File.ReadAllText(@"C:\Users\d.crha\Documents\Personal\colonizers\Experiments\Results\ISMCTSVsHeuristic.json");
            List<ExperimentResult> results = JsonConvert.DeserializeObject<List<ExperimentResult>>(json,
                new JsonSerializerSettings { Error = (sender, args) => { args.ErrorContext.Handled = true; } });

            var groupingByPosition = results.SelectMany(x => x.Players).GroupBy(y => y.PlayerEndInfo.Player.ID);

            var rankByPosition = groupingByPosition.Select(x => (x.Key, x.Average(y => y.PlayerEndInfo.Ranking)));
            var rankStdDev = groupingByPosition.Select(x =>
                (x.Key, x.Select(y => y.PlayerEndInfo.Ranking).SampleStandardDeviation()));
            var winsByPosition = groupingByPosition.Select(x => (x.Key, x.Count(y => y.PlayerEndInfo.Ranking == 1)));
            var lossesByPosition = groupingByPosition.Select(x => (x.Key, x.Count(y => y.PlayerEndInfo.Ranking == 4)));

            var groupingByAI = results.SelectMany(x => x.Players).GroupBy(y => y.Name);

            var rankByAI = groupingByAI.Select(x => (x.Key, x.Average(y => y.PlayerEndInfo.Ranking)));
            var winsByAI = groupingByAI.Select(x => (x.Key, x.Count(y => y.PlayerEndInfo.Ranking == 1)));
            var lossesByAI = groupingByAI.Select(x => (x.Key, x.Count(y => y.PlayerEndInfo.Ranking == 4)));

            //var ismctsNotWins = groupingByAI.First(x => x.Key == "ISMCTS").Where(x => x.PlayerEndInfo.Ranking != 1)
            //    .Select(x => x.PlayerEndInfo.Player.ID);

            var posAiWins = groupingByAI.Select(x => (x.Key, x.Where(y => y.PlayerEndInfo.Ranking == 1)
                .GroupBy(y => y.PlayerEndInfo.Player.ID).Select(y => (y.Key, y.Count()))));

            //var confidenceInterval = ConfidenceInterval(rankByPosition.First(x => x.Key == 4).Item2,
            //    rankStdDev.First(x => x.Key == 4).Item2, 1000, 2.576);
        }

        // Default zValue is for 95% confidence interval
        private static (double, double) ConfidenceInterval(double sampleMean, double sampleStdDev,
            double sampleSize, double zValue = 1.96)
        {
            double range = zValue * sampleStdDev / Math.Sqrt(sampleSize);
            return (sampleMean - range, sampleMean + range);
        }

        private static double SampleStandardDeviation(this IEnumerable<int> values)
        {
            double standardDeviation = 0;

            if (values.Any())
            {
                // Compute the average.     
                double avg = values.Average();

                // Perform the Sum of (value-avg)_2_2.      
                double sum = values.Sum(d => Math.Pow(d - avg, 2));

                // Put it all together.      
                standardDeviation = Math.Sqrt((sum) / (values.Count() - 1));
            }

            return standardDeviation;
        }
    }
}
