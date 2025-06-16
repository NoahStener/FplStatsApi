namespace FplStatsApi.DTOs
{
    public class GoalkeeperDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int TeamId { get; set; }
        public int CleanSheets { get; set; }
        public int GoalsConceded { get; set; }
        public int Saves { get; set; }
        public int PenaltiesSaved { get; set; }
        public string Photo { get; set; } = string.Empty;
    }
}
