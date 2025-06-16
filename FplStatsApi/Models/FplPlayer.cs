namespace FplStatsApi.Models
{
    public class FplPlayer
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public int TeamId { get; set; }
        public int PositionId { get; set; }
        public int Goals { get; set; }
        public int Assists { get; set; }
        public int ScoredGoals { get; set; }
        public int TotalPoints { get; set; }
    }

    public enum FplPosition
    {
        GoalKeeper = 1,
        Defender = 2,
        Midfielder = 3,
        Forward = 4
    }
}
