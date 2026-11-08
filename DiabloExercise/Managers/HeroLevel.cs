public class HeroLevel : ILevel
{
    private const int MAX_LEVEL = 50;
    public int Level { get; private set; }
    public int MaxLevel => MAX_LEVEL;

    public HeroLevel()
    {
        Level = 1;
    }
    public void IncreaseLevel()
    {
        if (Level < MAX_LEVEL)
        {
            Level += 1;
        }
    }

    public override string ToString()
    {
        return $"Hero is at {Level} level!";
    }
}

