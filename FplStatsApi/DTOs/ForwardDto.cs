namespace FplStatsApi.DTOs
{
    public class ForwardDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int TeamId { get; set; }
        public int Goals { get; set; }
        public int Assists { get; set; }
        public string ExpectedGoals { get; set; }
        public string ExpectedAssists { get; set; }
        public int Starts { get; set; }
        public int YellowCards { get; set; }
        public int RedCards { get; set; }
        public string Photo { get; set; } = string.Empty;
    }
}
