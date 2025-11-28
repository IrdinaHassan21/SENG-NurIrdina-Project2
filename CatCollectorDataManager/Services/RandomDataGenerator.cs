using System;
using System.Collections.Generic;
using CatCollectorDataManager.Models;

namespace CatCollectorDataManager.Services
{
    public static class RandomDataGenerator
    {
        private static readonly string[] SampleNames = new[]
        {
            "Mr. Goose", "Ms. Whiskers", "Captain Fluff", "Lady Purrington", "Sir Meowsalot",
            "Duchess Furrball", "Baron von Cuddles", "Countess Snugglepaws", "Lord Whiskerface", "Princess Paws",
            "Admiral Fuzzybottom", "General Purr", "Queen Meowths", "Duke Clawdia", "Emperor Furrypants"
            
        };

        public static IEnumerable<Player> GeneratePlayers(int count, int maxCats = 200, int maxScore = 10000)
        {
            var rand = new Random();
            var usedNames = new HashSet<string>();

            for (int i = 0; i < count; i++)
            {
                string name;
                // ensure unique-ish names by append index when needed
                var baseName = SampleNames[rand.Next(SampleNames.Length)];
                name = usedNames.Add(baseName) ? baseName : $"{baseName}{i}";

                var good = rand.Next(0, maxCats + 1);
                var bad = rand.Next(0, Math.Max(1, maxCats - good + 1));
                var chonky = rand.Next(0, Math.Max(1, maxCats - good - bad + 1));
                var bestScore = rand.Next(0, maxScore + 1);

                yield return new Player
                {
                    Name = name,
                    GoodCatsCollected = good,
                    BadCatsCollected = bad,
                    ChonkyCatsCollected = chonky,
                    BestScore = bestScore
                };
            }
        }
    }
}
