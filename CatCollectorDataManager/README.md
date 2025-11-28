# Cat Collector Data Manager

## Project Overview

The **Cat Collector Data Manager** is a C# console application that serves as a backend system for managing and analyzing player data for a video game. It handles player statistics, including:

- Player Name
- Good Cats Collected
- Bad Cats Collected
- Chonky Cats Collected
- Best Score

The system supports full **CRUD (Create, Read, Update, Delete) operations**, random data generation for testing, and summary reports using LINQ queries.

This project demonstrates:

- Console application development in C#
- Data management using Lists and classes
- CRUD operations
- Randomized data generation for testing
- LINQ queries for analysis

---

## Project Structure

CatCollectorDataManager/  
│  
├── Models/ # Classes representing game entities (Player.cs)  
├── Data/ # Data repository and interfaces (GameRepository.cs, IGameRepository.cs)  
├── Services/ # Helper services (RandomDataGenerator.cs)  
├── Reports/ # Analytics and summary reports (Analytics.cs)  
├── Program.cs # Main console application entry point  
└── CatCollectorDataManager.sln  
  
---

## Features

- Add a new player
- View all players and their statistics
- Update player stats (good/bad/chonky cats and best score)
- Delete a player
- Generate random test data
- View summary statistics (total cats collected, best scores, etc.)

---

## How to Run

### Prerequisites

- [.NET SDK 9.0 or 8.0](https://dotnet.microsoft.com/en-us/download/dotnet) installed on your system
- Windows, macOS, or Linux with terminal/PowerShell access

---

### Steps

1. **Clone the repository**

git clone https://github.com/IrdinaHassan21/SENG-NurIrdinaProject2.git  
cd SENG-NurIrdinaProject2/CatCollectorDataManager

2. Restore dependencies
   
dotnet restore

3. Build the project
   
dotnet build

4. Run the console application
   
dotnet run

5. Follow the on-screen menu
   
- Select options by typing the corresponding number
- Add players, view analytics, or generate random data

---

Notes
- Data is stored in local JSON files (or in-memory for testing)
- Random data generation helps demonstrate analytics features without manual input
- Best score is automatically updated if a player beats their previous score

Future Integration
- This backend is designed to be connected to a game frontend in the future:
- Game can send player stats directly to the backend
- Backend will automatically update Good/Bad/Chonky Cats and Best Score
- No manual input required from the player
