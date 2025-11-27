using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.Json;

namespace CatCollectorBackend
{
    class Program
    {
        static void Main(string[] args)
        {
            var manager = new CharacterManager();
            manager.LoadFromFile("characters.json");
            manager.LoadFromFile("characters.json");
            Console.Title = "Cat Collector - Character Management Backend";
        
            bool exit = false;
            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("=== Cat Collector: Character Management ===");
                Console.WriteLine("1. List all characters");
                Console.WriteLine("2. Create a character");
                Console.WriteLine("3. View character details");
                Console.WriteLine("4. Update a character");
                Console.WriteLine("5. Delete a character");
                Console.WriteLine("6. Generate random sample characters");
                Console.WriteLine("7. Data analysis & reports (LINQ)");
                Console.WriteLine("8. Save characters to file (characters.json)");
                Console.WriteLine("9. Load characters from file (characters.json)");
                Console.WriteLine("0. Exit");
                Console.Write("Choose an option: ");

                var cjoice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                    manager.ListCharacters();
                    Pause();
                    break;

                    case "2":
                    manager.CreateCharacterInteractive();
                    Pause();
                    break;

                    case "3":
                    manager.ViewCharacterInteractive();
                    Pause();
                    break;

                    case "4":
                    manager.UpdateCharacterInteractive();
                    Pause();   
                    break;

                    case "5":
                    manager.DeleteCharacterInteractive();
                    Pause();
                    break;

                    case "6":
                    manager.GenerateRandomCharactersInteractive();
                    Pause();
                    break;

                    case "7":
                    manager.RunReportsInteractive();
                    Pause();
                    break;

                    case "8":
                        manager.SaveToFile("characters.json");
                        Pause("Saved to characters.json. Press Enter to continue...");
                        break;
                    case "9":
                        manager.LoadFromFile("characters.json");
                        Pause("Loaded from characters.json (if it existed). Press Enter to continue...");
                        break;
                    case "0":
                        manager.SaveToFile("characters.json");
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Invalid choice.");
                        Pause();
                        break;

                }
            }

            Console.WriteLine("Goodbye! CHARACTER BACKEND SAVED TO CHARACTERS.JSON");
        }
        static void Pause(string message = "Press Enter to continue...")
        {
            Console.WriteLine();
            Console.WriteLine(message);
            Console.ReadLine();
        }
    }
}