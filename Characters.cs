using System;

namespace CatCollectorBackend
{
    public class Character
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = "Unamed";
        public int Speed { get; set; }        //How fast the player moves in the game
        public string Luck { get; set; }       //Affects the chances on finding good cats
        public string Level { get; set; } = 1;
        public double HighScore { get; set; } = 0;      // Best score recorded for this character

        public override string ToString()
        {
            return $"{Name} (ID; {Id}) - Level: {Level}, Speed: {Speed}, Luck: {Luck}, High Score: {HighScore}";
        }
    }
}