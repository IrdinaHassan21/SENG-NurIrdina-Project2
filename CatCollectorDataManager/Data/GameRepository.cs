using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using CatCollectorDataManager.Models;

namespace CatCollectorDataManager.Data
{
    public class GameRepository : IGameRepository
    {
        private readonly List<Player> _players = new();

        public IEnumerable<Player> GetAll() => _players.OrderByDescending(p => p.BestScore);

        public Player? GetById(Guid id) => _players.FirstOrDefault(p => p.Id == id);

        public Player? GetByName(string name) => _players.FirstOrDefault(p => string.Equals(p.Name, name, StringComparison.OrdinalIgnoreCase));

        public void Add(Player player)
        {
            if (GetByName(player.Name) != null)
                throw new InvalidOperationException("A player with that name already exists.");
            _players.Add(player);
        }

        public bool Update(Player player)
        {
            var existing = GetById(player.Id);
            if (existing == null) return false;
            existing.Name = player.Name;
            existing.GoodCatsCollected = player.GoodCatsCollected;
            existing.BadCatsCollected = player.BadCatsCollected;
            existing.ChonkyCatsCollected = player.ChonkyCatsCollected;
            existing.BestScore = player.BestScore;
            return true;
        }

        public bool Delete(Guid id)
        {
            var existing = GetById(id);
            if (existing == null) return false;
            return _players.Remove(existing);
        }

        public void ReplaceAll(IEnumerable<Player> players)
        {
            _players.Clear();
            _players.AddRange(players);
        }

        public void SaveToFile(string path)
        {
            var opts = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize(_players, opts);
            File.WriteAllText(path, json);
        }

        public void LoadFromFile(string path)
        {
            if (!File.Exists(path)) return;
            var json = File.ReadAllText(path);
            var players = JsonSerializer.Deserialize<List<Player>>(json);
            if (players != null)
            {
                ReplaceAll(players);
            }
        }
    }
}
