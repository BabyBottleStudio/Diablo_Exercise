public interface IIdentifiable
{
    //public bool IsIdentified { get; }

    //public StatsModifier? StatsModifier { get; }
    public StatsModifier? GetActiveStatsModifier();
    public void Identify();
}



