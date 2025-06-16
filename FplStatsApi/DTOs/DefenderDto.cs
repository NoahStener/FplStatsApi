namespace FplStatsApi.DTOs
{
    public class DefenderDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int TeamId { get; set; }
        public int CleanSheets { get; set; }
        public int GoalsConceded { get; set; }
        public int Goals { get; set; }
        public int Assists { get; set; }
        public int YellowCards { get; set; }
        public int RedCards { get; set; }
        public int OwnGoals { get; set; }
        public string Photo { get; set; } = string.Empty;
    }
}
