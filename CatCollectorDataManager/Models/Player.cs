using System;
using System.Text.Json.Serialization;

namespace CatCollectorDataManager.Models
{
    public class Player
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public int GoodCatsCollected { get; set; }
        public int BadCatsCollected { get; set; }
        public int ChonkyCatsCollected { get; set; }
        public int BestScore { get; set; }

        [JsonIgnore]
        public int TotalCats => GoodCatsCollected + BadCatsCollected + ChonkyCatsCollected;


        public override string ToString()
        {
            return $"{Name} (ID: {Id}) — BestScore: {BestScore}, Good: {GoodCatsCollected}, Bad: {BadCatsCollected}, Chonky: {ChonkyCatsCollected}, Total: {TotalCats}";
        }
    }
}