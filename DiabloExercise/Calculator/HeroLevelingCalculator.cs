public class HeroLevelingCalculator
{
    //public long NextLevelExperience => heroLevel.Level < heroLevel.MaxLevel ? nextLevelThreshold[heroLevel.Level + 1] : 0;

    private long[] nextLevelThreshold;


    public HeroLevelingCalculator(int heroMaxLevel)
    {
        nextLevelThreshold = new long[heroMaxLevel + 1];
        InitializeNextLevelTresholds();
    }


    public long GetExpirienceThreshold(int targetLevel)
    {
        if (targetLevel <= 0)
        {
            //debug
            Console.WriteLine($"Invalid hero level. It should be in the range of 1 to {nextLevelThreshold.Length - 1}");
            return 0;
        }

        if (targetLevel < nextLevelThreshold.Length)
        {
            return nextLevelThreshold[targetLevel];
        }

        // Debug
        Console.WriteLine("Hero reached the maximum level");
        return 0;
    }


    private void InitializeNextLevelTresholds()
    {
        const double baseXP = 1000;
        const double power = 1.5;
        const double growth = 1.08;

        for (int i = 1; i < nextLevelThreshold.Length; i++)
        {
            int level = i - 1;

            nextLevelThreshold[i] = level == 0
                ? 0
                : (long)Math.Floor(baseXP * Math.Pow(level, power) * Math.Pow(growth, level));
        }
    }

    public void PrintProgressList()
    {
        for (int i = 0; i < nextLevelThreshold.Length; i++)
        {
            Console.WriteLine($"Level{i:D2} - {nextLevelThreshold[i]}");
        }
    }
}

