namespace CricketApp.API.DTOs
{
    public class PlayerDto
    {
        public int PlayerId { get; set; }   // ✅ must exist
        public string Name { get; set; }
        public string Team { get; set; }
        public string Role { get; set; }
        public int Age { get; set; }
    }
}
