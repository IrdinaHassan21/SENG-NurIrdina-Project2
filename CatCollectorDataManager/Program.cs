using System;
using System.Linq;
using CatCollectorDataManager.Data;
using CatCollectorDataManager.Models;
using CatCollectorDataManager.Reports;
using CatCollectorDataManager.Services;

namespace CatCollectorDataManager
{
    class Program
    {
        private static readonly IGameRepository Repo = new GameRepository();
        private const string DefaultDataFile = "players.json";

        static void Main(string[] args)
        {
            // Try load existing data
            Repo.LoadFromFile(DefaultDataFile);

            bool running = true;
            while (running)
            {
                Console.Clear();
                Console.WriteLine("=== Video Game Data Management System ===");
                Console.WriteLine("1) View all players");
                Console.WriteLine("2) View player details");
                Console.WriteLine("3) Add new player");
                Console.WriteLine("4) Update player");
                Console.WriteLine("5) Delete player");
                Console.WriteLine("6) Generate random sample data");
                Console.WriteLine("7) Analytics & Reports");
                Console.WriteLine("8) Save data");
                Console.WriteLine("9) Load data");
                Console.WriteLine("0) Exit");
                Console.Write("Choose option: ");
                var choice = Console.ReadLine();

                try
                {
                    switch (choice)
                    {
                        case "1":
                            ViewAll();
                            break;
                        case "2":
                            ViewDetails();
                            break;
                        case "3":
                            AddNew();
                            break;
                        case "4":
                            Update();
                            break;
                        case "5":
                            Delete();
                            break;
                        case "6":
                            GenerateSample();
                            break;
                        case "7":
                            AnalyticsMenu();
                            break;
                        case "8":
                            SaveData();
                            break;
                        case "9":
                            LoadData();
                            break;
                        case "0":
                            running = false;
                            break;
                        default:
                            Console.WriteLine("Unknown option.");
                            Pause();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"ERROR: {ex.Message}");
                    Pause();
                }
            }

            // Auto-save on exit
            Repo.SaveToFile(DefaultDataFile);
            Console.WriteLine("Saved data. Goodbye!");
        }

        static void ViewAll()
        {
            var all = Repo.GetAll().ToList();
            Console.WriteLine($"\nPlayers ({all.Count}):");
            foreach (var p in all)
            {
                Console.WriteLine(p.ToString());
            }
            Pause();
        }

        static void ViewDetails()
        {
            var p = FindPlayerPrompt();
            if (p == null) return;
            Console.WriteLine("\nDETAILS:");
            Console.WriteLine($"ID: {p.Id}");
            Console.WriteLine($"Name: {p.Name}");
            Console.WriteLine($"Good Cats: {p.GoodCatsCollected}");
            Console.WriteLine($"Bad Cats: {p.BadCatsCollected}");
            Console.WriteLine($"Chonky Cats: {p.ChonkyCatsCollected}");
            Console.WriteLine($"Best Score: {p.BestScore}");
            Console.WriteLine($"Total Cats: {p.TotalCats}");
            Pause();
        }

        static void AddNew()
        {
            Console.WriteLine("\nAdd New Player");
            Console.Write("Name: ");
            var name = Console.ReadLine() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Name cannot be empty.");
                Pause();
                return;
            }

            var player = new Player { Name = name.Trim() };
            player.GoodCatsCollected = PromptInt("Good cats collected", 0);
            player.BadCatsCollected = PromptInt("Bad cats collected", 0);
            player.ChonkyCatsCollected = PromptInt("Chonky cats collected", 0);
            player.BestScore = PromptInt("Best score", 0);

            Repo.Add(player);
            Console.WriteLine("Player added.");
            Pause();
        }

