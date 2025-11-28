using System;
using System.Collections.Generic;
using CatCollectorDataManager.Models;

namespace CatCollectorDataManager.Data
{
    public interface IGameRepository
    {
        IEnumerable<Player> GetAll();
        Player? GetById(Guid id);
        Player? GetByName(string name);
        void Add(Player player);
        bool Update(Player player);
        bool Delete(Guid id);
        void ReplaceAll(IEnumerable<Player> players);
        void SaveToFile(string path);
        void LoadFromFile(string path);
    }
}
