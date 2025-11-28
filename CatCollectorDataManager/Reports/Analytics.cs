using System;
using System.Collections.Generic;
using System.Linq;
using CatCollectorDataManager.Models;

namespace CatCollectorDataManager.Reports
{
    public static class Analytics
    {
        // Total players
        public static int PlayerCount(IEnumerable<Player> players) => players.Count();

        // Average best score
        public static double AverageBestScore(IEnumerable<Player> players)
            => players.Any() ? players.Average(p => p.BestScore) : 0;

        // Total cats collected across all players
        public static int TotalCatsCollected(IEnumerable<Player> players)
            => players.Sum(p => p.TotalCats);

        // Top players by best score (top N)
        public static IEnumerable<Player> TopPlayersByScore(IEnumerable<Player> players, int topN = 5)
            => players.OrderByDescending(p => p.BestScore).Take(topN);

        // Best single player (highest best score)
        public static Player? BestPlayer(IEnumerable<Player> players)
            => players.OrderByDescending(p => p.BestScore).FirstOrDefault();

        // Players summary grouped by ranges of best score (example LINQ grouping)
        public static IEnumerable<(string range, int count)> ScoreRangeDistribution(IEnumerable<Player> players)
        {
            var groups = players
                .GroupBy(p =>
                {
                    if (p.BestScore >= 8000) return "8000+";
                    if (p.BestScore >= 5000) return "5000-7999";
                    if (p.BestScore >= 2000) return "2000-4999";
                    if (p.BestScore >= 1000) return "1000-1999";
                    return "0-999";
                })
                .Select(g => (range: g.Key, count: g.Count()))
                .OrderByDescending(g => g.range);
            return groups;
        }

        // Example LINQ: find players who have more bad cats than good cats
        public static IEnumerable<Player> PlayersWithMoreBadThanGood(IEnumerable<Player> players)
            => players.Where(p => p.BadCatsCollected > p.GoodCatsCollected);

        // Example LINQ: players with at least one chonky cat
        public static IEnumerable<Player> PlayersWithChonky(IEnumerable<Player> players)
            => players.Where(p => p.ChonkyCatsCollected > 0);
    }
}
