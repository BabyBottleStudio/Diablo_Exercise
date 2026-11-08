using System.Text;

public class ExperienceManager : IExperienceManager
{
    private readonly ILevel heroLevel;
    private readonly HeroLevelingCalculator heroLvlCalc;
    //private readonly DerivedCharacterAttributesCalculator resourcesCalculator;

    private long _experience;
    public long Experience => _experience;
    public int Level => heroLevel.Level;
    //public int MaxLevel => heroLevel.MaxLevel;

    //private double _nextLevelAt;
    //public double NextLevelAt => _nextLevelAt;

    // event for leveling up


    public ExperienceManager(ILevel heroLevel, HeroLevelingCalculator heroLevelingCalculator)
    {
        this.heroLevel = heroLevel;
        heroLvlCalc = heroLevelingCalculator;
        _experience = 0;

    }

    public void Increase(long xpPointsToAdd)
    {
        if (heroLevel.Level == heroLevel.MaxLevel) return;

        if (xpPointsToAdd < 0) return;

        _experience += xpPointsToAdd;

        while (heroLevel.Level < heroLevel.MaxLevel && _experience >= heroLvlCalc.GetExpirienceThreshold(heroLevel.Level + 1))
        {
            LevelUp();
        }

        HeroEvents.InvokeOnExpirienceIncreased(Experience);
    }


    private void LevelUp()
    {
        if (heroLevel.Level == heroLevel.MaxLevel)
        {
            Console.WriteLine("Hero is already on the maximum level.");
            return;
        }

        heroLevel.IncreaseLevel();

        // printing message
        string nextLevelXP = "Hero reached the maximum level!";

        if (heroLevel.Level < heroLevel.MaxLevel)
        {
            nextLevelXP = $"{heroLvlCalc.GetExpirienceThreshold(heroLevel.Level + 1)}";
        }
        Console.WriteLine($"Level:{heroLevel.Level}; XP:{_experience}; XPtNextLevel: {nextLevelXP}");


        HeroEvents.InvokeOnLevelUp();
    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();

        sb.AppendLine($"Level: {heroLevel.Level}");
        sb.AppendLine($"Experience:{Experience}. XPforNextLevel:{heroLvlCalc.GetExpirienceThreshold(heroLevel.Level + 1)}");

        return sb.ToString().TrimEnd();
    }
}