        static void Update()
        {
            var p = FindPlayerPrompt();
            if (p == null) return;
            Console.WriteLine($"\nUpdating player: {p.Name} ({p.Id})");
            Console.Write("New name (leave blank to keep): ");
            var newName = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newName)) p.Name = newName.Trim();
            p.GoodCatsCollected = PromptInt($"Good cats collected ({p.GoodCatsCollected})", p.GoodCatsCollected);
            p.BadCatsCollected = PromptInt($"Bad cats collected ({p.BadCatsCollected})", p.BadCatsCollected);
            p.ChonkyCatsCollected = PromptInt($"Chonky cats collected ({p.ChonkyCatsCollected})", p.ChonkyCatsCollected);
            p.BestScore = PromptInt($"Best score ({p.BestScore})", p.BestScore);

            var ok = Repo.Update(p);
            Console.WriteLine(ok ? "Updated." : "Failed to update.");
            Pause();
        }

        static void Delete()
        {
            var p = FindPlayerPrompt();
            if (p == null) return;
            Console.Write($"Are you sure you want to delete {p.Name}? (y/n): ");
            var c = Console.ReadLine();
            if (c?.ToLower() == "y")
            {
                var ok = Repo.Delete(p.Id);
                Console.WriteLine(ok ? "Deleted." : "Delete failed.");
            }
            else Console.WriteLine("Aborted.");
            Pause();
        }

        static void GenerateSample()
        {
            Console.WriteLine("\nGenerate sample players");
            var n = PromptInt("How many sample players to generate", 10);
            var sample = RandomDataGenerator.GeneratePlayers(n).ToList();
            Repo.ReplaceAll(sample);
            Console.WriteLine($"Generated {n} sample players and replaced current dataset.");
            Pause();
        }

        static void AnalyticsMenu()
        {
            var players = Repo.GetAll().ToList();
            Console.WriteLine("\n=== Analytics & Reports ===");
            Console.WriteLine($"Total players: {Analytics.PlayerCount(players)}");
            Console.WriteLine($"Average best score: {Analytics.AverageBestScore(players):F2}");
            Console.WriteLine($"Total cats collected (all players): {Analytics.TotalCatsCollected(players)}");
            Console.WriteLine();

            Console.WriteLine("Top players by score:");
            foreach (var p in Analytics.TopPlayersByScore(players, 5))
            {
                Console.WriteLine($" - {p.Name} : {p.BestScore}");
            }

            Console.WriteLine();
            var best = Analytics.BestPlayer(players);
            if (best != null)
            {
                Console.WriteLine($"Best player overall: {best.Name} (score {best.BestScore})");
            }

            Console.WriteLine("\nScore range distribution:");
            foreach (var group in Analytics.ScoreRangeDistribution(players))
            {
                Console.WriteLine($" {group.range} => {group.count}");
            }

            Console.WriteLine("\nPlayers with more bad cats than good cats:");
            foreach (var p in Analytics.PlayersWithMoreBadThanGood(players))
            {
                Console.WriteLine($" - {p.Name} (bad {p.BadCatsCollected} > good {p.GoodCatsCollected})");
            }

            Pause();
        }

        static void SaveData()
        {
            Console.Write("Enter filename to save (leave blank for default players.json): ");
            var path = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(path)) path = DefaultDataFile;
            Repo.SaveToFile(path);
            Console.WriteLine($"Saved to {path}");
            Pause();
        }

        static void LoadData()
        {
            Console.Write("Enter filename to load (leave blank for default players.json): ");
            var path = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(path)) path = DefaultDataFile;
            Repo.LoadFromFile(path);
            Console.WriteLine($"Loaded from {path} (if file existed).");
            Pause();
        }

        // helpers
        static Player? FindPlayerPrompt()
        {
            Console.Write("Search by (1) Name or (2) ID?: ");
            var mode = Console.ReadLine();
            if (mode == "2")
            {
                Console.Write("Enter ID: ");
                if (Guid.TryParse(Console.ReadLine(), out var id))
                {
                    var p = Repo.GetById(id);
                    if (p == null) Console.WriteLine("No player with that ID.");
                    return p;
                }
                else
                {
                    Console.WriteLine("Invalid GUID.");
                    Pause();
                    return null;
                }
            }
            else
            {
                Console.Write("Enter name: ");
                var name = Console.ReadLine() ?? string.Empty;
                var p = Repo.GetByName(name);
                if (p == null) Console.WriteLine("No player with that name.");
                return p;
            }
        }

        static int PromptInt(string prompt, int defaultValue)
        {
            Console.Write($"{prompt} [{defaultValue}]: ");
            var s = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(s)) return defaultValue;
            return int.TryParse(s, out var v) ? v : defaultValue;
        }

        static void Pause()
        {
            Console.WriteLine("\nPress Enter to continue...");
            Console.ReadLine();
        }
    }
}
