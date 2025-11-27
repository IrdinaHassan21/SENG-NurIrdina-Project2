using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace CatCollectorBackend
{
    /// <summary>
    /// Manages characters: CRUD, generation, reports, and persistence.
    /// </summary>
    public class CharacterManager
    {
        private List<Character> characters = new List<Character>();

        public void ListCharacters()
        {
            Console.WriteLine();
            if (!characters.Any())
            {
                Console.WriteLine("No characters available.");
                return;
            }

            Console.WriteLine("All Characters:");
            foreach (var c in characters)
            {
                Console.WriteLine(c.ToString());
            }
        }

        public void CreateCharacterInteractive()
        {
            Console.WriteLine();
            Console.Write("Enter character name: ");
            var name = Console.ReadLine();
            int speed = ReadInt("Speed (1-10): ", 1, 10);
            int luck = ReadInt("Luck (0-100): ", 0, 100);
            int level = ReadInt("Level (>=1): ", 1, 100);
            int highScore = ReadInt("HighScore (>=0): ", 0, int.MaxValue);

            var character = new Character
            {
                Name = string.IsNullOrWhiteSpace(name) ? "Unnamed" : name,
                Speed = speed,
                Luck = luck,
                Level = level,
                HighScore = highScore
            };

            characters.Add(character);
            Console.WriteLine($"Created: {character}");
        }

        public void ViewCharacterInteractive()
        {
            Console.WriteLine();
            var character = SelectCharacter();
            if (character == null) return;

            Console.WriteLine("Character Details:");
            Console.WriteLine($"ID: {character.Id}");
            Console.WriteLine($"Name: {character.Name}");
            Console.WriteLine($"Speed: {character.Speed}");
            Console.WriteLine($"Luck: {character.Luck}");
            Console.WriteLine($"Level: {character.Level}");
            Console.WriteLine($"HighScore: {character.HighScore}");
        }

        public void UpdateCharacterInteractive()
        {
            Console.WriteLine();
            var character = SelectCharacter();
            if (character == null) return;

            Console.WriteLine("Leave input blank to keep current value.");

            Console.Write($"Name ({character.Name}): ");
            var name = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(name)) character.Name = name;

            var speed = ReadIntNullable($"Speed ({character.Speed}): ", 1, 10);
            if (speed.HasValue) character.Speed = speed.Value;

            var luck = ReadIntNullable($"Luck ({character.Luck}): ", 0, 100);
            if (luck.HasValue) character.Luck = luck.Value;

            var level = ReadIntNullable($"Level ({character.Level}): ", 1, 100);
            if (level.HasValue) character.Level = level.Value;

            var hs = ReadIntNullable($"HighScore ({character.HighScore}): ", 0, int.MaxValue);
            if (hs.HasValue) character.HighScore = hs.Value;

            Console.WriteLine("Updated character:");
            Console.WriteLine(character.ToString());
        }

        public void DeleteCharacterInteractive()
        {
            Console.WriteLine();
            var character = SelectCharacter();
            if (character == null) return;

            Console.Write($"Are you sure you want to delete '{character.Name}'? (y/n): ");
            var ans = Console.ReadLine();
            if (ans?.Trim().ToLower() == "y")
            {
                characters.Remove(character);
                Console.WriteLine("Character deleted.");
            }
            else
            {
                Console.WriteLine("Delete cancelled.");
            }
        }

        public void GenerateRandomCharactersInteractive()
        {
            Console.WriteLine();
            int count = ReadInt("How many random characters to generate? ", 1, 1000);
            var rnd = new Random();
            var generator = new RandomDataGenerator();
            var generated = generator.GenerateCharacters(count);
            characters.AddRange(generated);
            Console.WriteLine($"Generated {count} random characters.");
        }

        public void RunReportsInteractive()
        {
            Console.WriteLine();
            if (!characters.Any())
            {
                Console.WriteLine("No data to analyze. Generate or add some characters first.");
                return;
            }

            Console.WriteLine("=== Reports Menu ===");
            Console.WriteLine("1. Average Speed and Luck");
            Console.WriteLine("2. Top 5 by HighScore");
            Console.WriteLine("3. Count by Level");
            Console.WriteLine("4. Characters with Luck >= threshold");
            Console.WriteLine("5. Summary (min/max/avg)");
            Console.Write("Choose report: ");
            var ch = Console.ReadLine();

            switch (ch)
            {
                case "1":
                    var avgSpeed = characters.Average(c => c.Speed);
                    var avgLuck = characters.Average(c => c.Luck);
                    Console.WriteLine($"Average Speed: {avgSpeed:F2}");
                    Console.WriteLine($"Average Luck: {avgLuck:F2}");
                    break;

                case "2":
                    var top = characters.OrderByDescending(c => c.HighScore).Take(5);
                    Console.WriteLine("Top 5 by HighScore:");
                    foreach (var t in top) Console.WriteLine(t);
                    break;

                case "3":
                    var byLevel = characters.GroupBy(c => c.Level)
                                             .OrderBy(g => g.Key)
                                             .Select(g => new { Level = g.Key, Count = g.Count() });
                    Console.WriteLine("Count by Level:");
                    foreach (var g in byLevel) Console.WriteLine($"Level {g.Level}: {g.Count} characters");
                    break;

                case "4":
                    int threshold = ReadInt("Enter luck threshold (0-100): ", 0, 100);
                    var filtered = characters.Where(c => c.Luck >= threshold).OrderByDescending(c => c.Luck);
                    Console.WriteLine($"Characters with Luck >= {threshold}:");
                    foreach (var f in filtered) Console.WriteLine(f);
                    break;

                case "5":
                    Console.WriteLine($"Count: {characters.Count}");
                    Console.WriteLine($"Min Speed: {characters.Min(c => c.Speed)}");
                    Console.WriteLine($"Max Speed: {characters.Max(c => c.Speed)}");
                    Console.WriteLine($"Min Luck: {characters.Min(c => c.Luck)}");
                    Console.WriteLine($"Max Luck: {characters.Max(c => c.Luck)}");
                    Console.WriteLine($"Avg HighScore: {characters.Average(c => c.HighScore):F2}");
                    break;

                default:
                    Console.WriteLine("Invalid report choice.");
                    break;
            }
        }

        // Persistence
        public void SaveToFile(string path)
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                var json = JsonSerializer.Serialize(characters, options);
                File.WriteAllText(path, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving file: {ex.Message}");
            }
        }

        public void LoadFromFile(string path)
        {
            try
            {
                if (!File.Exists(path))
                {
                    // No file to load; leave list as-is.
                    return;
                }
                var json = File.ReadAllText(path);
                var loaded = JsonSerializer.Deserialize<List<Character>>(json);
                if (loaded != null)
                {
                    characters = loaded;
                    Console.WriteLine($"Loaded {characters.Count} characters from file.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading file: {ex.Message}");
            }
        }

        // Helper & UI helpers
        private Character? SelectCharacter()
        {
            if (!characters.Any())
            {
                Console.WriteLine("No characters available.");
                return null;
            }

            Console.WriteLine("Select character by index:");
            for (int i = 0; i < characters.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {characters[i].Name} (Level {characters[i].Level}, HighScore {characters[i].HighScore})");
            }
            int idx = ReadInt("Index: ", 1, characters.Count);
            return characters[idx - 1];
        }

        private int ReadInt(string prompt, int min, int max)
        {
            int value;
            while (true)
            {
                Console.Write(prompt);
                var input = Console.ReadLine();
                if (int.TryParse(input, out value) && value >= min && value <= max) return value;
                Console.WriteLine($"Please enter an integer between {min} and {max}.");
            }
        }

        private int? ReadIntNullable(string prompt, int min, int max)
        {
            while (true)
            {
                Console.Write(prompt);
                var input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input)) return null;
                if (int.TryParse(input, out int v) && v >= min && v <= max) return v;
                Console.WriteLine($"Please enter an integer between {min} and {max}, or leave blank.");
            }
        }
    }
}
