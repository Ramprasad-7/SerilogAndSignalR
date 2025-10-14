namespace CricketApp.API.Models
{
    public class Player
    {
        public int PlayerId { get; set; }
        public string Name { get; set; }
        public string Team { get; set; }
        public string Role { get; set; } // e.g., Batsman, Bowler
        public int Age { get; set; }
    }
}
