namespace ChampionBuildApi.Models
{
    public class ChampionBuild
    {
        public List<string> StartingItems { get; set; }
        public List<string> EarlyItems { get; set; }
        public List<string> FullBuildItems { get; set; }
        public List<string> MainRunes { get; set; }
        public List<string> SecondaryRunes { get; set; }
        public List<string> BranchNames { get; set; }
        public List<string> Spells { get; set; }

        public ChampionBuild()
        {
            StartingItems = new List<string>();
            EarlyItems = new List<string>();
            FullBuildItems = new List<string>();
            MainRunes = new List<string>();
            SecondaryRunes = new List<string>();
            BranchNames = new List<string>();
            Spells = new List<string>();
        }
    }
}
